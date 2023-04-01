using System;

namespace TaxiClient.Exceptions
{
    public class InvalideUser : Exception
    {
        public InvalideUser() { }

        public override string Message => "Incorrect login or password";
    }
}
