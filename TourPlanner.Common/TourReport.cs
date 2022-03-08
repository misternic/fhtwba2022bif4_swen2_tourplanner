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

    private void LogsSection(SectionBuilder section)
    {
        section
            .SetOrientation(PageOrientation.Portrait)
            .AddHeading($"{Tour.Name} | Logs")
            .ToDocument();
    }

    private void CoverSection(SectionBuilder section)
    {
        section
            .SetOrientation(PageOrientation.Portrait)
            .AddHeading($"{Tour.Name} | Report")
            .AddTable()
            .SetContentPadding(8)
            .SetBorderColor(Color.FromHtml("#c2c2c2"))
            .AddColumnPercentToTable("", 25)
            .AddColumnPercentToTable("", 75)
                .AddRowFromList(new List<string> {"ID:", Tour.Id.ToString()})
                .AddRowFromList(new List<string> {"Name:", Tour.Name})
                .AddRowFromList(new List<string> {"Description:", Tour.Description})
                .AddRowFromList(new List<string> {"From:", Tour.From.ToString()})
                .AddRowFromList(new List<string> {"To:", Tour.To.ToString()})
                .AddRowFromList(new List<string> {"Transport:", Tour.TransportType.ToString()})
                .AddRowFromList(new List<string> {"Distance:", Tour.Distance.ToString(CultureInfo.CurrentCulture)})
                .AddRowFromList(new List<string> {"Estimated Time:", Tour.EstimatedTime.ToString()})
                .AddRow()
                    .AddCellToRow("Route:")
                    .AddCell()
                        .AddImage(Path.Join(Config["PersistenceFolder"], $"{Tour.Id}.jpg"))
                            .SetWidth(400)
                            .SetHeight(400)
                        .ToCell()
                    .ToRow()
                .ToTable()
            .ToDocument();
    }
    
    public void ToPdf()
    {
        var path = Path.Join(Config["PersistenceFolder"], $"{Tour.Name}.pdf");
        var fileStream = new FileStream(path, FileMode.Create);

        var document = DocumentBuilder.New();
        document.AddSection(CoverSection);
        document.AddSection(LogsSection);
        document.Build(fileStream);

        fileStream.Close();    
    }
}
