using CarRentalAdministration.BLL;
using CarRentalAdministration.DAL;
using CarRentalAdministration.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace CarRentalAdministration.Tests
{
    public class BookingTests
    {
        private ReservationOfCarsLogic CreateLogicInstance(out CarRentalDBContext ctx)
        {
            Guid databaseName = Guid.NewGuid();
            DbContextOptionsBuilder<CarRentalDBContext> builder = new DbContextOptionsBuilder<CarRentalDBContext>().UseInMemoryDatabase(databaseName.ToString());
            CarRentalDBContext context = new CarRentalDBContext(builder.Options);
            SeedData.PopulateTestData(context);
            context.SaveChanges();

            ctx = context;
            return new ReservationOfCarsLogic(context);
        }

        [Fact]
        public void CanMakeBooking()
        {
            var logic = CreateLogicInstance(out var context);

            var booking = logic.RentACar(CarCategory.Compact, new DateTime(1990, 1, 1), new DateTime(2021, 2, 22), new DateTime(2021, 3, 1));

            Assert.Equal(CarCategory.Compact, booking.Car.Category);
            Assert.Equal(new DateTime(1990, 1, 1), booking.CustomerBirthDate);
            Assert.Equal(new DateTime(2021, 2, 22), booking.RentalDate);
            Assert.Equal(new DateTime(2021, 3, 1), booking.DateOfRentalEnd);

            var bookingInDB = context.Find<Booking>(booking.Id);

            Assert.Equal(CarCategory.Compact, bookingInDB.Car.Category);
            Assert.Equal(new DateTime(1990, 1, 1), bookingInDB.CustomerBirthDate);
            Assert.Equal(new DateTime(2021, 2, 22), bookingInDB.RentalDate);
            Assert.Equal(new DateTime(2021, 3, 1), bookingInDB.DateOfRentalEnd);
        }

        [Fact]
        public void CanNotMakeBookingForOverlappingDates()
        {
            var logic = CreateLogicInstance(out var context);

            logic.RentACar(CarCategory.Compact, new DateTime(1990, 1, 1), new DateTime(2021, 2, 22), new DateTime(2021, 3, 1));

            logic.RentACar(CarCategory.Compact, new DateTime(1990, 1, 1), new DateTime(2021, 2, 22), new DateTime(2021, 3, 1));

            Assert.Throws<DBItemNotFoundException>(() =>
            {
                logic.RentACar(CarCategory.Compact, new DateTime(1990, 1, 1), new DateTime(2021, 2, 22), new DateTime(2021, 3, 1));
            });

            Assert.Throws<DBItemNotFoundException>(() =>
            {
                logic.RentACar(CarCategory.Compact, new DateTime(1987, 2, 1), new DateTime(2021, 2, 15), new DateTime(2021, 3, 1));
            });

            Assert.Throws<DBItemNotFoundException>(() =>
            {
                logic.RentACar(CarCategory.Compact, new DateTime(1990, 1, 1), new DateTime(2021, 2, 21), new DateTime(2021, 3, 1));
            });

            Assert.Throws<DBItemNotFoundException>(() =>
            {
                logic.RentACar(CarCategory.Compact, new DateTime(1990, 1, 1), new DateTime(2021, 2, 23), new DateTime(2021, 3, 1));
            });

            Assert.Throws<DBItemNotFoundException>(() =>
            {
                logic.RentACar(CarCategory.Compact, new DateTime(1990, 1, 1), new DateTime(2021, 2, 23), new DateTime(2021, 3, 3));
            });
        }

        [Fact]
        public void CanNotMakeBookingWithEndDateBeforeStartDate()
        {
            var logic = CreateLogicInstance(out var context);           

            Assert.Throws<DateOutOfRangeException>(() =>
            {
                logic.RentACar(CarCategory.Compact, new DateTime(1990, 1, 1), new DateTime(2021, 2, 22), new DateTime(2021, 2, 1));
            });
        }

        private void CanReturnCar(CarCategory category, decimal expectedPrice)
        {
            var logic = CreateLogicInstance(out var context);
            var booking = logic.RentACar(category, new DateTime(1990, 1, 1), new DateTime(2021, 2, 22), new DateTime(2021, 3, 1));
            var theBookingEnding = logic.ReturnRentedCar(booking.Id, new DateTime(2021, 3, 2), booking.Car.TotalCarMilage + 20);
            var bookingInDB = context.Find<Booking>(booking.Id);

            Assert.Equal(new DateTime(2021, 3, 2), bookingInDB.DateOfRentalEnd);
            Assert.Equal(theBookingEnding.TotalCostOfRent, bookingInDB.TotalCostOfRent);
            Assert.Equal(20, bookingInDB.KmDrivenWithinReservation);
            Assert.Equal(expectedPrice, bookingInDB.TotalCostOfRent);
        }

        [Fact]
        public void CanReturnPremiumCar()
        {
            CanReturnCar(CarCategory.Premium, 11600);
        }


        [Fact]
        public void CanReturnCompactCar()
        {
            CanReturnCar(CarCategory.Compact, 9000);
        }

        [Fact]
        public void CanReturnMinivanCar()
        {
            CanReturnCar(CarCategory.Minivan, 16500);
        }

        [Fact]
        public void CanNotReturnCarTwice()
        {
            var logic = CreateLogicInstance(out var context);
            var booking = logic.RentACar(CarCategory.Premium, new DateTime(1990, 1, 1), new DateTime(2021, 2, 22), new DateTime(2021, 3, 1));
            logic.ReturnRentedCar(booking.Id, new DateTime(2021, 3, 1), booking.Car.TotalCarMilage + 1);

            Assert.Throws<CarAlreadyReturnedException>(() => logic.ReturnRentedCar(booking.Id, new DateTime(2021, 3, 1), booking.Car.TotalCarMilage + 1));
        }

        [Fact]
        public void CanNotReturnCarWithLowerMilageThanBefore()
        {
            var logic = CreateLogicInstance(out var context);
            var booking = logic.RentACar(CarCategory.Premium, new DateTime(1990, 1, 1), new DateTime(2021, 2, 22), new DateTime(2021, 3, 1));
            
            Assert.Throws<ArgumentException>(() => logic.ReturnRentedCar(booking.Id, new DateTime(2021, 3, 1), booking.Car.TotalCarMilage -1));
        }
    }
}
