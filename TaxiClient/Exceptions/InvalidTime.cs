using System;

namespace TaxiClient.Exceptions
{
    public class InvalidTime : Exception
    {
        public InvalidTime() { }

        public override string Message => "You need at least 10 minutes to travel\n" +
            $"Now is {DateTime.Now}\n";
    }
}
