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
    string FileName { get; set; }
    int _layerNum;

    public Report(VesselManager vesselManager, string fileName)
    {
        _vesselManager = vesselManager;
        FileName = fileName;
        _layerNum = 1;

        GenerateReport();
    }

    void GenerateReport()
    {
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(8, Unit.Millimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(6).FontFamily("Inter"));

                // Title Header
                page.Header()
                    .Row(row =>
                {
                        row.RelativeItem()
                            .Height(40)
                            .Layers(layer =>
                            {
                                layer.Layer()
                                    .AlignRight()
                                    .AlignTop()
                                    .Image(SetCompanyLogo())
                                    .FitHeight();

                                layer.PrimaryLayer()
                                    .OffsetY(0)
                                    .Text($"Loading Report {_vesselManager.Vessel.VesselID}")
                                    .SemiBold().FontSize(30).FontColor(Colors.Black);
                            });
                    });

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

        //.ShowInCompanion();
         .GeneratePdf($"{FileName}");
           
    }

    void RenderVessel(LayersDescriptor layer)
    {
        // IMPORTANT:
        // All vessel SVG templates must be 220 x 700 with ViewBox="0 0 220 700"
        // Render offsets and outage positioning depend on this fixed coordinate system

        float textOffsetX = 45f;
        float textOffsetY = -5.5f;
        float layerLineWeight = 1f;
        const float textFontSize = 5f;
        float layerLineWidth = 325;

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
                .Width(layerLineWidth)
                .LineHorizontal(layerLineWeight);

            layer.Layer()
                .OffsetX(textOffsetX)
                .OffsetY(textOffsetY + outagePosition)
                .Text($"{outage.ActualOutage}mm {outage.ProductName}").FontSize(textFontSize);
           
        }


        // Initial outage
        float initialOutagePosition =
            _vesselManager.Vessel.InitialOutage * scale;

        layer.Layer()
            .OffsetY(initialOutagePosition)
            .AlignRight()
            .Width(layerLineWidth)
            .LineHorizontal(layerLineWeight);

        layer.Layer()
            .OffsetX(textOffsetX)
            .OffsetY(textOffsetY + initialOutagePosition)
            .Text($"{_vesselManager.Vessel.InitialOutage}mm Initial Outage").FontSize(textFontSize);



        // 0 mark
        layer.Layer()
            .AlignRight()
            .Width(layerLineWidth)
            .LineHorizontal(layerLineWeight);

        layer.Layer()
            .OffsetX(textOffsetX)
            .OffsetY(textOffsetY)
            .Text("Datum: 00000").FontSize(textFontSize);

        // Disclaimer
        layer.Layer()
            .AlignCenter()
            .AlignBottom()
            .OffsetY(20)
            .OffsetX(15)
            .Text("Schematic only — layer profiles may not reflect actual vessel geometry at heads or tangent lines").Italic().FontColor(Colors.Grey.Darken2);
    }

    void DisplayerVesselInformation(TableDescriptor table)
    {
        // Vessel Information Header
        table.Cell().ColumnSpan(2)
            .Background(Colors.Grey.Lighten2).Element(CellStyle)
            .Text("Vessel Information:").Bold();
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
            .Text("Load Information:").Bold();
        // Number of Layers
        table.Cell().Element(CellStyle).Text("Number of Layers:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.Layers.Count}");
        // Final Outage
        table.Cell().Element(CellStyle).Text("Final Outage:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.GetFinalOutage()}mm");
        // Bed Densety
        table.Cell().Element(CellStyle).Text("Catalyst/Media Bed Density:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.CalculateBedDensity():F2}kg/m3");

        static IContainer CellStyle(IContainer container)
            => container.Border(0.4f, Unit.Point).Padding(1);
    }

    void DisplayProjectInformation(TableDescriptor table)
    {
        // Table header
        table.Cell().ColumnSpan(2)
            .Background(Colors.Grey.Lighten2).Element(CellStyle)
            .Text("Project Information:").Bold();
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
        foreach (var layer in _vesselManager.Layers)
        {
            // Layer Number
            table.Cell().ColumnSpan(2)
                .Background(Colors.Grey.Lighten2).Element(CellStyle).Text($"Layer: {_layerNum} ").Bold();

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
            table.Cell().Element(CellStyle).Text("Target Outage:");
            table.Cell().Element(CellStyle).Text($"{layer.TargetOutage} mm");

            // Layer Actual Outage
            table.Cell().Element(CellStyle).Text("Actual Outage:");
            table.Cell().Element(CellStyle).Text($"{layer.ActualOutage} mm");

            // Layer Outage Difference 
            table.Cell().Background(OutageVarienceColor(layer)).Element(CellStyle).Text("Outage Difference:");
            table.Cell().Background(OutageVarienceColor(layer)).Element(CellStyle).Text($"{PlusOrMinusSymbol(layer)}{_vesselManager.CalculateOutageDiffereance(layer)} mm");

            // Layer Drum Number
            table.Cell().Element(CellStyle).Text("Total Drums:");
            table.Cell().Element(CellStyle).Text($"{layer.DrumQuanity}");

            // Layer Drum Weight
            table.Cell().Element(CellStyle).Text("Drum Net Weight:");
            table.Cell().Element(CellStyle).Text($"{layer.DrumNetWeight} kg");

            // Layer Loaded Mass
            table.Cell().Element(CellStyle).Text("Total Layer Mass:");
            table.Cell().Element(CellStyle).Text($"{_vesselManager.CalculateLayerMass(layer)} kg");

            // Layer Density
            table.Cell().Element(CellStyle).Text("Layer Density");
            table.Cell().Element(CellStyle).Text($"{_vesselManager.CalculateLayerDensity(layer):F2} kg/m3");

            static IContainer CellStyle(IContainer container)
                => container.Border(0.4f, Unit.Point).Padding(1);

            _layerNum++;
        }
    }

    string VesselTemplate()
    {
        string template = "Models/VesselTemplates/SingleBedVessel.svg";

        Template selection = _vesselManager.Vessel.TemplateType;

        switch (selection)
        {
            case Template.SingleBed:
                template = "Models/VesselTemplates/SingleBedVessel.svg";
                break;
            case Template.SingleBedWithSupportGrid:
                template = "Models/VesselTemplates/SingleBedVesselWithTray.svg";
                break;
            case Template.SecondaryReformer:
                template = "Models/VesselTemplates/SecondaryReformer.svg";
                break;
            case Template.TopBed:
                template = "Models/VesselTemplates/MultiBedVesselTopBed.svg";
                break;
            case Template.MiddleBed:
                template = "Models/VesselTemplates/MultiBedVesselMiddleBed.svg";
                break;
            case Template.BottomBed:
                template = "Models/VesselTemplates/MultiBedVesselBottomBed.svg";
                break;
        }
        return template;
    }

    int GetRenderOffset()
    {
        int renderOffset;
        if (_vesselManager.Vessel.TemplateType == Template.SecondaryReformer || _vesselManager.Vessel.TemplateType == Template.SingleBedWithSupportGrid)
        {
            renderOffset = 100;
        }
        else
        {
            renderOffset = 0;
        }
        return renderOffset;
    }

    string SetCompanyLogo()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;

        string relevantDir = "ReportService\\Images\\NoUpload.png";

        string filePath = Path.Combine(baseDir, relevantDir);

        if (_vesselManager.CompanyLogo == null || _vesselManager.CompanyLogo == "")
        {
            return filePath;
        }
        else
        {
            return _vesselManager.CompanyLogo;
        }
    }

    string PlusOrMinusSymbol(Layer layer)
    {
        float value = layer.TargetOutage - layer.ActualOutage;
        int sign = Math.Sign(value);

        if (sign > 0) return "+";
        else return "";
    }

    Color OutageVarienceColor(Layer layer)
    {
        if(layer.TargetOutage - layer.ActualOutage == 0) return Colors.Green.Lighten5;       
        else return Colors.Amber.Lighten5;
    }
}
