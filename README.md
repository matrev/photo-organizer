# Automated Photo Organizer

The purpose of this project was to automate the organization of my photos as I move them from my SD card to long term storage. You can easily adapt this to fit your own method of storing photos. I choose to store mine in the following format: 

```console
YEAR
    - YEAR-MONTH
        - PHOTO.jpg
2025
    - 2025-04
        - PHOTO.jpg
```

## Usage

### Python
You can run the python script with the output/input directories in the CLI arguments if you prefer that method. 

```bash
matrev@matrev:~$ python3 src/__init__.py /mnt/c/path/to/unorganized/photos /mnt/c/path/to/destination
```

Additionally, this script uses the tkintker package to offer a GUI if you prefer to choose the origin/destination paths that way.

## C++
You can run the following commands to build the executable that will run the executable `PhotoOrganizer`

```bash
cmake -s . -b build
```

```bash
./build/PhotoOrganizer /mnt/c/path/to/unorganized/photos /mnt/c/path/to/destination
```

## Dev Notes

### Python
- Ensure you have the tkintker packages installed on your machine before running this. You might need to reinstall python after doing so to get the tkinter dependencies.


## C++
- Ensure you have the exiv2 and tbb packages installed