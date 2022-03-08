using System.Globalization;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Microsoft.Extensions.Configuration;

namespace TourPlanner.Common;

public class TourReport
{
    private static readonly IConfigurationRoot Config = AppSettings.GetInstance().Configuration;
    
    private Tour Tour { get; set; }
    private List<TourLog> Logs { get; set; }

    public TourReport(Tour tour, List<TourLog> logs)
    {
        Tour = tour;
        Logs = logs;
    }
    
    public void ToPdf()
    {
        var path = Path.Join(Config["PersistenceFolder"], $"{Tour.Name}.pdf");
        var fileStream = new FileStream(path, FileMode.Create);

        DocumentBuilder
            .New()
            .AddSection()
            .SetOrientation(PageOrientation.Portrait)

            // Header
            .AddHeaderToBothPages(25)
            .AddParagraph($"Report: {Tour.Name}")
            .ToSection()
            
            .AddParagraph($"TourPlanner Report: {Tour.Name}")
            .SetBold()
            .SetFontSize(16)
            .SetMarginBottom(10)
            .ToSection()

            // Generic tour info
            .AddTable()
                .SetContentPadding(5)
                .AddColumnPercentToTable("", 25)
                .AddColumnPercentToTable("", 75)
                    .AddRow()
                        .AddCellToRow("Name")
                        .AddCellToRow(Tour.Name)
                        .ToTable()
                    .AddRow()
                        .AddCellToRow("Description")
                        .AddCellToRow(Tour.Description)
                        .ToTable()
                    .AddRow()
                        .AddCellToRow("From")
                        .AddCellToRow(Tour.From.ToString())
                        .ToTable()
                    .AddRow()
                        .AddCellToRow("To")
                        .AddCellToRow(Tour.To.ToString())
                        .ToTable()
                    .AddRow()
                        .AddCellToRow("Transport")
                        .AddCellToRow(Tour.TransportType.ToString())
                        .ToTable()
                    .AddRow()
                        .AddCellToRow("Distance")
                        .AddCellToRow($"{Tour.Distance.ToString(CultureInfo.CurrentCulture)} km")
                        .ToTable()
                    .AddRow()
                        .AddCellToRow("Duration")
                        .AddCellToRow(Tour.EstimatedTime.ToString())
                        .ToTable()

            // Build and write to file stream
            .ToDocument()
            .Build(fileStream);

        fileStream.Close();    
    }
}
