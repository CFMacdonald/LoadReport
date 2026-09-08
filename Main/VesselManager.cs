using LoadReport.Models;
using LoadReport.ReportService;

namespace LoadReport.Main;

public class VesselManager
{
    public Vessel Vessel { get; private set; }
    public Report Report { get; private set; }
    public List<Layer> Layers { get; private set; }
    public bool VesselCreated {  get; private set; }

    float _currentOutage; 

    public VesselManager()
    {
        Layers = new List<Layer>();
       
    }

    public void InitialiseVessel(Vessel vessel)
    {
        Vessel = vessel;
        VesselCreated = true;
       _currentOutage = Vessel.InitialOutage;
    }
    public void GenerateReport(VesselManager project)
    {
        Report = new Report(project);
    }

    public void AddLayer(Layer layer)
    {
        Layers.Add(layer);
    }
    public void RemoveLayer()
    {
        Layers.RemoveAt(Layers.Count - 1);
    }

   double CalculateLayerHeight(Layer layer)
   {       
        double bedHeight = _currentOutage - layer.ActualOutage;
        _currentOutage = layer.ActualOutage;
        return bedHeight / 1000;
   }
    double CalculateLayerVolume(Layer layer)
    {
        double radius = (Vessel.InternalDiameter / 1000.0) / 2;
        double area = Math.PI * (radius * radius);
        double volume = area * CalculateLayerHeight(layer);
        return volume;
    }
    double CalculateLayerMass(Layer layer)
    {
        double mass = layer.DrumNetWeight * layer.DrumQuanity;
        return mass;
    }
    public double CalculateLayerDensity(Layer layer)
    {
        double density = CalculateLayerMass(layer) / CalculateLayerVolume(layer);
        return density;
    }
}


