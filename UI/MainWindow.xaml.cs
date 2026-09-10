using LoadReport.Main;
using LoadReport.Models;
using System.Windows;
using System.Windows.Media;

namespace LoadReport;

public partial class MainWindow : Window
{
    string? jobDescription;
    string? jobNumber;
    string? clientName;
    string? clientAddress;
    string? vesselType;
    int numberOfBeds;
    int initalOutage;
    int internalDiamater;
    VesselManager vesselManager;

    public MainWindow()
    {
        InitializeComponent();
        vesselManager = new VesselManager();
    }

    private void LayerButton_Click(object sender, RoutedEventArgs e)
    {      
        NewLayerWindow newLayerWindow = new NewLayerWindow();
        newLayerWindow.Show();
        newLayerWindow.GetVesselManager(vesselManager);
        newLayerWindow.GetMainWindow(this);
        ButtonRemoveLayer.BorderBrush = Brushes.Black;

    }

    private void Generate_Button_Click(object sender, RoutedEventArgs e)
    {
        bool checkJobDes = ValidateJobDescription();
        bool checkJobNumber = ValidateJobNumber();
        bool checkClientName = ValidateClientName();
        bool checkClientAddress = ValidateClientAddress();
        bool checkVesselType = ValidateVesselType();
        bool checkBedNumber = ValidateNumberOfBeds();
        bool checkOutage = ValidateInitalOutage();
        bool checkInternalDiameter = ValidateInternalDiameter();

        if (checkJobDes && checkJobNumber && checkClientName && checkClientAddress && checkVesselType && checkBedNumber && checkOutage && checkInternalDiameter && !vesselManager.VesselCreated)
        {
            Vessel vessel = new Vessel(jobDescription!, jobNumber!, clientName!, clientAddress!, vesselType!, numberOfBeds, initalOutage, internalDiamater);
            vesselManager.InitialiseVessel(vessel);   
            vesselManager.GenerateReport(vesselManager);
        }
    }

    bool ValidateJobDescription()
    {
        jobDescription = JobDescriptionTextBox.Text;
        if (jobDescription == "")
        {
            JobDescriptionTextBlock.Foreground = Brushes.Red;
            return false;
        }
        else
        {
            JobDescriptionTextBlock.Foreground = Brushes.Black;
            return true;
        }
    }
    bool ValidateJobNumber()
    {
        jobNumber = JobNumberTextBox.Text;
        if (jobNumber == "")
        {
            JobNumberTextBlock.Foreground = Brushes.Red;
            return false;
        }
        else
        {
            JobNumberTextBlock.Foreground = Brushes.Black;
            return true;
        }
    }
    bool ValidateClientName()
    {
        clientName = ClientNameTextBox.Text;
        if (clientName == "")
        {
            ClientNameTextBlock.Foreground = Brushes.Red;
            return false;
        }
        else
        {
            ClientNameTextBlock.Foreground = Brushes.Black;
            return true;
        }
    }
    bool ValidateClientAddress()
    {
        clientAddress = ClientAddressTextBox.Text;
        if (clientAddress == "")
        {
            ClientAddressTextBlock.Foreground = Brushes.Red;
            return false;
        }
        else
        {
            ClientAddressTextBlock.Foreground = Brushes.Black;
            return true;
        }
    }
    bool ValidateVesselType()
    {
        var selectedItem = VesselTypeComboBox.SelectionBoxItem;
        if (selectedItem == null)
        {
            VesselTypeTextBlock.Foreground = Brushes.Red;
            return false;
        }
        else
        {
            VesselTypeTextBlock.Foreground = Brushes.Black;
            vesselType = selectedItem.ToString();
            return true;
        }
    }
    bool ValidateNumberOfBeds()
    {
        var selectedItem = BedNumberComboBox.SelectionBoxItem;

        bool pass = Int32.TryParse(selectedItem.ToString(), out numberOfBeds);
        if (pass)
        {
            BedNumberTextBlock.Foreground = Brushes.Black;
            return true;
        }
        else
        {
            BedNumberTextBlock.Foreground = Brushes.Red;
            return false;
        }

    }
    bool ValidateInitalOutage()
    {
        bool pass = Int32.TryParse(InitialOutageTextBox.Text, out initalOutage);
        if (pass)
        {
            InitialOutageTextBlock.Foreground = Brushes.Black;
            return true;
        }
        else
        {
            InitialOutageTextBlock.Foreground = Brushes.Red;
            return false;
        }

    }
    bool ValidateInternalDiameter()
    {
        bool pass = Int32.TryParse(InternalDiameterTextBox.Text, out internalDiamater);
        if (pass)
        {
            InternalDiameterTextBlock.Foreground = Brushes.Black;
            return true;
        }
        else
        {
            InternalDiameterTextBlock.Foreground = Brushes.Red;
            return false;
        }
    }
   
    public void RefreshDisplay()
    {
        int lastEntry = vesselManager.Layers.Count - 1;

        Layer layer = vesselManager.Layers[lastEntry];

        LayerListBox.Items.Add($"Layer: {lastEntry + 1} {layer.LayerType} {layer.ProductName} Outage: {layer.ActualOutage}mm ");
    }

    private void RemoveLayer_Click(object sender, RoutedEventArgs e)
    {    
        int lastEntry = vesselManager.Layers.Count - 1;
        if (lastEntry >= 0)
        {
            LayerListBox.Items.RemoveAt(lastEntry);
            vesselManager.RemoveLayer();
            lastEntry--;
        }
        else
        {
            ButtonRemoveLayer.BorderBrush = Brushes.Red;
        }
    }
}



