namespace LoadReport.Models;

internal class Layer
{
    string? LayerType {  get; set; }
    string? ProductName {  get; set; }
    int ActualOutage { get; set; }
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
