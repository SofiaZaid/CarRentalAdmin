using System;

namespace CarRentalAdministration.BLL
{
    public class CarAlreadyReturnedException : Exception
    {
        public CarAlreadyReturnedException()
        {
        }

        public CarAlreadyReturnedException(string message)
            : base(message)
        {
        }

        public CarAlreadyReturnedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
