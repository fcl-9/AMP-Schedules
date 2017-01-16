using System;
using System.Collections.Generic;

namespace AMPSchedules.Services
{
    public static class EmailMessageBuilder
    {
        public static MessageRequest Build( string aRecipients, string aSubject, string aContent )
        {
            // Prepare the recipient list.
            List<Recipient> recipients = new List<Recipient>();
            foreach ( string recipient in aRecipients.Split( new []{ ';' }, StringSplitOptions.RemoveEmptyEntries ) )
            {
                recipients.Add( new Recipient {
                    EmailAddress = new UserInfo { Address = recipient.Trim() }
                } );
            }

            // Build the email message.
            Message message = new Message {
                Body = new ItemBody {
                    Content = aContent,
                    ContentType = "HTML"
                },
                Subject = aSubject,
                ToRecipients = recipients
            };

            return new MessageRequest {
                Message = message,
                SaveToSentItems = true
            };
        }
    }
}