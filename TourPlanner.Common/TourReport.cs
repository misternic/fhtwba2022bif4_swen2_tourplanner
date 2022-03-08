using System.Collections.Generic;
using IronPdf;

namespace TourPlanner.Common
{
    public class TourReport
    {
        private Tour Tour { get; set; }
        private List<TourLog> Logs { get; set; }

        public TourReport(Tour tour, List<TourLog> logs)
        {
            Tour = tour;
            Logs = logs;
        }

        public void ToPdf()
        {
            // IronPdf.Installation.DefaultRenderingEngine = IronPdf.Rendering.PdfRenderingEngine.Chrome;
            var renderer = new IronPdf.ChromePdfRenderer();
            var document = renderer.RenderHtmlAsPdf("<h1>Html with CSS and Images</h1>");
            document.SaveAs($"{Tour.Name}.pdf");
        }
    }
}