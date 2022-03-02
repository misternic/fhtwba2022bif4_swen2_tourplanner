namespace TourPlanner.Lib.BL
{
    public class Address
    {
        public string Road { get; set; }
        public string City { get; set; }
        public string Number { get; set; }

        public new string ToString()
        {
            return $"{Road} {Number}, {City}, AT";
        }
    }
}