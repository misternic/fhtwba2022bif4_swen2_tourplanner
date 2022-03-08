using Gehtsoft.PDFFlow.Builder;

namespace TourPlanner.Common;

public static class PDFFlowExtensions
{
    public static TableBuilder AddRowFromList(this TableBuilder tb, List<string> columns)
    {
        var rb = tb.AddRow();
        foreach (var column in columns)
        {
            rb.AddCellToRow(column);
        }
        rb.ToTable();
        return tb;
    }

    public static SectionBuilder AddHeading(this SectionBuilder sb, string text)
    {
        return sb
            .AddParagraph(text)
            .SetBold()
            .SetFontSize(16)
            .SetMarginBottom(10)
            .ToSection();
    }
}