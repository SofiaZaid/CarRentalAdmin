using CarRentalAdministration.Domain;
using System;

namespace CarRentalAdministration.API
{
    public class BookingInfo
    {
        public DateTime CustomerBirthDate { get; set; }

        public DateTime StartTimeOfRental { get; set; }

        public DateTime EndTimeOfRental { get; set; }

        public CarCategory Category { get; set; }
    }
}