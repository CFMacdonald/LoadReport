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
    float actualScale;

    public Report(VesselManager vesselManager)
    {
        TEMPLAYERS = new Layer[6];
        TEMPLAYERS[0] = new Layer("Support", "Ceramic Balls 1\"", 9850, 9850, 8, 25, "Sock Load");
        TEMPLAYERS[1] = new Layer("Support", "Ceramic Balls 1/2\"", 9700, 9700, 8, 25, "Sock Load");
        TEMPLAYERS[2] = new Layer("Catalyst", "Catalyst A", 7200, 7200, 30, 100, "Dense Load");
        TEMPLAYERS[3] = new Layer("Catalyst", "Catalyst B", 3200, 3200, 35, 100, "Dense Load");
        TEMPLAYERS[4] = new Layer("Top Support", "Ceramic Balls 1/2\"", 3050, 3050, 6, 25, "Sock Load");
        TEMPLAYERS[5] = new Layer("Top Support", "Ceramic Balls 1\"", 2900, 2900, 6, 25, "Sock Load");

        _vesselManager = vesselManager;
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
                    .Text("Loading Report")
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
                            .Height(710)
                            .Layers(layer =>
                            {
                                RenderVessel(layer);
                            });
                    });
            });
        })

        .ShowInCompanion();

    }

    void RenderVessel(LayersDescriptor layer)
    {
        float topOffset = 10f;
        float rightOffset = 10f;
        float xOffsetTextPadding = 38f;
        actualScale = _vesselManager.Vessel.InitialOutage;
        float scale = renderHeight / actualScale;

        layer.PrimaryLayer()
            .Width(395)
            .Height(renderHeight);

        layer.Layer()
            .AlignRight()
            .OffsetX(-rightOffset)
            .OffsetY(topOffset)
            .Height(renderHeight)
            .Svg("Models/VesselTemplates/SingleBedVessel.svg");

        foreach (var outage in TEMPLAYERS.Reverse())
        {
            layer.Layer()
                .OffsetX(-rightOffset)
                .OffsetY(topOffset + (outage.ActualOutage * scale))
                .AlignRight()
                .Width(350)
                .LineHorizontal(1);

            layer.Layer()
                .OffsetX(xOffsetTextPadding)
                .OffsetY(12 + (outage.ActualOutage * scale))
                .Text($"{outage.ActualOutage}mm {outage.ProductName}");
        }

        // Initial Outage
        layer.Layer()
            .OffsetX(-rightOffset)
            .OffsetY(topOffset + (_vesselManager.Vessel.InitialOutage * scale))
            .AlignRight()
            .Width(350)
            .LineHorizontal(1);

        layer.Layer()
            .OffsetY(12 + (_vesselManager.Vessel.InitialOutage * scale))
            .OffsetX(xOffsetTextPadding)
            .Text($"{_vesselManager.Vessel.InitialOutage}mm Intial Outage");

        // 0 Mark
        layer.Layer()
            .OffsetX(-rightOffset)
            .OffsetY(topOffset)
            .AlignRight()
            .Width(350)
            .LineHorizontal(1);

        layer.Layer()
            .OffsetX(xOffsetTextPadding)
            .OffsetY(2 + topOffset)
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
        table.Cell().Element(CellStyle).Text($"{Placeholders.Integer()}");
        // Vessel Type
        table.Cell().Element(CellStyle).Text("Vessel Type:");
        table.Cell().Element(CellStyle).Text($"{_vesselManager.Vessel}");
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
