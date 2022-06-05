using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.Common.DTO;

public class ExportDto
{
    public IEnumerable<TourDto> Tours { get; set; }
    public IEnumerable<TourLogDto> Logs { get; set; }
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}
