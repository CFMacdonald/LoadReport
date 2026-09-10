using LoadReport.Main;
using LoadReport.Models;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LoadReport.ReportService;

public class Report
{
    static Report()
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
    }

    VesselManager _vesselManager;
    Layer[] TEMPLAYERS;
    float renderHeight = 700f;
    float logicalScale;
 
    public Report(VesselManager vesselManager)
    {
        TEMPLAYERS = new Layer[4];
        TEMPLAYERS[0] = new Layer("Support", "Ceramic Balls", 900, 790, 10, 25, "Sock Load");
        TEMPLAYERS[1] = new Layer("Catalyst", "Ceramics", 800, 540, 20, 100, "Dense Load");
        TEMPLAYERS[2] = new Layer("Catalyst", "Catalyst B", 200, 240, 25, 100, "Dense Load");
        TEMPLAYERS[3] = new Layer("Top Support", "Ceramic Balls", 100, 90, 8, 25, "Sock Load");


        _vesselManager = vesselManager;
        GenerateReport();       
    
    }

    void GenerateReport()
    {
        logicalScale = _vesselManager.Vessel.InitialOutage;
        float scale = renderHeight / logicalScale;

    Document.Create(container =>
        {
            

            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(5, Unit.Millimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(6));

            

                page.Header()
                    
                    .Text("Loading Report")
                    .SemiBold().FontSize(36).FontColor(Colors.Black);

                page.Content()

                // First Row Table

                .Row(row =>
                {
                    row.ConstantItem(140)
                    

                    .Column(column =>
                    {
                        // Top Section of Table Left Side

                        column.Item()
                       
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(70);
                                columns.ConstantColumn(70);

                            });

                            DisplayProjectInformation(table);
                            DisplayerVesselInformation(table);
                            DisplayLayers(table);
                            DisplayCompanyImage(table);
                        });

                        // Right Side of Table, Vessel Diagaram Location

                        row.RelativeItem()

                 
                            .Border(.5F)
                            .AlignMiddle()
                            .AlignCenter()  
                            .Width(220)
                            
                            .Height(renderHeight)

                            .Layers(layer =>
                            {
                                // vessel render
                            layer.PrimaryLayer()                            
                                   
                            .Svg("Models/VesselTemplates/SingleBedVessel.svg");


                                
                                // outage render
                                foreach (var outage in TEMPLAYERS.Reverse())
                                {
                                    layer.Layer()
                                    .OffsetY(outage.ActualOutage * scale)

                                    .LineHorizontal(1);

                                    layer.Layer()
                                    .OffsetY(outage.ActualOutage * scale)
                                    .Text($"{outage.ProductName}");

                                    
                                }
                                // Initial Outage render
                                layer.Layer()
                             .OffsetY(_vesselManager.Vessel.InitialOutage * scale)
                             .LineHorizontal(1);

                                layer.Layer()
                                .OffsetY(_vesselManager.Vessel.InitialOutage * scale)
                                .Text($"Inital Outage: {_vesselManager.Vessel.InitialOutage}");

                                // 0 Mark render


                                layer.Layer()
                                .OffsetY(0)
                                .LineHorizontal(1);

                                layer.Layer()
                                .OffsetY(0)
                                .Text($"Base Mark: 00000");
                            });
                            

                   
                    });
                });
                page.Footer();
            });
        })

 .ShowInCompanion();

    }

    void DisplayerVesselInformation(TableDescriptor table)
    {
        // Vessel Information Header
        table.Cell().ColumnSpan(2)
            .Background(Colors.Grey.Lighten2).Element(CellStyle)
            .Text("Vessel Information:");
        // Vessel ID
        table.Cell().Element(CellStyle).Text("Vessel ID:");
        table.Cell().Element(CellStyle).Text($"{Placeholders.Integer()}");
        // Vessel Type
        table.Cell().Element(CellStyle).Text("Vessel Type:");
        table.Cell().Element(CellStyle).Text($"_{_vesselManager.Vessel.VesselType}_");
        // Bed Number
        table.Cell().Element(CellStyle).Text("Number of Beds:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.Vessel.BedNumber}");
        // Initial Outage
        table.Cell().Element(CellStyle).Text("Initial Outage:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.Vessel.InitialOutage} mm");
        // Internal Diameter:
        table.Cell().Element(CellStyle).Text("Internal Diameter:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.Vessel.InternalDiameter} mm");
        // Load Information
        table.Cell().ColumnSpan(2)
            .Background(Colors.Grey.Lighten2).Element(CellStyle)
            .Text("Load Information:");
        // Number of Layers
        table.Cell().Element(CellStyle).Text("Total Number of Layers:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.Layers.Count}");

        static IContainer CellStyle(IContainer container)
            => container.Border(0.4f, Unit.Point).Padding(1);
    }
    void DisplayProjectInformation(TableDescriptor table)
    {
        // Table header
        table.Cell().ColumnSpan(2)
            .Background(Colors.Grey.Lighten2).Element(CellStyle)
            .Text("Project Information:");
        // Job Description
        table.Cell().Element(CellStyle).Text("Job Description:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.Vessel.JobDescription}");
        // Job Location
        table.Cell().Element(CellStyle).Text("Job Number:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.Vessel.JobNumber}");
        // Client Name
        table.Cell().Element(CellStyle).Text("Client Name:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.Vessel.ClientName}");
        // Client Address
        table.Cell().Element(CellStyle).Text("Client Location:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.Vessel.ClientAddress}");

        static IContainer CellStyle(IContainer container)
                => container.Border(0.4f, Unit.Point).Padding(1);
    }
    void DisplayLayers(TableDescriptor table)
    {
        int layerNum = 1;
        foreach (var layer in _vesselManager.Layers)
        {     
            // Layer Number
            table.Cell().ColumnSpan(2)
            .Background(Colors.Grey.Lighten2).Element(CellStyle).Text($"Layer: {layerNum} ");
            // Layer Type:
            table.Cell().Element(CellStyle).Text("Layer Type:");
            table.Cell().Element(CellStyle).Text($"{layer.LayerType}");
            // Layer Material
            table.Cell().Element(CellStyle).Text("Material:");
            table.Cell().Element(CellStyle).Text($"{layer.ProductName}");
            // Layer Load Method
            table.Cell().Element(CellStyle).Text("Load Method:");
            table.Cell().Element(CellStyle).Text($"{layer.LoadMethod}");
            // Layer Target Outage
            table.Cell().Background(Colors.Amber.Accent1).Element(CellStyle).Text("Target Outage:");
            table.Cell().Element(CellStyle).Text($"{layer.TargetOutage} mm");
            // Layer Actual Outage
            table.Cell().Element(CellStyle).Text("Actual Outage:");
            table.Cell().Element(CellStyle).Text($"{layer.ActualOutage} mm");
            // Layer Drum Number
            table.Cell().Element(CellStyle).Text("Total Drums:");
            table.Cell().Element(CellStyle).Text($"{layer.DrumQuanity}");
            // Layer Drum Weight
            table.Cell().Element(CellStyle).Text("Drum Net Weight:");
            table.Cell().Element(CellStyle).Text($"{layer.DrumNetWeight} kg");
            // Layer Density
            table.Cell().Element(CellStyle).Text("Layer Density");
            table.Cell().Element(CellStyle).Text($"{_vesselManager.CalculateLayerDensity(layer):F2} kg/m3");

            static IContainer CellStyle(IContainer container)
                => container.Border(0.4f, Unit.Point).Padding(1);

            layerNum++;
        }
    }
    void DisplayCompanyImage(TableDescriptor table)
    {
        table.Cell().ColumnSpan(2)
        .Border(0.4f, Unit.Point)
        .Padding(10)
        .AlignCenter()
        .Image(Placeholders.Image(50, 25));
    }


}
