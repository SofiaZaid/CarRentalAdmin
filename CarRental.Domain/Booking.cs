using System;
using System.Text.Json.Serialization;

namespace CarRentalAdministration.Domain
{
    public class Booking
    {

        public int Id { get; set; }

        public DateTime CustomerBirthDate { get; set; }

        public DateTime RentalDate { get; set; }

        public DateTime DateOfRentalEnd { get; set; }

        ///Property storing data on how many kilometers 
        ///the customer drove with their reservation,
        ///used to update the "new" value for total milage 
        ///of a specific car.
        public int KmDrivenWithinReservation { get; set; }

        public decimal? TotalCostOfRent { get; set; }

        public int CarId { get; set; }

        [JsonIgnore]
        public Car Car { get; set; }
    }
}
