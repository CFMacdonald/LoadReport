namespace LoadReport.Models;

public class Layer
{
    public string? LayerType {  get; private set; }
    public string? ProductName {  get; private set; }
    public int ActualOutage { get; private set; }
    public int TargetOutage { get; private set; }
    public double DrumQuanity { get; private set; }
    public double DrumNetWeight { get; private set; }
    public string? LoadMethod { get; private set; }
    public int LayerDensety { get; private set; }

    public float LayerHeight {  get; private set; }

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
