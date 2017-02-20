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

import org.apache.http.*;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;

/**
 * Java sample code for the Experian Data Quality Electronic Updates Metadata Web API.
 * @author Experian 
 */
public class Program {

    /**
     * The user name to use to authenticate with the web service.
     */
    private static String token = "x-api-key " + System.getenv("EDQ_ElectronicUpdates_Token");

    /*
     * The endpoint for the REST service.
     */
    private static final String endpoint = "https://ws.updates.qas.com/metadata/v2/";

    /**
     * The root folder to download data to.
     */
    private static final String rootDownloadPath = "EDQData";

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
     * Creates a String containing the JSON request to download the specified data file.
     * @param dataFile The data file to get the download request for.
     * @return A String containing the JSON to download the specified data file.
     */
    @SuppressWarnings("unchecked")
    private static String createFileDownloadRequest(DataFile dataFile) {

        JSONObject result = new JSONObject();
        
        result.put("FileMd5Hash", dataFile.getMD5Hash());

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
     * Returns the available package groups.
     * @return The available package groups.
     * @throws Exception
     */
    private static List<PackageGroup> getAvailablePackages() throws Exception {

        HttpClient httpClient = new DefaultHttpClient();
        List<PackageGroup> result = new ArrayList<PackageGroup>();

        try {
            
            // Create the HTTP GET to request the available packages
            HttpGet request = new HttpGet(endpoint + "packages");
            
            // Add the required headers
            request.addHeader("Accept", "application/json");
            request.addHeader("Content-Type", "application/json; charset=utf-8");
            request.addHeader("User-Agent", String.format("MetadataWebApi-Java/%1$s", System.getProperty("java.version")));
            request.addHeader("Authorization", token);

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
                JSONArray packageGroups = (JSONArray)parser.parse(reader);
                
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

                            String fileName = (String)fileJson.get("Filename");
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
                    endpoint + "filelink",
                body);

            request.addHeader("Authorization", token);

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

    	if (token == null || token.isEmpty()) {

    		System.out.println("No Experian Data Quality Electronic Updates service token is configured.");
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
