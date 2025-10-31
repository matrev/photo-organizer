#include "mover.hpp"
#include <filesystem>
#include <exiv2/exiv2.hpp>
#include <fstream>
#include <iostream>
#include <system_error>
#include <chrono>
#include <ctime>
#include <sys/stat.h>

#if defined(_WIN32)
    #include <windows.h>
#else
    #include <fcntl.h>
    #include <unistd.h>
    #include <sys/sendfile.h>
#endif

namespace photoorganizer {
    static constexpr size_t BUFFER_SIZE = 16 * 1024 * 1024; // 16MB buffer size

    pair<int, int> getYearMonth(const std::filesystem::path& file) {
        try {
            auto img = Exiv2::ImageFactory::open(file.string());
            img->readMetadata();
            Exiv2::ExifData& exif = img->exifData();
            auto dt = exif["Exif.Photo.DateTimeOriginal"]; // "2023:10:05 14:23:11"
            if (!dt.toString().empty()) {
                std::string s = dt.toString();
                int y = std::stoi(s.substr(0,4));
                int m = std::stoi(s.substr(5,2));
                return {y,m};
            }
        }
        catch (const Exiv2::Error& e) {
            std::cerr << "Error reading EXIF data from " << file << ": " << e.what() << "\n";
        }

        // Fallback: use last write time
        auto ft = std::filesystem::last_write_time(file);
        auto system_time_point = std::chrono::time_point_cast<std::chrono::system_clock::duration>(
            ft - std::filesystem::file_time_type::clock::now() + std::chrono::system_clock::now()
        );
        std::time_t tt = std::chrono::system_clock::to_time_t(system_time_point);
        std::tm tm{};

        // Use thread-safe localtime variants: localtime_s on Windows, localtime_r on POSIX
        #if defined(_WIN32)
            ::localtime_s(&tm, &tt);
        #else
            ::localtime_r(&tt, &tm);
        #endif
        return {tm.tm_year + 1900, tm.tm_mon + 1};
    }

    void moveFile(const std::filesystem::path& src, const std::filesystem::path& destRoot)
    {
        auto [yr, mo] = getYearMonth(src);
        std::filesystem::path destDir = destRoot /
                                    std::to_string(yr) /
                                    (mo < 10 ? "0"+std::to_string(mo)
                                                : std::to_string(mo));
        std::error_code ec;
        std::filesystem::create_directories(destDir, ec);
        if (ec) {
            std::cerr << "Failed to create dir " << destDir << ": " << ec.message() << "\n";
            return;
        }

        std::filesystem::path finalDest = destDir / src.filename();

        // try {
            
        //     std::filesystem::rename(src, finalDest, ec);
        // } catch (const std::filesystem::filesystem_error& e) {
            // fallback: cross-volume copy
            // std::filesystem::copy_file(src, finalDest, std::filesystem::copy_options::overwrite_existing);
            // std::filesystem::remove(src);
        // }
        // if (!ec) return;   // success!

        // // 2️⃣ Cross‑volume copy – use platform‑specific fast path
        #if defined(_WIN32)
            // Windows: CopyFileEx with NO_BUFFERING flag
            BOOL ok = CopyFileExW(src.c_str(), dst.c_str(),
                                nullptr, nullptr, FALSE,
                                COPY_FILE_NO_BUFFERING);
            if (ok) std::filesystem::remove(src, ec);
            if (!ok) std::cerr << "Copy failed for " << src << "\n";
        #else
            // POSIX: sendfile loop (1 MiB chunks)
            int in  = open(src.c_str(), O_RDONLY);
            int out = open(finalDest.c_str(), O_WRONLY | O_CREAT | O_TRUNC, 0644);
            if (in < 0 || out < 0) {
                std::cerr << "Open failure for " << src << "\n";
                if (in >= 0) ::close(in);
                if (out >= 0) ::close(out);
                return;
            }

            struct stat st{};
            fstat(in, &st);
            off_t offset = 0;
            while (offset < st.st_size) {
                ssize_t sent = ::sendfile(out, in, &offset, BUFFER_SIZE);
                if (sent <= 0) break;   // error or EOF
            }
            ::close(in);
            ::close(out);
            std::filesystem::remove(src, ec);
        #endif
        }
}