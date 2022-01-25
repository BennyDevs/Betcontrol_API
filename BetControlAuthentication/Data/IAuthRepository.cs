using System;
using BetControlAuthentication.Models;

namespace BetControlAuthentication.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<bool> UserExists(string email, string username);
    }
}

