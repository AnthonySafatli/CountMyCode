# CountMyCode

A command-line application that analyzes a code repository and provides an audit complete with charts and statistics about the code in your project.

## Installation

1. Download the executable from the [releases](https://github.com/AnthonySafatli/CountMyCode/releases) section.
2. Unzip the files and move them to a suitable location (e.g., `C:/Program Files/CountMyCode` or `C:/CountMyCode`).
3. Add the executable to your system PATH.
4. Open a command line and run the executable.

## Usage

1. Run the executable, optionally passing the path to the folder you want to analyze.
2. Choose which files and folders to include or ignore.
3. If there are unrecognized file types, assign them names.
4. View your audit results!

## Building

If you want to build the project yourself, run the command

```
dotnet publish -c Release -r <your-os> --self-contained true -p:PublishSingleFile=true -o publish
```

Your executable will be located in `/CountYourCode/Publish`

## License

This codebase uses the GNU GPL v3.0 License. More information [here](https://github.com/AnthonySafatli/Pixart/blob/main/LICENSE.txt).

## Contact

Anthony Safatli  
📧 anthonysafatli@dal.ca  
🌐 [anthonysafatli.ca](https://anthonysafatli.ca)  
✉️ [Contact Form](https://anthonysafatli.ca/Contact)
