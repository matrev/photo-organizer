import os
from PIL import Image
from src import main
from unittest import mock
from datetime import datetime
from contextlib import contextmanager

@contextmanager
def change_dir(destination):
    """Context manager to change the current working directory"""
    original_dir = os.getcwd()
    os.chdir(destination)
    try:
        yield
    finally:
        os.chdir(original_dir) 

def create_test_image(path, date_taken):
    """Create a simple JPEG image with EXIF date taken metadata"""
    img = Image.new('RGB', (100, 100), color = 'red')
    # Add EXIF data
    exif_dict = {36867: date_taken.strftime("%Y:%m:%d %H:%M:%S")}
    exif_bytes = Image.Exif()
    for tag, value in exif_dict.items():
        exif_bytes[tag] = value
    img.save(path, exif=exif_bytes)


def test_main(tmp_path):
    source = tmp_path / "source"
    destination = tmp_path / "destination"
    source.mkdir()
    destination.mkdir()

    # Create test images with different dates
    create_test_image(source / "img1.jpg", datetime(2020, 5, 17, 10, 0, 0))
    create_test_image(source / "img2.jpg", datetime(2021, 6, 18, 11, 0, 0))
    create_test_image(source / "img3.jpg", datetime(2020, 7, 19, 12, 0, 0))
    print(f"Source directory contents: {list(source.iterdir())}")
    print(f"Destination directory contents before: {list(destination.iterdir())}")
    
    # Mock the getSourceDestinationPaths function to return our test paths
    with mock.patch('src.getSourceDestinationPaths', return_value=(str(source), str(destination))):
        main()

    print(f"Destination directory contents after: {list(destination.iterdir())}")

    # Check that images have been copied to the correct directories
    assert (destination / "2020" / "2020-05").exists()
    assert (destination / "2020" / "2020-05" / "img1.jpg").exists()
    assert (destination / "2021" / "2021-06").exists()
    assert (destination / "2021" / "2021-06" / "img2.jpg").exists()
    assert (destination / "2020" / "2020-07").exists()
    assert (destination / "2020" / "2020-07" / "img3.jpg").exists()