using BetControlAuthentication.Models;

namespace BetControlAPI.Controllers
{
    internal class BetResultResponse
    {

        public string Month { get; internal set; } = String.Empty;
        public double MonthlyResult { get; internal set; }
        public double OverAllResult { get; internal set; }
    }
}