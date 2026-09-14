namespace LoadReport.Models;

public class Layer
{
    public LayerType LayerType {  get; private set; }
    public string? ProductName {  get; private set; }
    public int ActualOutage { get; private set; }
    public int TargetOutage { get; private set; }
    public double DrumQuanity { get; private set; }
    public double DrumNetWeight { get; private set; }
    public string? LoadMethod { get; private set; }
    public int LayerDensety { get; private set; }

    public double LayerHeight {  get; set; }

    public Layer(LayerType layerType, string productName, int actualOutage, int targetOutage, double drumQuanity, double drumNetWeight, string loadMethod)
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
public enum LayerType {Catalyst, Media, Support, Tray}
