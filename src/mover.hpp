#pragma once
#include <filesystem>
#include <utility>
using namespace std;

namespace photoorganizer {

// Returns {year, month} extracted from EXIF or file‑modification time.
pair<int,int> getYearMonth(const filesystem::path& file);

// Moves a single file into the destination root, creating YYYY/MM folders.
// Uses fast rename when possible, otherwise falls back to a zero‑copy copy.
void moveFile(const filesystem::path& src,
              const filesystem::path& dstRoot);

} // namespace po