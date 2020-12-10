using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityAlert.Exceptions
{
    public class MissingKeyException : Exception
    {
        public MissingKeyException() { }

        public MissingKeyException(string message):base(String.Format("Invalid key: {0}", message)) { }
    }
}
