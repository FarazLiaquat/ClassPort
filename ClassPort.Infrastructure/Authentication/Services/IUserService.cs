using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassPort.Domain.DTOs.Authentication;
using ClassPort.Domain.Entities.Identity;

namespace ClassPort.Infrastructure.Authentication.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<ApplicationUser?> GetById(string id);
    }
}
