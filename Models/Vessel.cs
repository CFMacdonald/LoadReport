namespace LoadReport.Models;

public class Vessel
{
    public string? JobDescription {  get; private set; }
    public string? JobNumber {  get; private set; }
    public string? ClientName { get; private set; }
    public string? ClientAddress { get; private set; }
    public VesselType Type {  get; private set; }
    public int BedNumber {  get; private set; }
    public int InitialOutage { get; private set; }
    public int InternalDiameter {  get; private set; }
   
   
    public Vessel(string jobDescription, string jobNumber, string clientName, string clientAddress, VesselType type, int bedNumber, int initialOutage, int internalDiameter)
    {
        JobDescription = jobDescription;
        JobNumber = jobNumber;
        ClientName = clientName;
        ClientAddress = clientAddress;
        Type = type;
        BedNumber = bedNumber;
        InitialOutage = initialOutage;
        InternalDiameter = internalDiameter;
    }
}
public enum VesselType { SingleBed, MultiBed, SecondaryRefomrer }

