**Create branded catalyst-loading reports with layer calculations and a vessel schematic.**

LoadReport is a Windows desktop application built with C# and WPF. It turns project details, vessel dimensions, and layer-by-layer loading records into an A4 PDF report.

The report service currently configures QuestPDF's Community license setting. Dependency licensing should be reviewed for the intended distribution and use. This README does not assign a license to LoadReport.

Developed by **Connor Macdonald**.

## Features

- Record job description, job number, client name, and client location.
- Record vessel ID, bed number, initial outage, and internal diameter.
- Choose from six vessel schematic templates.
- Add catalyst, media, support, and tray layers.
- Record product names, target and actual outages, drum quantities, drum net weights, and loading methods.
- Calculate layer mass, layer density, and combined catalyst/media bed density.
- Highlight outage differences using green, amber, and red backgrounds.
- Display layer outage lines and product labels on the vessel schematic.
- Add a company logo and export a branded PDF.

### Run the published application

1. Keep `LoadReport.exe` and its accompanying published files and folders together, including `Models` and `ReportService`.
2. Open the published application folder and run `LoadReport.exe`.
   **The application is not digitally signed.** If Windows displays a Microsoft Defender SmartScreen warning, select **More info**, then **Run anyway** to launch it. Only do this for a copy obtained from a source you trust. On a managed work computer, your organisation's security settings may prevent this option from being available.
3. Enter the project and vessel details.
4. Optionally add a company logo using the **Add** button beside Company Logo.
5. Select **Add Layer** and enter each layer in loading order, from bottom to top.
6. Select **Generate**, choose a PDF filename, and save the report.

The application closes after generating the report. The project is configured to publish a self-contained Windows x64 executable, so that published build does not require a separate .NET runtime installation.

Run from the published folder: vessel template paths currently resolve relative to the working directory.

## Current limitations

- The schematic is illustrative. Layer volumes use a constant cylindrical cross-section and do not account for heads, tangent lines, or displaced volume from internals.
- The current workflow exports PDF; it does not provide saved, reloadable project files or editing of existing layers.
- Large layer counts or long labels may affect report layout. Check the exported PDF before sharing.

