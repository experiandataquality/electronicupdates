# QAS Electronic Updates Metadata REST API Ruby Gem

## Overview

This directory contains a Ruby gem that can be used to determine what data files are available to download for an account, generate download URLs for the files and then download them onto the local file system.

Further documentation of the script is provided by the comments in the Ruby code itself.

*This documentation describes the gem as found in this [Git repository](https://github.com/experiandataquality/electronicupdates). It may no longer apply if you modify the sample code.*

## Prerequisites

 * [Ruby](https://www.ruby-lang.org/en/downloads/) 1.9.3 (or later);
 * The [rest-client](https://rubygems.org/gems/rest-client/) Ruby gem (or later);
 * The [minitest](https://rubygems.org/gems/minitest/) Ruby gem (or later, for testing).

## Setup

To set up the gem for usage you could either:

 1. Set your authentication token in the ```EDQ_ElectronicUpdates_Token``` environment variable just before running the script (**recommended**);
 1. Hard-code the token into the code that uses the gem. This approach is **not** recommended as the token is stored in plaintext and could represent a security risk.

Other approaches are possible but are considered outside the scope of this documentation.

## Building

To build and test the gem, execute the following command from the directory containing the Ruby code:

```sh
bundle
rake
```

## Example Usage

Below is an example Ruby script that could be used on a Linux machine to download all the latest data files from QAS Electronic Updates onto the local machine.

First set the token to use:

### Linux/OS X

```sh
export EDQ_ElectronicUpdates_Token=MyToken
```

### Windows

```batchfile
set EDQ_ElectronicUpdates_Token=MyToken
```

Then you can run a ruby script similar to this:

```ruby
#!/usr/bin/env ruby
require 'electronic_updates'

puts "Getting available packages..."
@packages = ElectronicUpdates.getPackages()

@packages.each do |packageGroup|

  @packageGroupCode = packageGroup["PackageGroupCode"]
  @vintage = packageGroup["Vintage"]

  packageGroup["Packages"].each do |package|

    @packageCode = package["PackageCode"]

    package["Files"].each do |file|

      @fileName = file["FileName"]
      @fileHash = file["Md5Hash"]
      @fileLength = file["Size"]

      # Get download URI for file
      @downloadUri = ElectronicUpdates.getDownloadUri(@fileName, @fileHash)

      # Download the file to the file system
      # RestClient.get @downloadUri

    end

  end

end
```

## Compatibility

This script was tested with the following Ruby versions and platforms:

 * Ruby 1.9.3p484 on Ubuntu 14.04.2 LTS;
 * Ruby 2.1.0p0 on OS X Yosemite (10.10.2);
 * Ruby 2.2.2p95 on Windows 8.1 (Build 9600).
