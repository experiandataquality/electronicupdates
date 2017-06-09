# Experian Data Quality Electronic Updates Metadata REST API C# Sample Integration Code

## Build Status

| **Build Status**| **Linux/OS X** | **Windows** |
|:-:|:-:|:-:|
| **C#** |  ![Build Status](https://edq-repo.visualstudio.com/_apis/public/build/definitions/034cfbed-e5ea-4de5-898a-85fbf66debfb/550/badge) | ![Build Status](https://edq-repo.visualstudio.com/_apis/public/build/definitions/034cfbed-e5ea-4de5-898a-85fbf66debfb/548/badge) |

## Overview

This folder contains a C# application that can be used to determine what data files are available to download for an account, generate download URLs for the files and then download them onto the local file system.

Further documentation of the application is provided by the comments in the source code itself.

*This documentation describes the application as found in this [Git repository](https://github.com/experiandataquality/electronicupdates). It may no longer apply if you modify the sample code.*

## Compilation

The following prerequisites are required to compile and debug the application:

### Runtime

Choose your preferred runtime from either of below:

* .Net Framework 4.6 (Windows 7+)
  * [Visual Studio Community](https://www.microsoft.com/net/core)
  * You can also use Visual Studio 2017 Professional/Enterprise
* .Net Core
  * [Get Started](https://www.microsoft.com/net/core#windowscmd)
  * [.Net Core SDK free download list](https://www.microsoft.com/net/download/core)

By default the code is targetting .Net Core but you can change the build target to work on top of the .Net Framework.

#### Supported platforms

* Windows 7 SP1*, 8, 10
* Windows Server 2012 R2, 2016
* Red Hat Enterprise Linux 7.2
* CentOS 7.1+
* Debian 8.2+
* Fedora 23, 24
* Linux Mint 17.1, 18
* OpenSUSE 13.2, 42.1
* Oracle Linux 7.1
* Ubuntu 14.04 & 16.04*
* Mac OS X 10.11, 10.12

 \* Actively tested platforms

### Command line compliation

To compile the application, you can do any of the following:

```batchfile
dotnet restore MetadataWebApi.sln
dotnet build MetadataWebApi.sln
```

## Running the compiled application

### Setup

To set up the application for usage you could either:

1. Configure the token in the ```app.config.json``` configuration file. 
1. Set your authentication token in the ```EDQ_ElectronicUpdates_Token``` environment variable just before running the application (**recommended**);

```js
 {
  "appSettings": {
    "token": "",
    "downloadRootPath": "EDQData",
    "validateDownloads": "true",
    "serviceUri": "https://ws.updates.qas.com/metadata/V2/"
  }
}
```

Other approaches are possible but are considered outside the scope of this documentation.
Note please remember to maintain the security of your token, either apply it via environment varible only at the point of use or ensure that you secure access to your configuration files.

### Usage

To run the application, execute the following command from the directory containing the compiled executable:

#### Linux/OS X

```sh
dotnet ./MetadataWebApi.dll
```

#### Windows

For the .Net Framework exe

```batchfile
MetadataWebApi.exe
```
or for the .Net core app

```batchfile
dotnet .\MetadataWebApi.dll
```

## Example Usage

Below is an example set of commands that could be run on Windows to download all the latest data files from Electronic Updates onto the local machine into a ```EDQData``` directory in the same directory as the application

```batchfile
set EDQ_ElectronicUpdates_Token=MyToken
MetadataWebApi.exe
set EDQ_ElectronicUpdates_Token=
```