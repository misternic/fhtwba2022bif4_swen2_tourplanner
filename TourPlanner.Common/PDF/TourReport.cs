using System.Globalization;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Microsoft.Extensions.Configuration;
using TourPlanner.Common.DTO;
using TourPlanner.Common.Extensions;

namespace TourPlanner.Common.PDF;

public class TourReport
{
    private static readonly IConfigurationRoot Config = AppSettings.GetInstance().Configuration;
    
    private TourDto TourDto { get; }
    private List<TourLogDto> Logs { get; }

    public TourReport(TourDto tourDto, List<TourLogDto> logs)
    {
        TourDto = tourDto;
        Logs = logs;
    }

    private void BuildPdfLogsSection(SectionBuilder section)
    {
        var formattedLogs = Logs.Select((log) => new List<string>
        {
            log.Date.ToString(),
            log.Duration.ToString(),
            log.Difficulty.ToString(),
            $"{log.Rating.ToString()} / 5",
            log.Comment.ToString(),
        }).ToList();
        
        section
            .SetOrientation(PageOrientation.Portrait)
            .AddHeading($"{TourDto.Name} | Logs")
            .AddTable()
            .SetBorderColor(Color.FromHtml("#c2c2c2"))
                .SetContentPadding(5)
                .SetAltRowStyle(
                    StyleBuilder.New()
                        .SetPaddingTop(new XUnit(5))
                        .SetPaddingBottom(new XUnit(5))
                        .SetPaddingLeft(new XUnit(5))
                        .SetPaddingRight(new XUnit(5))
                        .SetBorderColor(Color.FromHtml("#c2c2c2"))
                        .SetBackColor(Color.FromHtml("#f5f5f5"))
                )
                .AddColumnToTable("Date")
                .AddColumnToTable("Duration")
                .AddColumnToTable("Difficulty")
                .AddColumnToTable("Rating")
                .AddColumnToTable("Comment")
                .AddRowsFromList(formattedLogs)
            .ToDocument();
    }

    private void BuildPdfCoverSection(SectionBuilder section)
    {
        section
            .SetOrientation(PageOrientation.Portrait)
            .AddHeading($"{TourDto.Name} | Report")
            .AddTable()
            .SetContentPadding(8)
            .SetBorderColor(Color.FromHtml("#c2c2c2"))
            .AddColumnPercentToTable("", 25)
            .AddColumnPercentToTable("", 75)
                .AddRowFromList(new List<string> {"ID:", TourDto.Id.ToString()})
                .AddRowFromList(new List<string> {"Name:", TourDto.Name})
                .AddRowFromList(new List<string> {"Description:", TourDto.Description})
                .AddRowFromList(new List<string> {"From:", TourDto.From})
                .AddRowFromList(new List<string> {"To:", TourDto.To})
                .AddRowFromList(new List<string> {"Transport:", TourDto.TransportType.ToString()})
                .AddRowFromList(new List<string> {"Distance:", TourDto.Distance.ToString(CultureInfo.CurrentCulture)})
                .AddRowFromList(new List<string> {"Estimated Time:", TourDto.EstimatedTime.ToString()})
                .AddRow()
                    .AddCellToRow("Route:")
                    .AddCell()
                        .AddImage(Path.Join(Config["PersistenceFolder"], $"{TourDto.Id}.jpg"))
                            .SetWidth(400)
                            .SetHeight(400)
                        .ToCell()
                    .ToRow()
                .ToTable()
            .ToDocument();
    }

    public bool ExportToPdf()
    {
        return ExportToPdf(Path.Combine(Config["PersistenceFolder"], $"{TourDto.Name}.pdf"));
    }
    
    public bool ExportToPdf(string path)
    {
        try
        {
            var fileStream = new FileStream(path, FileMode.Create);

            var document = DocumentBuilder.New();
            document.AddSection(BuildPdfCoverSection);
            document.AddSection(BuildPdfLogsSection);
            document.Build(fileStream);

            fileStream.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
