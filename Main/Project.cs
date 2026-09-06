using LoadReport.Models;

namespace LoadReport.Main;

internal class Project
{
    Vessel Vessel { get; set; }
    List<Layer> Layers { get; set; }
    public bool VesselCreated {  get; private set; }

    public void InitialiseVessel(Vessel vessel)
    {
        Vessel = vessel;
        VesselCreated = true;
    }

    public void AddLayer(Layer layer)
    {
        Layers.Add(layer);
    }
}


