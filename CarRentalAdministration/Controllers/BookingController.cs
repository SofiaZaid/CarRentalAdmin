using CarRentalAdministration.BLL;
using CarRentalAdministration.DAL;
using CarRentalAdministration.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CarRentalAdministration.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        public BookingController(CarRentalDBContext dbContext)
        {
            logicInstance = new ReservationOfCarsLogic(dbContext);
        }

        private ReservationOfCarsLogic logicInstance;

        /// <summary>
        /// Create a new car reservation in the system
        /// based on given information needed to make
        /// a reservation/booking.
        /// </summary>
        /// <param name="bookingInfo">Data from client with information neeeded to be able to
        /// register a new reservation.</param>
        /// <returns>A new Booking object</returns>
        [HttpPost]
        public ActionResult<Booking> CreateNewBooking(BookingInfo bookingInfo)
        {
            try
            {
                var newBooking = logicInstance.RentACar(bookingInfo.Category,
                                 bookingInfo.CustomerBirthDate,
                                 bookingInfo.StartTimeOfRental,
                                 bookingInfo.EndTimeOfRental);


                return new JsonResult(newBooking);
            }
            catch(DBItemNotFoundException ex)
            {
                return new JsonResult(ex.Message);
            }
        }


        /// <summary>
        /// Updates an existing booking in the system when a car is returned (rental ends).
        /// </summary>
        /// <param name="bookingEnd">Object containing the input from user about which booking we want to update, 
        /// what date the car is returned, what the current car milage is.</param>
        /// <returns>If successful: An updated booking with the total cost for the rental, else if the given 
        /// return date is wrong or the booking does not exist: badrequest with message about the error.</returns>
        [HttpPut]
        public ActionResult EndReservationAndUpdateBooking(ReturnOfCarInfo bookingEnd)
        {
            try
            {
                var updatedBooking = logicInstance.ReturnRentedCar(bookingEnd.BookingId, bookingEnd.ActualReturnDate, bookingEnd.CurrentCarMilage);
                return new JsonResult(updatedBooking);
            }
            catch (DateOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(DBItemNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(CarAlreadyReturnedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
