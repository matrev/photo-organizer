#include <exiv2/exiv2.hpp>
#include <fstream>
#include <iostream>
#include <system_error>

#if defined(_WIN32)
    #include <windows.h>
#else
    #include <fcntl.h>
    #include <unistd.h>
    #include <sys/sendfile.h>
#endif

namespace photoorganizer {
    static constexpr size_t BUFFER_SIZSE = 1 << 20;// 16 * 1024 * 1024; // 16MB buffer size

    pari<int, int> getYearMonth(const filesystem::path& file) {
        try {
            auto img = Exiv2::ImageFactory::open(file.string());
            img->readMetadata();
            const Exiv2::ExifData& exif = img->exifData();
            const auto dt = exif["Exif.photo.DateTimeOriginal"]; // "2023:10:05 14:23:11"
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
        std::time_t tt = decltype(ft)::clock::to_time_t(ft);
        std::tm tm = *std::localtime(&tt);
        return {tm.tm_year + 1900, tm.tm_mon + 1};
    }
}