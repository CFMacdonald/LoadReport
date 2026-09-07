namespace LoadReport.Models;

public class Layer
{
    public string? LayerType {  get; private set; }
    public string? ProductName {  get; private set; }
    public int ActualOutage { get; private set; }
    int TargetOutage { get; set; }
    double DrumQuanity { get; set; }
    double DrumNetWeight { get; set; }
    string? LoadMethod { get; set; }

    public Layer(string layerType, string productName, int actualOutage, int targetOutage, double drumQuanity, double drumNetWeight, string loadMethod)
    {
        LayerType = layerType;
        ProductName = productName;
        ActualOutage = actualOutage;
        TargetOutage = targetOutage;
        DrumQuanity = drumQuanity;
        DrumNetWeight = drumNetWeight;
        LoadMethod = loadMethod;
    }

}
