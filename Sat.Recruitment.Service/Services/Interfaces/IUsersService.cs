using Sat.Recruitment.Service.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Service.Services.Interfaces
{
    public interface IUsersService
    {
        Task<bool> CreateUser(User user, string userTextPath);
        Task<List<User>> GetUsers(  string userTextPath);

    }
}