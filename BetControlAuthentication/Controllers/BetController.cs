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
using Microsoft.EntityFrameworkCore.Query;

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
        private string? mostCommonTipster;
        private string? mostCommonBookie;
        private string? mostCommonSport;

        public BetController(DataContext context)
        {
            _context = context;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        private async Task<User?> GetUser() => await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

        [MapToApiVersion("1.0")]
        [HttpPost("addbet")]
        public async Task<IActionResult> AddBet([FromBody] Bet bet)
        {
            User? user = await GetUser();

            if (user is not null)
            {
                double result = 0;

                // Switch statement to handle bet result
                result = bet.Status switch
                {
                    "WON" => (bet.Stake * bet.Odds) - bet.Stake,
                    "HALF WON" => ((bet.Stake * bet.Odds) - (bet.Stake)) / 2,
                    "LOST" => -bet.Stake,
                    "HALF LOST" => -(bet.Stake / 2),
                    _ => 0,
                };

                // Get user bets
                IIncludableQueryable<Bet, Tipster?>? userBets = _context.Bets.AsTracking()
                    .Where(bet => bet.UserId == user.Id)
                    .Include(bet => bet.Sport)
                    .Include(bet => bet.Bookie)
                    .Include(bet => bet.Tipster);

                // Get user bookies and check if already exist
                Bookie? bookie = bet.Bookie;
                if (bookie is not null)
                {
                    List<Bookie?>? userBookies = userBets.AsEnumerable().Select(b => b.Bookie).ToList();
                    bool bookieExist = userBookies.Any(b => b.Name.ToString().ToLower() == bookie.Name.ToString().ToLower());

                    if (bookieExist)
                        bookie = userBookies.Where(b => b.Name.ToLower() == bookie.Name.ToLower()).First();
                }

                // Get user tipsters and check if already exist
                Tipster? tipster = bet.Tipster;
                if (tipster is not null)
                {
                    List<Tipster?>? userTipsters = userBets.AsEnumerable().Select(b => b.Tipster).ToList();
                    bool tipsterExist = userTipsters.Any(t => t.Name.ToString().ToLower() == tipster.Name.ToString().ToLower());

                    if (tipsterExist)
                        tipster = userTipsters.Where(t => t.Name.ToLower() == tipster.Name.ToLower()).First();
                }

                // Get user sports and check if already exist
                Sport? sport = bet.Sport;
                if (sport is not null)
                {
                    List<Sport?>? userSports = userBets.AsEnumerable().Select(b => b.Sport).ToList();
                    bool sportExist = userSports.Any(s => s.Name.ToString().ToLower() == sport.Name.ToString().ToLower());

                    if (sportExist)
                        sport = userSports.Where(s => s.Name.ToLower() == sport.Name.ToLower()).First();
                }
               

                Bet? newBet = new Bet
                {
                    UserId = user.Id,
                    Selection = bet.Selection,
                    Event = bet.Event,
                    EventTime = bet.EventTime,
                    Odds = bet.Odds,
                    Stake = bet.Stake,
                    Bookie = bookie,
                    Sport = sport,
                    Tipster = tipster,
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
            return BadRequest("Not able to find user");
        }

        [MapToApiVersion("1.0")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBet(int id, [FromBody] Bet bet)
        {
            if (id <= 0)
                return BadRequest("Not a valid ID.");

            User? user = await GetUser();

            if (user is not null)
            {
                Bet? userBet = await _context.Bets.Where(bet => bet.Id == user.Id).FirstOrDefaultAsync(bet => bet.Id == id);

                if (bet == null)
                    return BadRequest("Bet not found and was therefore not able to update bet!");

                // Switch statement to handle bet result
                bet.Result = bet.Status switch
                {
                    "WON" => (bet.Stake * bet.Odds) - bet.Stake,
                    "HALF WON" => ((bet.Stake * bet.Odds) - (bet.Stake)) / 2,
                    "LOST" => -bet.Stake,
                    "HALF LOST" => -(bet.Stake / 2),
                    _ => 0,
                };

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
            return BadRequest("Not able to find user");
        }


        [MapToApiVersion("1.0")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveBet(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid ID.");

            var user = await GetUser();

            if (user is not null)
            {
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
            return BadRequest("Not able to find user");
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{page}")]
        public async Task<ActionResult<List<Bet>>> GetBets(int page)
        {
            User? user = await GetUser();

            if (user is not null)
            {
                var userBets = _context.Bets.AsNoTracking()
                    .Where(bet => bet.UserId == user.Id)
                    .Include(bet => bet.Sport)
                    .Include(bet => bet.Bookie)
                    .Include(bet => bet.Tipster);

                if (userBets is not null)
                {
                    IEnumerable<string>? userBookies = userBets.AsEnumerable().Select(bookie => bookie.Bookie.Name);
                    if (userBookies is not null)
                    {
                        mostCommonBookie = userBookies?.GroupBy(x => x)
                        .Select(x => new { BookieName = x.Key, Count = x.Count() })
                        .OrderByDescending(x => x.Count)
                        .FirstOrDefault().BookieName;
                    }

                    IEnumerable<string>? userSports = userBets.AsEnumerable().Select(sport => sport.Sport.Name);
                    if (userSports is not null)
                    {
                        mostCommonSport = userSports?.GroupBy(x => x)
                            .Select(x => new { SportName = x.Key, Count = x.Count() })
                            .OrderByDescending(x => x.Count)
                            .FirstOrDefault().SportName;
                    }

                    IEnumerable<string>? userTipsters = userBets.AsEnumerable().Select(tipster => tipster.Tipster.Name);
                    if (userTipsters is not null)
                    {
                        mostCommonTipster = userTipsters?.GroupBy(x => x)
                            .Select(x => new { TipsterName = x.Key, Count = x.Count() })
                            .OrderByDescending(x => x.Count)
                            .FirstOrDefault().TipsterName;
                    }

                    double results = userBets.Sum(bet => bet.Result);
                    double ROI = results / userBets.Count();

                    var pageResults = 10f;
                    double pageCount = Math.Ceiling(userBets.Count() / pageResults);

                    List<Bet> bets = await userBets
                    .OrderByDescending(bet => bet.EventTime)
                    .Skip((page - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();

                    BetResponse response = new BetResponse
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
            }
            return BadRequest("Not able to find user");
        }

        [MapToApiVersion("1.0")]
        [HttpGet("betresults")]
        public async Task<ActionResult> GetResults()
        {
            User? user = await GetUser();

            if (user is not null)
            {
                var userBets = _context.Bets.AsNoTracking()
                .Where(bet => bet.UserId == user.Id)
                .Include(bet => bet.Sport)
                .Include(bet => bet.Bookie)
                .Include(bet => bet.Tipster);

                IEnumerable<string>? lastTwelveMonths = Enumerable.Range(0, 13).Select(i => DateTime.Now.AddMonths(1 + (i - 13))).Select(date => date.ToString("Y"));

                double results = userBets.Sum(bet => bet.Result);

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
            return BadRequest("Not able to find user");
        }
    }
}