using LoadReport.Models;
using LoadReport.ReportService;

namespace LoadReport.Main;

public class VesselManager
{
    public Vessel Vessel { get; private set; }
    public Report Report { get; private set; }
    public List<Layer> Layers { get; private set; }
    public bool VesselCreated {  get; private set; }
    public float CurrentOutage { get; private set; }
    public string CompanyLogo { get; set; }
    public float FinalOutage { get; private set; }

    public VesselManager()
    {
        Layers = new List<Layer>();
       
    }

    public void InitialiseVessel(Vessel vessel)
    {
       Vessel = vessel;
       VesselCreated = true;
       CurrentOutage = Vessel.InitialOutage;
    }
    public void GenerateReport(VesselManager vesselManager, string fileName)
    {
        Report = new Report(vesselManager, fileName);
    }

    public void AddLayer(Layer layer)
    {
        Layers.Add(layer);
        
    }
    public void RemoveLayer()
    {
        Layers.RemoveAt(Layers.Count - 1);
    }

    public double CalculateLayerHeight(Layer layer)
   {       
        double layerHeight = CurrentOutage - layer.ActualOutage;
        CurrentOutage = layer.ActualOutage;
        layer.LayerHeight = layerHeight;
        return layerHeight / 1000.0;
   }
    double CalculateLayerVolume(Layer layer)
    {
        double radius = (Vessel.InternalDiameter / 1000.0) / 2;
        double area = Math.PI * (radius * radius);
        double volume = area * CalculateLayerHeight(layer);
        return volume;
    }

    public double CalculateLayerMass(Layer layer) => layer.DrumNetWeight * layer.DrumQuanity;
   
    public double CalculateLayerDensity(Layer layer) => CalculateLayerMass(layer) / CalculateLayerVolume(layer);

    public int CalculateOutageDiffereance(Layer layer) => layer.TargetOutage - layer.ActualOutage;

    public int GetTotalLayerAmount() => Layers.Count;

    public float GetFinalOutage() 
    {
        if(Layers.Count <= 0) 
        {  
            return Vessel.InitialOutage;
        }
        else
        {
            int finalIndex = Layers.Count - 1;

            float outage = Layers[finalIndex].ActualOutage;
            return outage;

        }


       
    }

    public double CalculateBedDensity()
    {
        double totalMass = 0;
        double totalVolume = 0;

        foreach (Layer layer in Layers)
        {
            if (layer.LayerType == LayerType.Catalyst || layer.LayerType == LayerType.Media)
            {
                totalMass += CalculateLayerMass(layer);
                totalVolume += CalculateLayerVolume(layer);
            }
        }
        CurrentOutage = Vessel.InitialOutage;

        return totalMass / totalVolume;
    }
      
}


