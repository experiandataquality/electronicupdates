/**
 * Copyright (c) Experian. All rights reserved.
 */

package com.experian.qas.updates.metadata.webapi.v2;

import java.util.ArrayList;
import java.util.List;

/**
 * A class representing a single vintage of data.
 */
public class PackageGroup {

    /**
     * The individual packages of this group.
     */
    private final List<Package> packages;
    
    /**
     * The package group code.
     */
    private final String packageGroupCode;
    
    /**
     * The package vintage.
     */
    private final String vintage;
    
    /**
     * Initializes a new instance of the PackageGroup class.
     */
    public PackageGroup(String packageGroupCode, String vintage) {
        
        this.packageGroupCode = packageGroupCode;
        this.vintage = vintage;
        this.packages = new ArrayList<Package>();
    }
    
    /**
     * Gets the packages associated with the package group.
     * @return The packages associated with this group.
     */
    public List<Package> getPackages() {
        return this.packages;
    }
    
    /**
     * Gets the package group code.
     * @return The package group code.
     */
    public String getPackageGroupCode() {
        return this.packageGroupCode;
    }
    
    /**
     * Gets the package vintage.
     * @return The package vintage.
     */
    public String getVintage() {
        return this.vintage;
    }
}
