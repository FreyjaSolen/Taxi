using System;

namespace TaxiClient.Exceptions
{
    public class NullFields : Exception
    {
        public NullFields() { }

        public override string Message => "Please fill in all fields";
    }
}
