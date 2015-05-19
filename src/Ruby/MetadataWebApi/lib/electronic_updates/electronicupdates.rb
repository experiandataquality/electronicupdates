# Copyright (c) Experian. All rights reserved.

##
# Downloads data files from the QAS Electronic Updates Metadata REST API.
#
module ElectronicUpdates

  # Base URI of REST API
  @@serviceUri = "https://ws.updates.qas.com/metadata/v1/"

  # Get credentials from environment variables
  @@userName = ENV["QAS_ElectronicUpdates_UserName"]
  @@password = ENV["QAS_ElectronicUpdates_Password"]

  # Construct User-Agent HTTP header
  @@userAgent = "MetadataWebApi-Ruby-%s/%s" % [RUBY_PLATFORM, RUBY_VERSION]

  @@headers = {
    :"accept" => "json",
    :"content-type" => "application/json; charset=UTF-8",
    :"User-Agent" => @userAgent
  }

  ##
  # Gets the available packages that can be downloaded.
  #
  # Example:
  #   >> ElectronicUpdates::getPackages()
  #   => { "Packages": [ ... ] }
  #
  # Returns:
  #   A map containing the available packages.
  #
  def self.getPackages

    self.ensureCredentials()

    @packagesPayload = {
      "usernamePassword" => {
        "UserName" => @@userName,
        "Password" => @@password
      }
    }.to_json

    @packageResponse = RestClient.post "%s%s" % [@@serviceUri, "packages"], @packagesPayload, @@headers
    return JSON.parse(@packageResponse)

  end

  ##
  # Gets a URI to download the file with the specified name and hash.
  #
  # Arguments:
  #   fileName: The name of the file to download. (String)
  #   fileHash: The hash of the file to download. (String)
  #
  # Example:
  #   >> ElectronicUpdates::getDownloadUri("MyFile.txt", "7039d49e15fd4e164e2c07fe76fd61a2")
  #   => "https://..."
  #
  # Returns:
  #   A URI for downloading the specified file, if available; otherwise nil.
  #
  def self.getDownloadUri(fileName, fileHash)

    if fileName === nil or fileName.empty? then
      raise ArgumentError.new("No file name specified.")
    end

    if fileHash === nil or fileHash.empty? then
      raise ArgumentError.new("No file hash specified.")
    end

    self.ensureCredentials()

    @downloadPayload = {
      "usernamePassword" => {
        "UserName" => @@userName,
        "Password" => @@password
      },
      "fileDownloadRequest" => {
        "FileName" => fileName,
        "FileMd5Hash" => fileHash
      }
    }.to_json
    
    @downloadResponse = RestClient.post "%s%s" % [@@serviceUri, "filedownload"], @downloadPayload, @@headers
    @downloadJson = JSON.parse(@downloadResponse)
    
    return @downloadJson["DownloadUri"]

  end

  ##
  # Sets the specified credentials to use with the QAS Electronic Updates Metadata REST API.
  #
  # Arguments:
  #   userName: The user name to use. (String)
  #   password: The password to use. (String)
  #
  # Example:
  #   >> ElectronicUpdates::setCredentials("MyUserName", "MyPassword")
  #
  def self.setCredentials(userName, password)
    @@userName = userName
    @@password = password
  end

  ##
  # Gets the current user name configured for use with the QAS Electronic Updates Metadata REST API.
  #
  # Example:
  #   >> ElectronicUpdates:getUserName()
  #
  def self.getUserName()
      return @@userName;
  end

  private

  ##
  # Ensures that credentials are configured.
  def self.ensureCredentials()

    if @@userName === nil or @@userName.empty? or @@password === nil or @@password.empty? then
    
      raise "No QAS Electronic Updates credentials are configured."

    end

  end

end
