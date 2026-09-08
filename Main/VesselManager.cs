using LoadReport.Models;
using LoadReport.ReportService;

namespace LoadReport.Main;

public class VesselManager
{
    public Vessel Vessel { get; private set; }
    public Report Report { get; private set; }
    public List<Layer> Layers { get; private set; }
    public bool VesselCreated {  get; private set; }

    public VesselManager()
    {
        Layers = new List<Layer>();
       
    }

    public void InitialiseVessel(Vessel vessel)
    {
        Vessel = vessel;
        VesselCreated = true;
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
     
}


