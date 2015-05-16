/**
 * Copyright (c) Experian. All rights reserved.
 */

package com.experian.qas.updates.metadata.webapi.v1;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.net.URL;
import java.nio.channels.Channels;
import java.nio.channels.ReadableByteChannel;
import java.security.MessageDigest;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;

/**
 * Java sample code for the QAS Electronic Updates Metadata Web API.
 * @author Experian QAS
 */
public class Program {

    /**
     * The user name to use to authenticate with the web service.
     */
    private static final String userName = System.getenv("QAS_ElectronicUpdates_UserName");

    /**
     * The password to use to authenticate with the web service.
     */
    private static final String password = System.getenv("QAS_ElectronicUpdates_Password");

    /**
     * The root folder to download data to.
     */
    private static final String rootDownloadPath = "QASData";

    /**
     * Calculates the MD5 hash of the specified file.
     * @param fileName The name of the file to calculate the MD5 hash of.
     * @return The MD5 hash of the specified file.
     * @throws Exception
     */
    private static String calculateMD5Hash(String fileName) throws Exception {

        MessageDigest md = MessageDigest.getInstance("MD5");
        FileInputStream stream = new FileInputStream(fileName);

        try {

            byte[] dataBytes = new byte[1024];

            int bytesRead = 0;

            while ((bytesRead = stream.read(dataBytes)) > -1) {
                md.update(dataBytes, 0, bytesRead);
            }

            byte[] hashBytes = md.digest();

            StringBuffer sb = new StringBuffer();

            for (int i = 0; i < hashBytes.length; i++) {

                String hex = Integer.toHexString(0xff & hashBytes[i]);

                if(hex.length() == 1) {
                    sb.append('0');
                }

                sb.append(hex);
            }

            return sb.toString();
        }
        finally {
            stream.close();
        }
    }

    /**
     * Creates the credentials JSON to use to authenticate with the web service.
     * @return A JSON object containing the user's credentials.
     */
    @SuppressWarnings("unchecked")
    private static JSONObject createCredentials() {

        JSONObject result = new JSONObject();

        result.put("UserName", userName);
        result.put("Password", password);

        return result;
    }

    /**
     * Creates a String containing the JSON request to download the specified data file.
     * @param dataFile The data file to get the download request for.
     * @return A String containing the JSON to download the specified data file.
     */
    @SuppressWarnings("unchecked")
    private static String createFileDownloadRequest(DataFile dataFile) {

        JSONObject credentials = createCredentials();

        JSONObject fileDownloadRequest = new JSONObject();
        fileDownloadRequest.put("FileName", dataFile.getFileName());
        fileDownloadRequest.put("FileMd5Hash", dataFile.getMD5Hash());

        JSONObject result = new JSONObject();

        result.put("usernamePassword", credentials);
        result.put("fileDownloadRequest", fileDownloadRequest);

        return result.toJSONString();
    }

    /**
     * Creates a HttpPost request object for the specified URI and body.
     * 
     * @param uri The URI of the POST request.
     * @param body The body of the POST request.
     * @return The created HttpPost instance.
     * @throws Exception
     */
    private static HttpPost createHttpPostRequest(String uri, String body) throws Exception {

        HttpPost result = new HttpPost(uri);

        // Add the required headers
        result.addHeader("Accept", "application/json");
        result.addHeader("Content-Type", "application/json; charset=utf-8");
        result.addHeader("User-Agent", String.format("MetadataWebApi-Java/%1$s", System.getProperty("java.version")));

        StringEntity entity = new StringEntity(body);
        result.setEntity(entity);

        return result;
    }

    /**
     * Creates a String containing the JSON request to get the available package groups.
     * @return A String containing the JSON to request the available package groups.
     */
    @SuppressWarnings("unchecked")
    private static String createPackagesRequest() {

        JSONObject credentials = createCredentials();

        JSONObject request = new JSONObject();
        request.put("usernamePassword", credentials);

        return request.toJSONString();
    }

