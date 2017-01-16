using System.Collections.Generic;

namespace AMPSchedules.Services
{
    public class UserInfo
    {
        public string Address { get; set; }
    }

    public class Message
    {
        public string Subject { get; set; }
        public ItemBody Body { get; set; }
        public List<Recipient> ToRecipients { get; set; }
    }

  public class Recipient
    {
        public UserInfo EmailAddress { get; set; }
    }

    public class ItemBody
    {
        public string ContentType { get; set; }
        public string Content { get; set; }
    }

    public interface IMessageRequest {
        Message Message { get; set; }
        bool SaveToSentItems { get; set; }
    }

    public class MessageRequest : IMessageRequest
    {
        public Message Message { get; set; }
        public bool SaveToSentItems { get; set; }
    }
}