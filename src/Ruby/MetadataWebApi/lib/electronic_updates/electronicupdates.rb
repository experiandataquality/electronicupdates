# Copyright (c) Experian. All rights reserved.

##
# Downloads data files from the QAS Electronic Updates Metadata REST API.
#
module ElectronicUpdates

  # Base URI of REST API
  @@serviceUri = "https://ws.updates.qas.com/metadata/v2/"

  # Get credentials from environment variables
  @@authToken = "x-api-key " + ENV["EDQ_ElectronicUpdates_Token"]

  # Construct User-Agent HTTP header
  @@userAgent = "MetadataWebApi-Ruby-%s/%s" % [RUBY_PLATFORM, RUBY_VERSION]

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

    @@headers = {
      :"accept" => "json",
      :"content-type" => "application/json; charset=UTF-8",
      :"User-Agent" => @userAgent,
      :"Authorization" => @@authToken
    }

	  @packageResponse = RestClient::Request.execute(:method => :get, :url => "%s%s" % [@@serviceUri, "packages"], :headers => @@headers, :verify_ssl => true)
    return JSON.parse(@packageResponse)

  end

  ##
  # Gets a URI to download the file with the specified name and hash.
  #
  # Arguments:
  #   fileHash: The hash of the file to download. (String)
  #
  # Example:
  #   >> ElectronicUpdates::getDownloadUri("7039d49e15fd4e164e2c07fe76fd61a2")
  #   => "https://..."
  #
  # Returns:
  #   A URI for downloading the specified file, if available; otherwise nil.
  #
  def self.getDownloadUri(fileHash)

    if fileHash === nil or fileHash.empty? then
      raise ArgumentError.new("No file hash specified.")
    end

    self.ensureCredentials()

    @downloadPayload = {
        "FileMd5Hash" => fileHash
    }.to_json

    @@headers = {
      :"accept" => "json",
      :"content-type" => "application/json; charset=UTF-8",
      :"User-Agent" => @userAgent,
      :"Authorization" => @@authToken
    }

	  @downloadResponse = RestClient::Request.execute(:method => :post, :url => "%s%s" % [@@serviceUri, "filelink"], :payload => @downloadPayload, :headers => @@headers, :verify_ssl => true)
    @downloadJson = JSON.parse(@downloadResponse)

    return @downloadJson["DownloadUri"]

  end

  ##
  # Sets the specified credentials to use with the QAS Electronic Updates Metadata REST API.
  #
  # Arguments:
  #   token: The authentication token to use. (String)
  #
  # Example:
  #   >> ElectronicUpdates::setCredentials("MyToken")
  #
  def self.setToken(token)
    @@authToken = "x-api-key " + token
  end

  ##
  # Gets the current user name configured for use with the QAS Electronic Updates Metadata REST API.
  #
  # Example:
  #   >> ElectronicUpdates:getToken()
  #
  def self.getToken()
      return @@authToken;
  end

  private

  ##
  # Ensures that credentials are configured.
  def self.ensureCredentials()

    if @@authToken === nil or @@authToken.empty? then

      raise "Electronic Updates authentication token has not been configured."

    end

  end

end
