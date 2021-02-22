using System;

namespace CarRentalAdministration.API
{
    public class ReturnOfCarInfo
    {
        public int BookingId { get; set; }

        public DateTime ActualReturnDate { get; set; }

        public int CurrentCarMilage { get; set; }
    }
}
