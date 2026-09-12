using LoadReport.Main;
using LoadReport.Models;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Media;

namespace LoadReport;

public partial class MainWindow : Window
{
    string? jobDescription;
    string? jobNumber;
    string? clientName;
    string? clientAddress;
    string? vesselID;
    Template templateType;
    int bedNumber;
    int initalOutage;
    int internalDiamater;
    VesselManager _vesselManager;
   

    public MainWindow()
    {
        InitializeComponent();
        _vesselManager = new VesselManager();
        VesselTypeComboBox.ItemsSource = Enum.GetValues<Template>();
    }

    private void LayerButton_Click(object sender, RoutedEventArgs e)
    {
        NewLayerWindow newLayerWindow = new NewLayerWindow();
        newLayerWindow.Show();
        newLayerWindow.GetVesselManager(_vesselManager);
        newLayerWindow.GetMainWindow(this);
        ButtonRemoveLayer.BorderBrush = Brushes.Black;
    }

    private void Generate_Button_Click(object sender, RoutedEventArgs e)
    {
        AssignTextBox();

        bool checkJobDes = ValidateJobDescription();
        bool checkJobNumber = ValidateJobNumber();
        bool checkClientName = ValidateClientName();
        bool checkClientAddress = ValidateClientAddress();
        bool checkVesselID = ValidateVesselID();
        bool checkTemplateType = ValidateTemplateType();
        bool checkBedNumber = ValidateNumberOfBeds();
        bool checkOutage = ValidateInitalOutage();
        bool checkInternalDiameter = ValidateInternalDiameter();
       
      
        if (checkJobDes && 
            checkJobNumber && 
            checkClientName && 
            checkClientAddress && 
            checkVesselID &&
            checkTemplateType && 
            checkBedNumber && 
            checkOutage && 
            checkInternalDiameter &&             
            !_vesselManager.VesselCreated)
        {
            Vessel vessel = new Vessel(jobDescription!,
                jobNumber!,
                clientName!,
                clientAddress!,
                vesselID!,
                templateType!,
                bedNumber,
                initalOutage,
                internalDiamater);

            string fileName = SaveFileDialog();
            _vesselManager.InitialiseVessel(vessel);
            _vesselManager.GenerateReport(_vesselManager, fileName);
            Close();
        }
    }

    private static string SaveFileDialog()
    {
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        saveFileDialog1.Filter = "PDF|*.pdf";
        saveFileDialog1.Title = "Save Report";
        saveFileDialog1.ShowDialog();
        string fileName = saveFileDialog1.FileName;
        return fileName;
    }

    bool ValidateJobDescription()
    {    
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

    bool ValidateVesselID()
    {
        if (vesselID == "")
        {
            VesselIDTextBlock.Foreground = Brushes.Red;
            return false;
        }
        else
        {
            VesselIDTextBlock.Foreground = Brushes.Black;
            return true;
        }
    }

    bool ValidateTemplateType()
    {
        var selectedItem = VesselTypeComboBox.SelectedItem;

        if (selectedItem == null)
        {
            VesselTypeTextBlock.Foreground = Brushes.Red;
            return false;
        }
        else
        {
            VesselTypeTextBlock.Foreground = Brushes.Black;
            templateType = (Template)selectedItem;
            return true;
        }
    }

    bool ValidateNumberOfBeds()
    {
        var selectedItem = BedNumberComboBox.SelectionBoxItem;

        bool pass = Int32.TryParse(selectedItem.ToString(), out bedNumber);
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
        int lastEntry = _vesselManager.Layers.Count - 1;

        Layer layer = _vesselManager.Layers[lastEntry];

        LayerListBox.Items.Add($"Layer: {lastEntry + 1} {layer.LayerType} {layer.ProductName} Outage: {layer.ActualOutage}mm ");
    }

    void RemoveLayer_Click(object sender, RoutedEventArgs e)
    {
        int lastEntry = _vesselManager.Layers.Count - 1;
        if (lastEntry >= 0)
        {
            LayerListBox.Items.RemoveAt(lastEntry);
            _vesselManager.RemoveLayer();
            lastEntry--;
        }
        else
        {
            ButtonRemoveLayer.BorderBrush = Brushes.Red;
        }
    }

    void AssignTextBox() 
    {
        jobDescription = JobDescriptionTextBox.Text;
        jobNumber = JobNumberTextBox.Text;
        clientName = ClientNameTextBox.Text;
        clientAddress = ClientAddressTextBox.Text;
        vesselID = VesselIDTextBox.Text;
    }

    private void AddCompanyLogoClick(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();

        openFileDialog.ShowDialog();

        _vesselManager.CompanyLogo = openFileDialog.FileName;
    }
}

