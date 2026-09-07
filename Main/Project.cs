using LoadReport.Models;
using System.Diagnostics;

namespace LoadReport.Main;

public class Project
{
    Vessel Vessel { get; set; }
    public List<Layer> Layers { get; private set; }
    public bool VesselCreated {  get; private set; }

    public Project()
    {
        Layers = new List<Layer>();
    }

    public void InitialiseVessel(Vessel vessel)
    {
        Vessel = vessel;
        VesselCreated = true;
    }

    public void AddLayer(Layer layer)
    {
        Layers.Add(layer);
    }

    public void RemoveLayer()
    {
        Layers.RemoveAt(Layers.Count - 1);
    }
    public void DebugLayers()
    {
        for (int i = 0; i < Layers.Count; i++)
        {
            Debug.WriteLine(Layers[i].ToString());
        }
    }
}


