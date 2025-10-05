cmake_minimum_required(VERSION 3.15)
project(PhotoOrganizer LANGUAGES CXX)

# -------------------------------------------------
# Global settings
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_POSITION_INDEPENDENT_CODE ON)   # useful for shared libs later
set(GCC_COVERAGE_LINK_FLAGS    "-lexiv2 -ltbb") # Linker flags

# -------------------------------------------------
# Find external libraries -------------------------------------------------`
# Exiv2 (EXIF handling) – installed via package manager or vcpkg
find_package(exiv2 REQUIRED)
find_package(TBB REQUIRED)
# -------------------------------------------------
# Include our own headers
include_directories(${PROJECT_SOURCE_DIR}/include)

# -------------------------------------------------
# Add the executable
add_executable(${PROJECT_NAME}
    src/main.cpp
    src/mover.cpp
)

target_link_libraries(${PROJECT_NAME} 
    PRIVATE
        exiv2
        TBB::tbb
)
# target_link_libraries(${PROJECT_NAME} exiv2::exiv2)
# add_library(exiv2::exiv2) # To silence CMake warnings
# set_target_properties(${PROJECT_NAME} PROPERTIES LINK_FLAGS "${GCC_COVERAGE_LINK_FLAGS}")

target_include_directories(${PROJECT_NAME} PRIVATE
    ${PROJECT_SOURCE_DIR}/external   # taskflow.hpp lives here
)

# -------------------------------------------------
# Install rules (used by CPack)
install(TARGETS ${PROJECT_NAME}
        RUNTIME DESTINATION bin)

# -------------------------------------------------
# Packaging with CPack -------------------------------------------------
include(InstallRequiredSystemLibraries)

set(CPACK_PACKAGE_NAME "PhotoOrganizer")
set(CPACK_PACKAGE_VENDOR "MarkTrevino")
set(CPACK_PACKAGE_DESCRIPTION_SUMMARY "Fast EXIF‑based photo sorter")
set(CPACK_PACKAGE_VERSION "1.0.0")
set(CPACK_PACKAGE_CONTACT "dev@marktrevino.com")

# Windows – generate an NSIS installer (you need NSIS installed)
if(WIN32)
    set(CPACK_GENERATOR "NSIS")
    set(CPACK_NSIS_DISPLAY_NAME "Photo Organizer")
    set(CPACK_NSIS_MUI_ICON "${CMAKE_CURRENT_SOURCE_DIR}/resources/icon.ico")
endif()

# macOS – create a .dmg
if(APPLE)
    set(CPACK_GENERATOR "DragNDrop")
    set(CPACK_DMG_BACKGROUND_IMAGE "${CMAKE_CURRENT_SOURCE_DIR}/resources/dmg_background.png")
endif()

# Linux – DEB & RPM
if(UNIX AND NOT APPLE)
    set(CPACK_GENERATOR "DEB;RPM")
    set(CPACK_DEBIAN_PACKAGE_MAINTAINER "Your Name <you@example.com>")
endif()

include(CPack)