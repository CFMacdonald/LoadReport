using LoadReport.Models;
using LoadReport.ReportService;

namespace LoadReport.Main;

public class VesselManager
{
    public Vessel Vessel { get; private set; }
    public Report Report { get; private set; }
    public List<Layer> Layers { get; private set; }
    public bool VesselCreated {  get; private set; }
  
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
        int layerIndex = Layers.IndexOf(layer);

        double previousOutage;

        if (layerIndex == 0)
        {
            previousOutage = Vessel.InitialOutage;
        }
        else
        {
            previousOutage = Layers[layerIndex - 1].ActualOutage;
        }

        double currentHeight = previousOutage - layer.ActualOutage;

        return currentHeight / 1000.0;
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
       

        return totalMass / totalVolume;
    }
      
}


