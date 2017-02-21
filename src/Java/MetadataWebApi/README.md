# Experian Data Quality Electronic Updates Metadata REST API Java Sample Integration Code

## Overview

This directory contains a Java application that can be used to determine what data files are available to download for an account, generate download URLs for the files and then download them onto the local file system.

Further documentation of the application is provided by the comments in the source code itself.

*This documentation describes the application as found in this [Git repository](https://github.com/experiandataquality/electronicupdates). It may no longer apply if you modify the sample code.*

## Prerequisites

### Compilation and Debugging

The following prerequisites are required to compile and debug the application:

 * [Java](https://java.com/en/download/) 1.8.0 (or later);
 * [Apache Ant](http://ant.apache.org/bindownload.cgi) 1.9.3 (or later);
 * [Eclipse](https://eclipse.org/downloads/) 3.8.1 (or later).

### Runtime

The following prerequisites are required to run the compiled application:

 * [Java](https://java.com/en/download/) 1.7.0 (or later).

## Compliation

To compile the application, you can do any of the following:

### Linux

 * Import the project into Eclipse;
 * Run ```./build.sh``` from the terminal.

### Windows

 * Import the project into Eclipse.

## Setup

To set up the application, set your token in the ```EDQ_ElectronicUpdates_Token``` environment variable just before running the application.

Other approaches for loading the credentials are possible but are considered outside the scope of this documentation.

## Usage

To run the application, execute the following command from the directory containing the compiled JAR file:

### Linux

```sh
java -jar MetadataWebApi.jar
```

### Windows

```batchfile
java -jar MetadataWebApi.jar
```

## Example Usage

Below is an example set of commands that could be run on Linux to download all the latest data files from Electronic Updates onto the local machine into a ```ExperianData``` directory in the same directory as the application

```sh
export EDQ_ElectronicUpdates_Token=MyToken
java -jar MetadataWebApi.jar
```

## Compatibility

This application has been compiled and tested on the following platforms:

 * Java 1.8.0_121 (x64), Eclipse Java Neon x64 and Apache Ant 1.10.1 on Windows 7 Enterprise (Build 7601).
