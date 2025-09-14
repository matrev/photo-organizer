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
You can run the python script with the output/input directories in the CLI arguments if you prefer that method. 

```bash
matrev@matrev:~$ python3 src/__init__.py /mnt/c/path/to/unorganized/photos /mnt/c/path/to/desination
```

Additionally, this script uses the tkintker package to offer a GUI if you prefer to choose the origin/destination paths that way.

## Dev Notes
- Ensure you have the tkintker packages installed on your machine before running this. You might need to reinstall python after doing so to get the tkinter dependencies.
