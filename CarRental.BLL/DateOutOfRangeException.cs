using System;

namespace CarRentalAdministration.BLL
{
    public class DateOutOfRangeException : Exception
    {
        public DateOutOfRangeException()
        {
        }

        public DateOutOfRangeException(string message)
            : base(message)
        {
        }

        public DateOutOfRangeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
