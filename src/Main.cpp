#include "Mover.hpp"
#include "../gui/MainWindow.hpp"
#include <iostream>
#include <filesystem>
#include <vector>
#if defined(_WIN32)
    #include <execution>
    #include <algorithm>
#else
    #include <execution>
    #include <algorithm>
#endif

#include <QApplication>
#include <QMainWindow>
#include <QLabel>


int main(int argc, char* argv[]) {
    // if (argc != 3) {
    //     std::cout << "Usage: " << argv[0] << " <source_file> <destination_file>\n";
    //     return 1;
    // }

    // std::filesystem::path src = argv[1];
    // std::filesystem::path dest = argv[2];

    // if (!std::filesystem::exists(src) || !std::filesystem::is_directory(src)) {
    //     std::cerr << "Source directory " << src << " does not exist or is not a directory.\n";
    //     return 1;
    // }

    // // gather all regular files recursively
    // vector<std::filesystem::path> files;
    // for(auto const& entry : std::filesystem::recursive_directory_iterator(src)) {
    //     if(entry.is_regular_file())
    //         files.emplace_back(entry.path());
    // }

    // std::cout << "Found " << files.size() << " files - starting move...\n";

    // std::for_each(
    //     std::execution::par_unseq,
    //     files.begin(),
    //     files.end(), 
    //     [&](const std::filesystem::path& p){
    //         photoorganizer::moveFile(p, dest);
    // });

    // cout << "Done.\n";

    QApplication app(argc, argv);
    QCoreApplication::setApplicationName("Photo Organizer");
    QCoreApplication::setOrganizationName("anthonytrevino");

    MainWindow mainWindow;
    mainWindow.resize(800, 600);
    mainWindow.setWindowTitle("Photo Organizer");
    mainWindow.show();
    // mainWindow.show();
    return app.exec();
}