import shutil
import sys
import os
from PIL import Image

from tkinter.filedialog import askdirectory

if len(sys.argv) > 1:
    originPath = sys.argv[1]
    destinationPath = sys.argv[2]
else:
    originPath = askdirectory(title="Select Photo Directory To Organize")
    destinationPath = askdirectory(title="Select Destination Directory")

print(f"Organizing and copying photos from {originPath} to {destinationPath}")

#check if paths are valid
if not os.path.exists(originPath):
    print(f"Origin path {originPath} does not exist.")
    sys.exit(1)
if not os.path.exists(destinationPath):
    print(f"Destination path {destinationPath} does not exist.")
    sys.exit(1)

#check if you have read/write permissions for the paths
if not os.access(originPath, os.R_OK):
    print(f"No read access to origin path {originPath}.")
    sys.exit(1)
if not os.access(destinationPath, os.W_OK):
    print(f"No write access to destination path {destinationPath}.")
    sys.exit(1)

os.chdir(originPath)
date_dict = {}
for item in os.listdir('.'):
    #ignore files that start with ._, my camera creates these files
    if item.startswith('._'):
        continue
    if item.lower().endswith(('.jpg', '.jpeg')):
        with Image.open(item) as img:
            exif = Image.open(item)._getexif()
        if not exif:
            print(f"No EXIF data for {item}")
        else:
            for tag, value in exif.items():
                if tag in [36867]:  #36867 is DateTimeOriginal
                    # print(f"{tag}: {value}")
                    # extract date in format YYYY-MM
                    date_taken = value.split(' ')[0].replace(':', '-')[0:7]
                    if date_taken not in date_dict:
                        date_dict[date_taken] = [item]
                    date_dict[date_taken].append(item)

# print the dictionary
# for date, files in date_dict.items():
#     print(f"{date}: {files}")

# create directories and copy files
for date, files in date_dict.items():
    year = date.split('-')[0]
    date_dir = os.path.join(destinationPath, year, date)
    if not os.path.exists(date_dir):
        os.makedirs(date_dir)
    for file in files:
        shutil.copy2(os.path.join(originPath, file), os.path.join(date_dir, file))
        print(f"Copied {file} to {date_dir}")