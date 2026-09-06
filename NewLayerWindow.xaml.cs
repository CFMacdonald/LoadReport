using LoadReport.Models;
using System.Windows;
using System.Windows.Media;

namespace LoadReport;


public partial class NewLayerWindow : Window
{

    string? layerType;
    string productName;
    int actualOutage;
    int targetOutage;
    double drumQuanity;
    double drumNetWeight;
    string? loadMethod;
    public Layer NewLayer {  get; set; }
    public bool layerCreated { get; private set; } = false; 

    public NewLayerWindow()
    {
        InitializeComponent();
    }

    private void AddLayerConfirm_Click(object sender, RoutedEventArgs e)
    {   
        bool checkLayer = ValidateLayerType();
        bool checkName = ValidateProductName();
        bool checkTarget = ValidateTargetOutage();
        bool checkActual = ValidateActualOutage();
        bool checkWeight = ValidateDrumWeight();
        bool checkQuanity = ValidateDrumQuanity();
        bool checkMethod = ValidateLoadingMethod();

        if (checkLayer && checkName && checkTarget && checkActual && checkWeight && checkQuanity && checkMethod)
        {  
            NewLayer = new Layer(layerType!, productName, actualOutage, targetOutage, drumQuanity, drumNetWeight, loadMethod!);
            layerCreated = true;
            Close();
        }
    }
        bool ValidateLayerType()
        {
        var selectedItem = LayerTypeComboBox.SelectedItem;
        if (selectedItem == null)
        {
            LayerTypeTextBlock.Foreground = Brushes.Red;
            return false;
        }
        else
        {
            LayerTypeTextBlock.Foreground = Brushes.Black;
            layerType = selectedItem.ToString();
            return true;
        }
        }
        bool ValidateProductName()
        {
            productName = ProductNameTextBox.Text;
            if (productName == "")
            {
                ProductNameTextBlock.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                ProductNameTextBlock.Foreground = Brushes.Black;
                return true;
            }
        }
        bool ValidateActualOutage()
        {             
            bool pass = Int32.TryParse(ActualOutageTextBox.Text, out actualOutage);
            if (pass)
            {
                ActualOutageTextBlock.Foreground = Brushes.Black;
                return true;
            }
            else
            {
                ActualOutageTextBlock.Foreground = Brushes.Red;
                return false;
            }
        }
        bool ValidateTargetOutage()
        {             
            bool pass = Int32.TryParse(TargetOutageTextBox.Text, out targetOutage);
            if (pass)
            {
                TargetOutageTextBlock.Foreground = Brushes.Black;
                return true;
            }
            else
            {
                TargetOutageTextBlock.Foreground = Brushes.Red;
                return false;
            }
        }
        bool ValidateDrumQuanity()
        {            
            bool pass = double.TryParse(DrumQuantityTextBox.Text, out drumQuanity);
            if (pass)
            {
                DrumQuantityTextBlock.Foreground = Brushes.Black;
                return true;
            }
            else
            {
                DrumQuantityTextBlock.Foreground = Brushes.Red;
                return false;
            }
        }
        bool ValidateDrumWeight()
        {         
            bool pass = double.TryParse(DrumNetWeightTextBox.Text, out drumNetWeight);
            if (pass)
            {
                DrumNetWeightTextBlock.Foreground = Brushes.Black;
                return true;
            }
            else
            {
                DrumNetWeightTextBlock.Foreground = Brushes.Red;
                return false;
            }
        }
        bool ValidateLoadingMethod()
        {
            var selectedItem = LoadMethodComboBox.SelectedItem;
            if (selectedItem == null)
            {
                LoadMethodTextBlock.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                LoadMethodTextBlock.Foreground = Brushes.Black;
                loadMethod = selectedItem.ToString();
                return true;
            }

        }
   
    }


