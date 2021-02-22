using System;

namespace CarRentalAdministration.BLL
{
    public class DBItemNotFoundException : Exception
    {
        public DBItemNotFoundException()
        {
        }

        public DBItemNotFoundException(string message)
            : base(message)
        {
        }

        public DBItemNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
