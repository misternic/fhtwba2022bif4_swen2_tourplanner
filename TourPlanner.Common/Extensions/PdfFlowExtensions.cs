using Gehtsoft.PDFFlow.Builder;

namespace TourPlanner.Common.Extensions;

public static class PdfFlowExtensions
{
    public static TableBuilder AddRowFromList(this TableBuilder tableBuilder, List<string> columns)
    {
        var rowBuilder = tableBuilder.AddRow();
        
        foreach (var column in columns)
        {
            rowBuilder.AddCellToRow(column);
        }
        
        rowBuilder.ToTable();
        
        return tableBuilder;
    }

    public static TableBuilder AddRowsFromList(this TableBuilder tableBuilder, List<List<string>> rows)
    {
        foreach (var row in rows)
        {
            tableBuilder.AddRowFromList(row);
        }

        return tableBuilder;
    }

    public static SectionBuilder AddHeading(this SectionBuilder sectionBuilder, string text)
    {
        return sectionBuilder
            .AddParagraph(text)
            .SetBold()
            .SetFontSize(16)
            .SetMarginBottom(10)
            .ToSection();
    }
}