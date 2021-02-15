# Experian Data Quality Electronic Updates Metadata REST API Powershell Script

## Overview

This directory contains a Powershell script that can be used to determine what data files are available to download for an account, generate download URLs for the files and then download them onto the local file system.

Further documentation of the script is provided by the comments in the Powershell script itself.

*This documentation describes the script as found in this [Git repository](https://github.com/experiandataquality/electronicupdates). It may no longer apply if you modify the sample code.*

## Prerequisites

 * [Powershell] 5.0 (or later);

## Setup

To set up the script for usage you could either:

 1. Set your token in the ```EDQ_ElectronicUpdates_Token``` environment variable just before running the script (**recommended**);
 1. Hard-code the token into the script. This approach is **not** recommended as the token is stored in plaintext and could represent a security risk.

Other approaches are possible but are considered outside the scope of this documentation.

### Windows

```batchfile
powershell -file .\experianupdate.ps1
```

## Example Usage

Below is an example set of commands that could be run on Linux to download all the latest data files from Electronic Updates onto the local machine into a ```EDQData``` directory in the same directory as the script:

```powershell
$env:EDQ_ElectronicUpdates_Token="MyToken"
.\experianupdate.ps1
```

## Compatibility

This script was tested with the following Powershell versions and platforms:

 * Powershell 5.0 on Windows 10 Enterprise (Build 1607);
