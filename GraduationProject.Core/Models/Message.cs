using MimeKit;

namespace GraduationProject.Core.Models
{
    public class Message
    {
        public Message(IEnumerable<string> to, string? subject, string? content)
        {
            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            To = new List<MailboxAddress>();
            foreach (var address in to)
            {
                if (!string.IsNullOrWhiteSpace(address))
                {
                    To.Add(new MailboxAddress("Email", address));
                }
            }

            Subject = subject;
            Content = content;
        }

        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