    /**
     * Returns the available package groups.
     * @return The available package groups.
     * @throws Exception
     */
    private static List<PackageGroup> getAvailablePackages() throws Exception {

        HttpClient httpClient = new DefaultHttpClient();
        List<PackageGroup> result = new ArrayList<PackageGroup>();

        try {

            // Create the request JSON
            String body = createPackagesRequest();

            // Create the HTTP POST to request the available packages
            HttpPost request = createHttpPostRequest(
                "https://ws.updates.qas.com/metadata/V1/packages",
                body);

            HttpResponse response = httpClient.execute(request);

            try {

                if (response.getStatusLine().getStatusCode() != 200) {
                    throw new Exception(
                        String.format(
                            "Request for available packages failed with HTTP Status %d.",
                            response.getStatusLine().getStatusCode()));
                }

                // Read the response JSON
                HttpEntity entity = response.getEntity();

                Reader reader = new BufferedReader(
                    new InputStreamReader(entity.getContent()));

                JSONParser parser = new JSONParser();
                JSONObject packagesJson = (JSONObject)parser.parse(reader);

                JSONArray packageGroups = (JSONArray)packagesJson.get("PackageGroups");
                Iterator<?> packageGroupIterator = packageGroups.iterator();

                System.out.println("Available packages:");

                // Iterate through the available package groups
                while (packageGroupIterator.hasNext()) {

                    JSONObject packageGroupJson = (JSONObject)packageGroupIterator.next();

                    String packageGroupCode = (String)packageGroupJson.get("PackageGroupCode");
                    String vintage = (String)packageGroupJson.get("Vintage");

                    PackageGroup packageGroup = new PackageGroup(packageGroupCode, vintage);
                    result.add(packageGroup);

                    System.out.println(
                        String.format(
                            "Package Group Code: %1$s; Vintage: %2$s",
                            packageGroup.getPackageGroupCode(),
                            packageGroup.getVintage()));

                    JSONArray packages = (JSONArray)packageGroupJson.get("Packages");
                    Iterator<?> packageIterator = packages.iterator();

                    // Iterate through the available packages
                    while (packageIterator.hasNext()) {

                        JSONObject packageJson = (JSONObject)packageIterator.next();

                        String packageCode = (String)packageJson.get("PackageCode");

                        Package thePackage = new Package(packageCode);
                        packageGroup.getPackages().add(thePackage);

                        System.out.println(
                            String.format(
                                "Package Code: %1$s",
                                thePackage.getPackageCode()));

                        JSONArray files = (JSONArray)packageJson.get("Files");
                        Iterator<?> fileIterator = files.iterator();

                        // Iterate through the data files
                        while (fileIterator.hasNext()) {

                            JSONObject fileJson = (JSONObject)fileIterator.next();

                            String fileName = (String)fileJson.get("FileName");
                            String md5Hash = (String)fileJson.get("Md5Hash");
                            Long size = (Long)fileJson.get("Size");

                            DataFile dataFile = new DataFile(fileName, md5Hash, size);
                            thePackage.getDataFiles().add(dataFile);

                            System.out.println(
                                String.format(
                                    "Name: %1$s; MD5 Hash: %2$s; Size: %3$d",
                                    dataFile.getFileName(),
                                    dataFile.getMD5Hash(),
                                    dataFile.getSize()));
                        }
                    }
                }
            }
            finally {
                request.releaseConnection();
            }
        }
        finally {
            httpClient.getConnectionManager().shutdown();
        }

        return result;
    }

    /**
     * Gets a URI to use to download the specified data file.
     * @param dataFile The data file to get a download URI for.
     * @return The download URI to use to download the specified data file, if available; otherwise null.
     * @throws Exception
     */
    private static String getDownloadUri(DataFile dataFile) throws Exception {

        HttpClient httpClient = new DefaultHttpClient();

        try {

            // Create the request JSON
            String body = createFileDownloadRequest(dataFile);

            // Create the HTTP POST to request a download URI for the specified file
            HttpPost request = createHttpPostRequest(
                "https://ws.updates.qas.com/metadata/V1/filedownload",
                body);

            HttpResponse response = httpClient.execute(request);

            try {

                if (response.getStatusLine().getStatusCode() != 200) {
                    throw new Exception(
                        String.format(
                            "Request for file download URI failed with HTTP Status %d.",
                            response.getStatusLine().getStatusCode()));
                }

                // Read the response JSON
                HttpEntity entity = response.getEntity();

                Reader reader = new BufferedReader(
                    new InputStreamReader(entity.getContent()));

                JSONParser parser = new JSONParser();
                JSONObject json = (JSONObject)parser.parse(reader);

                // Return the URI, if any
                return (String)json.get("DownloadUri");
            }
            finally {
                request.releaseConnection();
            }
        }
        finally {
            httpClient.getConnectionManager().shutdown();
        }
    }

    public static void main(String[] args) throws Exception {
    	
    	if (userName == null || userName.isEmpty() || password == null || password.isEmpty()) {

    		System.out.println("No QAS Electronic Updates service credentials are configured.");
    		System.exit(-1);
    		
    	}

        List<PackageGroup> availablePackages = getAvailablePackages();

        for (int i = 0; i < availablePackages.size(); i++) {

            PackageGroup packageGroup = availablePackages.get(i);

            for (int j = 0; j < packageGroup.getPackages().size(); j++) {

                Package thePackage = packageGroup.getPackages().get(j);

                for (int k = 0; k < thePackage.getDataFiles().size(); k++) {

                    DataFile dataFile = thePackage.getDataFiles().get(k);

                    String downloadUri = getDownloadUri(dataFile);

                    if (downloadUri != null) {

                        File rootDataDirectory = new File(rootDownloadPath);
                        File packageGroupDirectory = new File(rootDataDirectory, packageGroup.getPackageGroupCode());
                        File vintageDirectory = new File(packageGroupDirectory, packageGroup.getVintage());

                        if (!vintageDirectory.exists()) {
                            vintageDirectory.mkdirs();
                        }

                        File fileName = new File(vintageDirectory, dataFile.getFileName());

                        URL url = new URL(downloadUri);
                        
                        System.out.println(String.format("Downloading %1$s to '%2$s'...", dataFile.getFileName(), fileName.getPath()));

                        // Download the file
                        ReadableByteChannel channel = Channels.newChannel(url.openStream());
                        FileOutputStream stream = new FileOutputStream(fileName.getPath());

                        try {
                            stream.getChannel().transferFrom(channel, 0, Long.MAX_VALUE);
                        }
                        finally {
                            stream.close();
                        }

                        // Validate the downloaded file is not corrupt
                        String computedHash = calculateMD5Hash(fileName.getPath());

                        if (!computedHash.equals(dataFile.getMD5Hash())) {
                            System.out.println(String.format("'%1$s' is corrupted.", fileName.getPath()));
                            fileName.delete();
                        }
                    }
                }
            }
        }
        
        System.out.println("All file(s) downloaded.");
    }
}
