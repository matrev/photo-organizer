import shutil
import sys
import os
from PIL import Image
import time

from tkinter.filedialog import askdirectory

def getSourceDestinationPaths() -> tuple[str, str]: 
    """Return (originPath, destinationPath) either from CLI args or GUI dialogs."""
    if len(sys.argv) > 2:
        return sys.argv[1], sys.argv[2]

    source = askdirectory(title="Select Photo Directory To Organize")
    destination   = askdirectory(title="Select Destination Directory")

    return source, destination

def validatePaths(source, destination) -> None:
    """Exit with a clear message if anything is wrong with the supplied paths."""
    for p, kind in ((source, "source"), (destination, "destination")):
        if not os.path.exists(p):
            sys.exit(f"{kind.capitalize()} path '{p}' does not exist.")
        if not os.path.isdir(p):
            sys.exit(f"{kind.capitalize()} path '{p}' is not a directory.")
        if kind == "source" and not os.access(p, os.R_OK):
            sys.exit(f"No read permission for source path '{p}'.")
        if kind == "destination" and not os.access(p, os.W_OK):
            sys.exit(f"No write permission for destination path '{p}'.")

def main():
    sourcePath, destinationPath = getSourceDestinationPaths()
    print(f"Organizing and copying photos from {sourcePath} to {destinationPath}")
    validatePaths(sourcePath, destinationPath)

    countOfPhotosDetected = 0

    with os.scandir(sourcePath) as it:
        for entry in it:
            if entry.name.startswith('._'):
                continue
            if entry.is_file() and entry.name.lower().endswith(('.jpg', '.jpeg')):
                with Image.open(entry.path) as img:
                    exif = img.getexif()
                    if not exif:
                        print(f"No EXIF data for {entry.name}")
                    else:
                        for tag, value in exif.items():
                            # print(tag, value)
                            if tag in [36867, 306]:  #36867 is DateTimeOriginal
                                # extract date in format YYYY-MM
                                date_taken = value.split(' ')[0].replace(':', '-')[0:7]
                                #move files to new directory
                                year = date_taken.split('-')[0]
                                date_dir = os.path.join(destinationPath, year, date_taken)
                                if not os.path.exists(date_dir):
                                    os.makedirs(date_dir)
                                #check if copy already exists in destination
                                if os.path.exists(os.path.join(date_dir, entry.name)):
                                    print(f"File {entry.name} already exists in {date_dir}, skipping copy.")
                                    break
                                else:
                                    shutil.copy2(os.path.join(sourcePath, entry.name), os.path.join(date_dir, entry.name))
                                print(f"Copied {entry.name} to {date_dir}")
                                countOfPhotosDetected += 1
                                break

if __name__ == "__main__":
    # count how long the script takes to run
    startTime = time.perf_counter()
    main()
    endTime = time.perf_counter()
    print(f"Time taken: {endTime - startTime} seconds")

