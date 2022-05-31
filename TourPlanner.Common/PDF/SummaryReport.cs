using System.Globalization;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Models.Shared;
using Microsoft.Extensions.Configuration;
using TourPlanner.Common.DTO;
using TourPlanner.Common.Extensions;

namespace TourPlanner.Common.PDF;

public class SummaryReport
{
    private static readonly IConfigurationRoot Config = AppSettings.GetInstance().Configuration;
    
    private ICollection<TourSummaryDto> Summaries { get; }
    
    public SummaryReport(ICollection<TourSummaryDto> summaries)
    {
        Summaries = summaries;
    }

    private void BuildPdfLogsSection(SectionBuilder section)
    {
        var formattedSummaries = Summaries.Select(s => new List<string>
        {
            s.Name,
            s.AvgRating.ToString(CultureInfo.InvariantCulture),
            s.AvgDifficulty.ToString(CultureInfo.InvariantCulture),
            s.AvgDuration.ToString(),
        }).ToList();
        
        section
            .SetOrientation(PageOrientation.Portrait)
            .AddHeading("TourPlanner Summary")
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
                .AddColumnToTable("Name")
                .AddColumnToTable("Rating")
                .AddColumnToTable("Difficulty")
                .AddColumnToTable("Duration")
            .AddRowsFromList(formattedSummaries)
            .ToDocument();
    }
    
    public bool ExportToPdf()
    {
        return ExportToPdf(Path.Combine(Config["PersistenceFolder"], "Summary.pdf"));
    }
    
    public bool ExportToPdf(string path)
    {
        try
        {
            var fileStream = new FileStream(path, FileMode.Create);

            var document = DocumentBuilder.New();
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
