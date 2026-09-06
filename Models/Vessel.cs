namespace LoadReport.Models;

internal class Vessel
{
    string? JobDescription {  get; set; }
    string? JobNumber {  get; set; }
    string? ClientName { get; set; }
    string? ClientAddress { get; set; }
    string? VesselType {  get; set; }
    int BedNumber {  get; set; }
    int InitialOutage { get; set; }
    int InternalDiameter {  get; set; }
   
   
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


