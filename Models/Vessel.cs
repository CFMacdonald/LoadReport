namespace LoadReport.Models;

public class Vessel
{
    public string? JobDescription {  get; private set; }
    public string? JobNumber {  get; private set; }
    public string? ClientName { get; private set; }
    public string? ClientAddress { get; private set; }
    public string? VesselType {  get; private set; }
    public int BedNumber {  get; private set; }
    public int InitialOutage { get; private set; }
    public int InternalDiameter {  get; private set; }
   
   
    public Vessel(string jobDescription, string jobNumber, string clientName, string clientAddress, string vesselType, int bedNumber, int initialOutage, int internalDiameter)
    {
        JobDescription = jobDescription;
        JobNumber = jobNumber;
        ClientName = clientName;
        ClientAddress = clientAddress;
        VesselType = vesselType;
        BedNumber = bedNumber;
        InitialOutage = initialOutage;
        InternalDiameter = internalDiameter;
    }
}


