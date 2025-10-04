# create tests for main.py using pytest
import os
import shutil
import pytest
from PIL import Image
from main import organize_photos
from unittest import mock
from io import BytesIO
import tempfile
from tkinter.filedialog import askdirectory
from contextlib import chdir

@pytest.fixture
def temp_dirs():
    origin = tempfile.mkdtemp()
    destination = tempfile.mkdtemp()
    yield origin, destination
    shutil.rmtree(origin)
    shutil.rmtree(destination)
@pytest.fixture
def sample_images(temp_dirs):
    origin, _ = temp_dirs
    img1 = Image.new('RGB', (100, 100), color = 'red')
    img2 = Image.new('RGB', (100, 100), color = 'blue')
    img3 = Image.new('RGB', (100, 100), color = 'green')
    img1_path = os.path.join(origin, 'img1.jpg')
    img2_path = os.path.join(origin, 'img2.jpg')
    img3_path = os.path.join(origin, 'img3.jpg')
    img1.save(img1_path)
    img2.save(img2_path)
    img3.save(img3_path)
    yield [img1_path, img2_path, img3_path]
    os.remove(img1_path)
    os.remove(img2_path)
    os.remove(img3_path)
def test_organize_photos(temp_dirs, sample_images):
    origin, destination = temp_dirs
    with mock.patch('tkinter.filedialog.askdirectory', side_effect=[origin, destination]):
        organize_photos()
    # Check if directories are created
    dirs = os.listdir(destination)
    assert len(dirs) > 0
    # Check if files are copied
    for dir in dirs:
        dir_path = os.path.join(destination, dir)
        files = os.listdir(dir_path)
        assert len(files) > 0
        for file in files:
            assert os.path.exists(os.path.join(dir_path, file))
            
def test_invalid_paths():
    with mock.patch('tkinter.filedialog.askdirectory', side_effect=['/invalid/origin', '/invalid/destination']):
        with pytest.raises(SystemExit):
            organize_photos()
def test_no_exif_data(temp_dirs):
    origin, destination = temp_dirs
    img = Image.new('RGB', (100, 100), color = 'yellow')
    img_path = os.path.join(origin, 'no_exif.jpg')
    img.save(img_path)
    with mock.patch('tkinter.filedialog.askdirectory', side_effect=[origin, destination]):
        organize_photos()
    dirs = os.listdir(destination)
    assert len(dirs) == 0
    os.remove(img_path)

def test_non_jpg_files(temp_dirs):
    origin, destination = temp_dirs
    txt_path = os.path.join(origin, 'file.txt')
    with open(txt_path, 'w') as f:
        f.write('This is a text file.')
    with mock.patch('tkinter.filedialog.askdirectory', side_effect=[origin, destination]):
        organize_photos()
    dirs = os.listdir(destination)
    assert len(dirs) == 0
    os.remove(txt_path)