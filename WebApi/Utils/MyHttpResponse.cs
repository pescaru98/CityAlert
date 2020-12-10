using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CityAlert.Utils
{
    public class MyHttpResponse
    {
        public static readonly string SUCCESSFULL_OPERATION = "Operation success!";
        public static readonly string ERROR_ITEM_NOT_FOUND = "The item was not found!";
        public static readonly string ERROR_ITEM_EXISTENT = "The item is already existent!";
        public static readonly string ERROR_INVALID = "Invalid request!";

        public static HttpResponseMessage CreateResponse(HttpStatusCode code, string reason, string content = "")
        {
            HttpResponseMessage message = new HttpResponseMessage(code);
            message.Content = new StringContent(content, Encoding.Unicode);
            message.ReasonPhrase = reason;
            return message;
        }
    }
}
