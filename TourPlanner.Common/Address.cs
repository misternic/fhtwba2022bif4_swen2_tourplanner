using System.Text;

namespace TourPlanner.Common;

public class Address
{
    public string Road { get; set; } = "";
    public string City { get; set; } = "";
    public string Number { get; set; } = "";

    public new string ToString()
    {
        var bytes = Encoding.Default.GetBytes($"{Road} {Number}, {City}, AT");
        return Encoding.UTF8.GetString(bytes);
    }
}