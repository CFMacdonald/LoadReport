using System.ComponentModel;

namespace LoadReport.Models;

public class Vessel
{
    public string? JobDescription {  get; private set; }
    public string? JobNumber {  get; private set; }
    public string? ClientName { get; private set; }
    public string? ClientAddress { get; private set; }
    public string? VesselID { get; private set; }
    public Template TemplateType {  get; private set; }
    public int BedNumber {  get; private set; }
    public int InitialOutage { get; private set; }
    public int InternalDiameter {  get; private set; }

    public Vessel(
        string jobDescription,
        string jobNumber,
        string clientName,
        string clientAddress,
        string vesselID,
        Template type,
        int bedNumber,
        int initialOutage,
        int internalDiameter)
    {
        JobDescription = jobDescription;
        JobNumber = jobNumber;
        ClientName = clientName;
        ClientAddress = clientAddress;
        TemplateType = type;
        BedNumber = bedNumber;
        InitialOutage = initialOutage;
        InternalDiameter = internalDiameter;
        VesselID = vesselID;
    }
    
}

public enum Template {SingleBed, SingleBedWithSupportGrid, TopBed, MiddleBed, BottomBed, SecondaryReformer }

