
using GraduationProject.Core.Models;

namespace GraduationProject.Core.ServiceInterfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message);

    }
}
