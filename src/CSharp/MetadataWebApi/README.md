# QAS Electronic Updates Metadata REST API C# Sample Integration Code

| | Build Status |
|:-:|:-:|
| **Linux** | [![Build status](https://img.shields.io/travis/experiandataquality/electronicupdates/master.svg)](https://travis-ci.org/experiandataquality/electronicupdates) |
| **OS X** | [![Build status](https://img.shields.io/travis/experiandataquality/electronicupdates/master.svg)](https://travis-ci.org/experiandataquality/electronicupdates) |
| **Windows** | [![Build status](https://img.shields.io/appveyor/ci/experiandataquality/electronicupdates/master.svg)](https://ci.appveyor.com/project/experiandataquality/electronicupdates) |

## Overview

This directory contains a C# application that can be used to determine what data files are available to download for an account, generate download URLs for the files and then download them onto the local file system.

Further documentation of the application is provided by the comments in the source code itself.

*This documentation describes the application as found in this [Git repository](https://github.com/experiandataquality/electronicupdates). It may no longer apply if you modify the sample code.*

## Prerequisites

### Compilation and Debugging

The following prerequisites are required to compile and debug the application:

#### Linux

 * [Mono](http://www.mono-project.com/download/) 4.0.4 (or later);
 * [Mono Develop](http://www.monodevelop.com/download/) 5.9.0 (or later).

#### Windows

 * Microsoft Windows 7 SP1 (or later);
 * One of the following editions of [Microsoft Visual Studio](https://www.visualstudio.com/en-us/downloads/download-visual-studio-vs.aspx):
   * Visual Studio (Community, Professional or Enterprise) 2015 (or later).

### Runtime

The following prerequisites are required to run the compiled application:

### Linux/OS X

 * [Mono](http://www.mono-project.com/download/) 4.0.4 (or later).

### Windows

 * Microsoft Windows 7 SP1 (or later);
 * Microsoft .NET Framework 4.6 (or later).

## Compliation

To compile the application, you can do any of the following:

### Linux/OS X

 * Open ```MetadataWebApi.sln``` in MonoDevelop;
 * Run ```./build.sh``` from the terminal.

### Windows

 * Open ```MetadataWebApi.sln``` in Visual Studio;
 * Run ```Build.cmd``` from the command prompt.

## Setup

To set up the application for usage you could either:

 1. Set your credentials in the ```QAS_ElectronicUpdates_UserName``` and ```QAS_ElectronicUpdates_Password``` environment variables just before running the application (**recommended**);
 1. Configure the credentials in the ```MetadataWebApi.exe.config``` configuration file. If you place your credentials in this file ensure that you have adequate security controls in place to protect your account credentials as they are sensitive. 

Other approaches are possible but are considered outside the scope of this documentation.

## Usage

To run the application, execute the following command from the directory containing the compiled executable:

### Linux/OS X

```sh
./MetadataWebApi.exe
```

### Windows

```batchfile
MetadataWebApi.exe
```

## Example Usage

Below is an example set of commands that could be run on Windows to download all the latest data files from QAS Electronic Updates onto the local machine into a ```QASData``` directory in the same directory as the application

```batchfile
set QAS_ElectronicUpdates_UserName=MyUserName
set QAS_ElectronicUpdates_Password=MyPassword
MetadataWebApi.exe
```

## Compatibility

This sample code has been compiled and tested on the following platforms:

 * MonoDevelop 5.9.0 on Ubuntu 14.04.2 LTS;
 * Visual Studio Enterprise 2015 on Windows 10 (OS Build 10586.3).
