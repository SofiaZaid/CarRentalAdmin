using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CarRentalAdministration.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CarCategory
    {
        Compact,
        Premium,
        Minivan
    }

    public class Car
    {
        public int Id { get; set; }


        //To be updated after a booking/reservation has ended.
        public int TotalCarMilage { get; set; }

        public CarCategory Category { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}
