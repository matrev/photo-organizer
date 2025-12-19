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

## Building and Deployment

This application is built with Avalonia UI and .NET, making it cross-platform compatible. You can build executables for Windows, macOS, and Linux.

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- For cross-platform builds, you may need additional SDKs depending on your host platform

### Quick Build Commands

#### Windows (PowerShell)
```powershell
# Build for current platform
dotnet publish -c Release --self-contained

# Build for specific platform
dotnet publish -c Release -r win-x64 --self-contained --output ./publish/win-x64
dotnet publish -c Release -r linux-x64 --self-contained --output ./publish/linux-x64
dotnet publish -c Release -r osx-x64 --self-contained --output ./publish/osx-x64
```

#### Linux/macOS (Bash)
```bash
# Build for current platform
dotnet publish -c Release --self-contained

# Build for specific platform
dotnet publish -c Release -r linux-x64 --self-contained --output ./publish/linux-x64
dotnet publish -c Release -r win-x64 --self-contained --output ./publish/win-x64
dotnet publish -c Release -r osx-arm64 --self-contained --output ./publish/osx-arm64
```

### Automated Build Scripts

Use the provided build scripts for convenience:

#### Windows
```powershell
# Build for all platforms
.\build.ps1 -AllPlatforms

# Build for specific platform
.\build.ps1 -TargetRid win-x64

# Build with custom output directory
.\build.ps1 -TargetRid linux-x64 -OutputDir ./my-builds
```

#### Linux/macOS
```bash
# Make script executable (first time only)
chmod +x build.sh

# Build for all platforms
./build.sh all

# Build for specific platform
./build.sh linux-x64

# Build with custom output directory
./build.sh win-x64 ./my-builds
```

### Supported Platforms

The application can be built for the following platforms:

- **Windows**: `win-x64`, `win-x86`, `win-arm64`
- **macOS**: `osx-x64` (Intel), `osx-arm64` (Apple Silicon)
- **Linux**: `linux-x64`, `linux-arm64`, `linux-musl-x64`, `linux-musl-arm64`

### Distribution

The built executables are self-contained and include the .NET runtime, so users don't need to install .NET separately. Each executable is typically 50-100MB in size.

### System Requirements

- **Windows**: Windows 10 version 1607 or later
- **macOS**: macOS 10.12 Sierra or later
- **Linux**: Most modern distributions (glibc or musl-based)

### Troubleshooting

- If builds fail, ensure you have the latest .NET SDK installed
- For Linux builds on Windows, you may need additional components
- Check the [Avalonia documentation](https://docs.avaloniaui.net/) for platform-specific issues