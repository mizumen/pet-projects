package com.patch.database;

/**
 * Created by maks on 5/9/2016.
 */
public class Patch {
    private int id;
    private String name;
    private String patchType;
    private String description;
    private String productVersion;
    private String defectIds;
    private String zendesk;
    private String targetProductName;
    private String xmlLink;
    private String msiLink;

    public Patch(){

    }

    public Patch(int id, String name, String patchType, String description, String productVersion,
                 String defectIds, String zendesk, String targetProductName, String xmlLink, String msiLink) {
        this.id = id;
        this.name = name;
        this.patchType = patchType;
        this.description = description;
        this.productVersion = productVersion;
        this.defectIds = defectIds;
        this.zendesk = zendesk;
        this.targetProductName = targetProductName;
        this.xmlLink = xmlLink;
        this.msiLink = msiLink;
    }

    public void setId(int id) {
        this.id = id;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setPatchType(String patchType) {
        this.patchType = patchType;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public void setProductVersion(String productVersion) {
        this.productVersion = productVersion;
    }

    public void setDefectIds(String defectIds) {
        this.defectIds = defectIds;
    }

    public void setZendesk(String zendesk) {
        this.zendesk = zendesk;
    }

    public void setTargetProductName(String targetProductName) {
        this.targetProductName = targetProductName;
    }

    public void setXmlLink(String xmlLink) {
        this.xmlLink = xmlLink;
    }

    public void setMsiLink(String msiLink) {
        this.msiLink = msiLink;
    }

    public int getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getPatchType() {
        return patchType;
    }

    public String getDescription() {
        return description;
    }

    public String getProductVersion() {
        return productVersion;
    }

    public String getDefectIds() {
        return defectIds;
    }

    public String getZendesk() {
        return zendesk;
    }

    public String getTargetProductName() {
        return targetProductName;
    }

    public String getXmlLink() {
        return xmlLink;
    }

    public String getMsiLink() {
        return msiLink;
    }

    @Override
    public String toString() {
        return getClass().getSimpleName() + " : id: " + id +
                ", name: " + name + ", patchtype:" + patchType + ", description: " + description + ", productVersion: " + productVersion +
                ", defectIds: " + defectIds + ", zendesk: " + zendesk + ", targetProductName: " + targetProductName +
                ", xmlLink: " + xmlLink + ", msiLink: " + msiLink ;
    }
}
