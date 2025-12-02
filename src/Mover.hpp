#ifndef MOVER_HPP
#define MOVER_HPP

#include <filesystem>
#include <utility>
using namespace std;

namespace photoorganizer {

// Returns {year, month} extracted from EXIF or file‑modification time.
std::pair<int,int> getYearMonth(const std::filesystem::path& file);

// Moves a single file into the destination root, creating YYYY/MM folders.
// Uses fast rename when possible, otherwise falls back to a zero‑copy copy.
void moveFile(const std::filesystem::path& src,
              const std::filesystem::path& dstRoot);

}

#endif // MOVER_HPP