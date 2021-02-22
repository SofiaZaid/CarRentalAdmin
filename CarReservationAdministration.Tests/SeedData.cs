using CarRentalAdministration.DAL;
using CarRentalAdministration.Domain;
using System;

namespace CarRentalAdministration.Tests
{
    public class SeedData
    {
        public static void PopulateTestData(CarRentalDBContext context)
        {
            context.Cars.Add(new Car
            {
                Id= 1,
                Category = CarCategory.Compact,
                TotalCarMilage = 3000
            });

            context.Cars.Add(new Car
            {
                Id = 2,
                Category = CarCategory.Premium,
                TotalCarMilage = 10000
            });

            context.Cars.Add(new Car
            {
                Id = 3,
                Category = CarCategory.Minivan,
                TotalCarMilage = 400
            });

            context.Cars.Add(new Car
            {
                Id = 4,
                Category = CarCategory.Compact,
                TotalCarMilage = 500
            });

            context.Cars.Add(new Car
            {
                Id = 5,
                Category = CarCategory.Premium,
                TotalCarMilage = 583
            });

            context.Cars.Add(new Car
            {
                Id = 6,
                Category = CarCategory.Minivan,
                TotalCarMilage = 2086
            });

            //context.Bookings.Add(new Booking
            //{
            //    Id = 1,
            //    CarId = 1,
            //    CustomerBirthDate = new DateTime(1989, 09, 03),
            //    TotalCostOfRent = 0.0m,
            //    RentalDate = new DateTime(2021, 1, 2),
            //    DateOfRentalEnd = new DateTime(2021, 1, 2),
            //    KmDrivenWithinReservation = 0
            //});

            //context.Bookings.Add(new Booking
            //{
            //    Id = 2,
            //    CarId = 3,
            //    CustomerBirthDate = new DateTime(1989, 09, 03),
            //    TotalCostOfRent = 0.0m,
            //    RentalDate = new DateTime(2021, 2, 2),
            //    DateOfRentalEnd = new DateTime(2021, 2, 3),
            //    KmDrivenWithinReservation = 0
            //});

        }
        
    }
}
