using CarRentalAdministration.DAL;
using CarRentalAdministration.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarRentalAdministration.BLL
{
    public class ReservationOfCarsLogic
    {
        public ReservationOfCarsLogic(CarRentalDBContext dbContext)
        {
            repos = new GenericRepository(dbContext);
        }

        private GenericRepository repos;

        ///These values could possibly have been part of domain instead but
        ///I made a decision to place it here since it felt easier to 
        ///have the values here when they are the same for all cars/all bookings.
        ///Here we can fairly easily change these constant values to update our 
        ///calculations.
        private readonly decimal baseDayRental = 1000m;
        private readonly decimal pricePerKm = 40m;

        private decimal CalculatePriceOfCarRental(Booking booking)
        {
            int nrOfDaysRented = (booking.DateOfRentalEnd - booking.RentalDate).Days + 1;

            switch (booking.Car.Category)
            {
                case CarCategory.Compact:
                    return baseDayRental * nrOfDaysRented;

                case CarCategory.Premium:
                    return (baseDayRental * nrOfDaysRented * 1.2m)
                        + (pricePerKm * booking.KmDrivenWithinReservation);

                case CarCategory.Minivan:
                    return (baseDayRental * nrOfDaysRented * 1.7m) + (pricePerKm * booking.KmDrivenWithinReservation * 1.5m);

                default:
                    throw new ArgumentException($"{booking.Car.Category} not in system");
            }
        }

        /// <summary>
        /// Calculates total price for the booking when the car is returned, an returns the updated
        /// booking with the updated total price as well as updated "enddate" according to which date
        /// the car was actually returned.
        /// </summary>
        /// <param name="bookingNumber">Booking id connected to the reservation that is now ending (car is returned).</param>
        /// <param name="returnDate">Date of the actual return of the car.</param>
        /// <param name="currentCarMilage">The current car milage when the car is returned.</param>
        /// <returns>The booking object with upddated totalprice for the rental after the car is returned.</returns>
        public Booking ReturnRentedCar(int bookingNumber, DateTime returnDate, int currentCarMilage)
        {
            var theBooking = repos.DataSet<Booking>().Include(c => c.Car).SingleOrDefault(b => b.Id == bookingNumber);

            if(theBooking == null)
            {
                throw new DBItemNotFoundException("There is no booking with the given bookingnumber, please check entered number.");
            }

            if(returnDate < theBooking.RentalDate)
            {
                throw new DateOutOfRangeException("ReturnDate for the car can not be before startdate of the booking/reservation.");
            }

            if(theBooking.TotalCostOfRent != null)
            {
                throw new CarAlreadyReturnedException("Car has already been registered as returned in the system.");
            }

            if(currentCarMilage < theBooking.Car.TotalCarMilage)
            {
                throw new ArgumentException("Can not set total car milage to a lower value than it was in the start of the rental period.");
            }

            var numberOfDrivenKilometers = currentCarMilage - theBooking.Car.TotalCarMilage;

            theBooking.DateOfRentalEnd = returnDate;
            theBooking.KmDrivenWithinReservation = numberOfDrivenKilometers;

            var totalRentalPrice = CalculatePriceOfCarRental(theBooking);

            theBooking.TotalCostOfRent = totalRentalPrice;

            repos.SaveChanges();

            return theBooking;
        }


        public Booking RentACar(CarCategory carCategory, DateTime customerBirthDate, DateTime dateOfRentalStart, DateTime dateOfRentalEnd)
        {
            var availableCars = GetAvailableCars(carCategory, dateOfRentalStart, dateOfRentalEnd);

            if(!availableCars.Any())
            {
                throw new DBItemNotFoundException("There were no available cars in the database for the specified car category and reservation dates.");
            }
            
            var chosenCar = availableCars.First();

            if(dateOfRentalEnd < dateOfRentalStart)
            {
                throw new DateOutOfRangeException("ReturnDate for the car can not be before startdate of the booking/reservation.");
            }

            var newBooking = new Booking
            {
                Car = chosenCar,
                CustomerBirthDate = customerBirthDate,
                RentalDate = dateOfRentalStart,
                DateOfRentalEnd = dateOfRentalEnd,
            };

            repos.Create(newBooking);
            repos.SaveChanges();
            return newBooking;
        }

        private ICollection<Car> GetAvailableCars(CarCategory carCategory, DateTime dateOfRentalStart, DateTime dateOfRentalEnd)
        {
            var carsInCategory = repos.DataSet<Car>().Where(c => c.Category == carCategory).Include(c => c.Bookings).ToList();
            var availableCars = carsInCategory.Where(c => c.Bookings.All(b => !BlocksNewBooking(b, dateOfRentalStart, dateOfRentalEnd))).ToList();
            return availableCars;
        }

        /// <summary>
        /// Control of whether a given startdate and given enddate
        /// has any overlap with already existing booking.
        /// </summary>
        /// <param name="booking">The booking that already exists and has a startdate and an enddate.</param>
        /// <param name="start">Start date that one wants to control availability for.</param>
        /// <param name="end">End date that one wants to control availability for.</param>
        /// <returns>True if the proposed dates are blocked by an already existing booking in anyway, else false.</returns>
        private bool BlocksNewBooking(Booking booking, DateTime start, DateTime end)
        {
            if(start >= booking.RentalDate && start <= booking.DateOfRentalEnd)
            {
                return true;
            }
            if(end >= booking.RentalDate && end <= booking.DateOfRentalEnd)
            {
                return true;
            }
            if(booking.RentalDate >= start && booking.DateOfRentalEnd <= start)
            {
                return true;
            }
            if (booking.RentalDate >= end  && booking.DateOfRentalEnd <= end)
            {
                return true;
            }
            return false;
        }
    }
}
