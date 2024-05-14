using GraduationProject.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Core.ServiceInterfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager);
    }
}
