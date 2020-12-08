using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityAlert.Entities
{
    public class Message : IResult
    {
        public Message(int messageId, string messageText)
        {
            MessageId = messageId;
            MessageText = messageText;
        }

        public int MessageId { get; set; }
        public string MessageText { get; set; }
    }
}
