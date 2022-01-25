using System;
using BetControlAuthentication.Models;

namespace BetControlAuthentication.Services
{
    public interface IBetService
    {
        List<Bet> GetAllBets();
    }
}

