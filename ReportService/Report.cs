using LoadReport.Main;
using LoadReport.Models;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;


namespace LoadReport.ReportService;

public class Report
{
    static Report()
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
    }

    VesselManager _vesselManager;  
    float RenderHeight = 700f;
    string FileName {  get; set; }
   
    
    public Report(VesselManager vesselManager, string fileName)
    {
        _vesselManager = vesselManager;
        FileName = fileName;
        
        GenerateReport();
    }

    void GenerateReport()
    {
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(10, Unit.Millimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(6));

                // Title Header
                page.Header()
                    .OffsetY(-10)
                    .Text($"Loading Report - {_vesselManager.Vessel.VesselID}")
                    .SemiBold().FontSize(36).FontColor(Colors.Black);

                // Page
                page.Content()
                    // First Row Table
                    .Row(row =>
                    {
                        row.ConstantItem(140)
                            .Column(column =>
                            {
                                // Left Side Table, Information
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
                            });

                        // Right Side of Table, Vessel Diagram
                        row.RelativeItem()
                            .Border(0.5F)
                            .Row(row =>
                            {

                                row.RelativeItem()
                                    .Width(370)
                                    .AlignCenter()
                                    .AlignMiddle()

                                .Layers(layer =>
                                {
                                    RenderVessel(layer);
                                });
                            });
                    });
            });
        })

        .GeneratePdf($"{FileName}");

    }



    void RenderVessel(LayersDescriptor layer)
    {
        // IMPORTANT:
        // All vessel SVG templates must be 220 x 700 with ViewBox="0 0 220 700"
        // Render offsets and outage positioning depend on this fixed coordinate system

        float textOffsetX = 20f;
        float textOffsetY = 1f;

        // Work out how much of the SVG is actually available
        // for rendering outage measurements
        float renderOffset = GetRenderOffset();
        float usableRenderHeight = RenderHeight - renderOffset;

        // Physical vessel measurement represented by the usable render area
        float vesselHeightMm = _vesselManager.Vessel.InitialOutage;

        // Points per millimetre
        float scale = usableRenderHeight / vesselHeightMm;


        // Drawing area
        layer.PrimaryLayer()
            .Width(370)
            .Height(RenderHeight);


        // Vessel SVG
        layer.Layer()
            .AlignRight()
            .Height(RenderHeight)
            .Svg(VesselTemplate());


        // Layer outage lines and labels

     
        foreach (var outage in _vesselManager.Layers)
        {
            float outagePosition = outage.ActualOutage * scale;

            layer.Layer()
                .OffsetY(outagePosition)
                .AlignRight()
                .Width(350)
                .LineHorizontal(1);

            layer.Layer()
                .OffsetX(textOffsetX)
                .OffsetY(textOffsetY + outagePosition)
                .Text($"{outage.ActualOutage}mm {outage.ProductName}");
        }


        // Initial outage
        float initialOutagePosition =
            _vesselManager.Vessel.InitialOutage * scale;

        layer.Layer()
            .OffsetY(initialOutagePosition)
            .AlignRight()
            .Width(350)
            .LineHorizontal(1);

        layer.Layer()
            .OffsetX(textOffsetX)
            .OffsetY(textOffsetY + initialOutagePosition)
            .Text($"{_vesselManager.Vessel.InitialOutage}mm Initial Outage");


        // 0 mark
        layer.Layer()
            .AlignRight()
            .Width(350)
            .LineHorizontal(1);

        layer.Layer()
            .OffsetX(textOffsetX)
            .OffsetY(textOffsetY)
            .Text("00000");
    }
    void DisplayerVesselInformation(TableDescriptor table)
    {
        // Vessel Information Header
        table.Cell().ColumnSpan(2)
            .Background(Colors.Grey.Lighten2).Element(CellStyle)
            .Text("Vessel Information:");
        // Vessel ID
        table.Cell().Element(CellStyle).Text("Vessel ID:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.Vessel.VesselID}");        
        // Bed Number
        table.Cell().Element(CellStyle).Text("Bed Number:");
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
            .Image(CompanyLogo());
    }

    string VesselTemplate()
    {
        string template = "Models/VesselTemplates/SingleBedVessel.svg";

        Template selection = _vesselManager.Vessel.TemplateType;

        switch (selection)
        {
            case Template.SingleBed: template = "Models/VesselTemplates/SingleBedVessel.svg"; break;
            case Template.SingleBedWithSupportGrid: template = "Models/VesselTemplates/SingleBedVesselWithTray.svg"; break;
            case Template.SecondaryReformer: template = "Models/VesselTemplates/SecondaryReformer.svg"; break;
            case Template.TopBed: template = "Models/VesselTemplates/MultiBedVesselTopBed.svg"; break;
            case Template.MiddleBed: template = "Models/VesselTemplates/MultiBedVesselMiddleBed.svg"; break;
            case Template.BottomBed: template = "Models/VesselTemplates/MultiBedVesselBottomBed.svg"; break;
        }
        return template;
    }

    int GetRenderOffset()
    {      
        int renderOffset;
        if (_vesselManager.Vessel.TemplateType == Template.SecondaryReformer || _vesselManager.Vessel.TemplateType ==  Template.SingleBedWithSupportGrid)
        {
            renderOffset = 100;
        }
        else
        {
            renderOffset = 0;
        }
        return renderOffset;
    }

    string CompanyLogo()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;

        string relevantDir = "ReportService\\Images\\NoUpload.png";

        string filePath = Path.Combine(baseDir, relevantDir);

       if (_vesselManager.CompanyLogo == null || _vesselManager.CompanyLogo == "") 
        {
           return  filePath;
        }
       else
        {
            return _vesselManager.CompanyLogo;
        }
        
    }
   
}
