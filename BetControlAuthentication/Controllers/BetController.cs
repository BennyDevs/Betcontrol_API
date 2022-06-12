using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BetControlAuthentication.Data;
using BetControlAuthentication.Models;
using BetControlAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BetControlAPI.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")] // for backward compatibility
    [Authorize]
    [EnableCors("ReactPolicy")]
    public class BetController : ControllerBase
    {
        private readonly DataContext _context;
        private string mostCommonBookie;

        public BetController(DataContext context)
        {
            _context = context;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        private async Task<User> GetUser() => await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

        [MapToApiVersion("1.0")]
        [HttpPost("addbet")]
        public async Task<IActionResult> AddBet([FromBody] Bet bet)
        {
            User? user = await GetUser();

            double result = 0;

            switch (bet.Status)
            {
                case "WON":
                    result = (bet.Stake * bet.Odds) - bet.Stake;
                    break;
                case "HALF WON":
                    result = ((bet.Stake * bet.Odds) - (bet.Stake)) / 2;
                    break;
                case "LOST":
                    result = -bet.Stake;
                    break;
                case "HALF LOST":
                    result = -(bet.Stake / 2);
                    break;
                default:
                    result = 0;
                    break;
            }

            var userBets = _context.Bets.AsNoTracking()
                .Where(bet => bet.UserId == user.Id)
                .Include(bet => bet.Sport)
                .Include(bet => bet.Bookie)
                .Include(bet => bet.Tipster);

            Bookie? bookie = bet.Bookie;
            List<Bookie?>? userBookies = userBets.AsEnumerable().Select(b => b.Bookie).ToList() ;
            bool bookieExist = userBookies.Any(b => b.Name.ToString().ToLower() == bookie.Name.ToString().ToLower());

            if (bookieExist)
                bookie = userBookies.Where(b => b.Name.ToLower() == bookie.Name.ToLower()).First();

            Bet? newBet = new Bet
            {
                UserId = user.Id,
                Selection = bet.Selection,
                Event = bet.Event,
                EventTime = bet.EventTime,
                Odds = bet.Odds,
                Stake = bet.Stake,
                BookieId = bookie.Id,
                Bookie = bookie,
                Sport = bet.Sport,
                Tipster = bet.Tipster,
                Status = bet.Status,
                Locked = bet.Locked,
                Result = result
            };

            try
            {
                _context.Bets.Add(newBet);
                await _context.SaveChangesAsync();
                return Ok(user.Bets);
            }
            catch (DbUpdateException)
            {
                return BadRequest("Was not able to add bet.");
            }

        }

        [MapToApiVersion("1.0")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBet(int id, [FromBody] Bet bet)
        {
            if (id <= 0)
                return BadRequest("Not a valid ID.");

            User? user = await GetUser();
            Bet? userBet = await _context.Bets.Where(bet => bet.Id == user.Id).FirstOrDefaultAsync(bet => bet.Id == id);

            if (bet == null)
                return BadRequest("Bet not found and was therefore not able to update bet!");

            switch (bet.Status)
            {
                case "WON":
                    bet.Result = (bet.Stake * bet.Odds) - bet.Stake;
                    break;
                case "HALF WON":
                    bet.Result = ((bet.Stake * bet.Odds) - (bet.Stake)) / 2;
                    break;
                case "LOST":
                    bet.Result = -bet.Stake;
                    break;
                case "HALF LOST":
                    bet.Result = -(bet.Stake / 2);
                    break;
                default:
                    bet.Result = 0;
                    break;
            }

            try
            {
                _context.Bets.Update(bet);
                await _context.SaveChangesAsync();
                return Ok(user.Bets);
            }
            catch (DbUpdateException)
            {
                return BadRequest("Failed removing bet.");
            }

        }


        [MapToApiVersion("1.0")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveBet(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid ID.");

            var user = await GetUser();

            Bet? bet = await _context.Bets.Where(bet => bet.Id == user.Id).FirstOrDefaultAsync(bet => bet.Id == id);
            if (bet == null)
                return BadRequest("Bet not found!");

            try
            {
                _context.Bets.Remove(bet);
                await _context.SaveChangesAsync();
                return Ok(user.Bets);
            }
            catch (DbUpdateException)
            {
                return BadRequest("Failed removing bet.");
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{page}")]
        public async Task<ActionResult<List<Bet>>> GetBets(int page)
        {
            var user = await GetUser();

            var userBets = _context.Bets.AsNoTracking()
                .Where(bet => bet.UserId == user.Id)
                .Include(bet => bet.Sport)
                .Include(bet => bet.Bookie)
                .Include(bet => bet.Tipster);

            IEnumerable<string>? userBookies = userBets.AsEnumerable().Select(bookie => bookie.Bookie.Name);
            var bookieQuery = userBookies.GroupBy(x => x)
                                         .Select(x => new { BookieName = x.Key, Count = x.Count() })
                                         .OrderByDescending(x => x.Count);
            string? mostCommonBookie = bookieQuery.First().BookieName;

            IEnumerable<string>? userSports = userBets.AsEnumerable().Select(sport => sport.Sport.Name);
            var sportQuery = userSports.GroupBy(x => x)
                                       .Select(x => new { SportName = x.Key, Count = x.Count() })
                                       .OrderByDescending(x => x.Count);
            string? mostCommonSport = sportQuery.First().SportName;

            IEnumerable<string>? userTipsters = userBets.AsEnumerable().Select(tipster => tipster.Tipster.Name);
            var tipsterQuery = userTipsters.GroupBy(x => x)
                                           .Select(x => new { TipsterName = x.Key, Count = x.Count() })
                                           .OrderByDescending(x => x.Count);
            string? mostCommonTipster = tipsterQuery.First().TipsterName;

            double results = userBets.Sum(bet => bet.Result);
            double ROI = results / userBets.Count();

            var pageResults = 10f;
            double pageCount = Math.Ceiling(userBets.Count() / pageResults);

            List<Bet>? bets = await userBets
                .OrderByDescending(bet => bet.EventTime)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            BetResponse? response = new BetResponse
            {
                Bets = bets,
                CurrentPage = page,
                Pages = (int)pageCount,
                AmountOfBets = userBets.Count(),
                UserResult = results,
                ROI = ROI,
                UserBookies = userBookies.Distinct(),
                UserSports = userSports.Distinct(),
                UserTipsters = userTipsters.Distinct(),
                CommonUserBookie = mostCommonBookie,
                CommonUserSport = mostCommonSport,
                CommonUserTipster = mostCommonTipster
            };

            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("betresults")]
        public async Task<ActionResult> GetResults()
        {
            User? user = await GetUser();

            var userBets = _context.Bets.AsNoTracking()
                .Where(bet => bet.UserId == user.Id)
                .Include(bet => bet.Sport)
                .Include(bet => bet.Bookie)
                .Include(bet => bet.Tipster);

            IEnumerable<string>? lastTwelveMonths = Enumerable.Range(0, 13).Select(i => DateTime.Now.AddMonths(1 + (i - 13))).Select(date => date.ToString("Y"));

            double results = userBets.Sum(bet => bet.Result);
            //var months = userBets.AsEnumerable().Select(month => month.EventTime.ToString("Y")).Distinct().ToList();

            List<BetResultResponse> response = new List<BetResultResponse>();
            double overAllResults = 0;
            foreach (var month in lastTwelveMonths)
            {
                double result = userBets.AsEnumerable().Where(bet => bet.EventTime.ToString("Y") == month).Sum(bet => bet.Result);
                overAllResults += result;

                response.Add(new BetResultResponse
                {
                    MonthlyResult = result,
                    OverAllResult = overAllResults,
                    Month = month,
                });
            }

            return Ok(response);
        }
    }
}