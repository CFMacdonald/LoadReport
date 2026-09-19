using LoadReport.Main;
using LoadReport.Models;
using System.Windows;
using System.Windows.Media;

namespace LoadReport;



public partial class NewLayerWindow : Window
{
    LayerType layerType;
    string productName;
    int actualOutage;
    int targetOutage;
    double drumQuanity;
    double drumNetWeight;
    string? loadMethod;
    public Layer NewLayer {  get; set; }
    public bool layerCreated { get; private set; } = false;
    VesselManager _vesselManager;
    MainWindow _mainWindow;
    Brush defaultBrush;

    public NewLayerWindow()
    {
        InitializeComponent();
        LayerTypeComboBox.ItemsSource = Enum.GetValues<LayerType>();
        defaultBrush = (Brush)new BrushConverter().ConvertFromString("#E6E6E6");
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
            _vesselManager.AddLayer(NewLayer);
            _mainWindow.RefreshLayerDisplay();        
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
            LayerTypeTextBlock.Foreground = defaultBrush;
            layerType = (LayerType)selectedItem;
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
            ProductNameTextBlock.Foreground = defaultBrush;
                return true;
            }
        }

        bool ValidateActualOutage()
        {             
            bool pass = Int32.TryParse(ActualOutageTextBox.Text, out actualOutage);
            if (pass)
            {
            ActualOutageTextBlock.Foreground = defaultBrush;
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
                TargetOutageTextBlock.Foreground = defaultBrush;
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
                DrumQuantityTextBlock.Foreground = defaultBrush;
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
                DrumNetWeightTextBlock.Foreground = defaultBrush;
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
            var selectedItem = LoadMethodComboBox.SelectionBoxItem;
            if (string.IsNullOrEmpty(selectedItem.ToString()))
            {
                LoadMethodTextBlock.Foreground = Brushes.Red;
                return false;
            }
            else
            {
                LoadMethodTextBlock.Foreground = defaultBrush;
                loadMethod = (string)selectedItem;
            return true;
            }
        }

    public void GetVesselManager(VesselManager vesselManager)
    {
        _vesselManager = vesselManager;
    }

    public void GetMainWindow(MainWindow window)
    {
        _mainWindow = window;
    }

    void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this).Close();
    }

    void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ChangedButton == System.Windows.Input.MouseButton.Left) 
        {
            this.DragMove();
        }

    }
}


