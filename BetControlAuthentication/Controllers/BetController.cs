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

        public BetController(DataContext context)
        {
            _context = context;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        private async Task<User> GetUser() => await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

        //[MapToApiVersion("1.0")]
        //[HttpGet]
        //public async Task<IActionResult> GetUserBets()
        //{
        //    var user = await GetUser();
        //    var bets = await _context.Bets.Where(bet => bet.UserId == user.Id).ToListAsync();

        //    if (bets == null)
        //        return NotFound("No bets found!");

        //    return Ok(bets);
        //}

        [MapToApiVersion("1.0")]
        [HttpPost("addbet")]
        public async Task<IActionResult> AddBet([FromBody] Bet bet)
        {
            var user = await GetUser();

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

            var newBet = new Bet
            {
                UserId = user.Id,
                Selection = bet.Selection,
                Event = bet.Event,
                EventTime = bet.EventTime,
                Odds = bet.Odds,
                Stake = bet.Stake,
                Bookie = bet.Bookie,
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
                return BadRequest("Failed creating new bet..");
            }

        }

        [MapToApiVersion("1.0")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBet(int id, [FromBody] Bet bet)
        {
            if (id <= 0)
                return BadRequest("Not a valid ID.");

            var user = await GetUser();
            var userBet = await _context.Bets.Where(bet => bet.Id == user.Id).FirstOrDefaultAsync(bet => bet.Id == id);

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
                _context.SaveChangesAsync();
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

            var bet = await _context.Bets.Where(bet => bet.Id == user.Id).FirstOrDefaultAsync(bet => bet.Id == id);
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

            var userBookies = userBets.AsEnumerable().Select(bookie => bookie.Bookie.Name);
            var bookieQuery = userBookies.GroupBy(x => x).Select(x => new { BookieName = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count);
            var mostCommonBookie = bookieQuery.First().BookieName;

            var userSports = userBets.AsEnumerable().Select(sport => sport.Sport.Name);
            var sportQuery = userSports.GroupBy(x => x).Select(x => new { SportName = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count);
            var mostCommonSport = sportQuery.First().SportName;

            var userTipsters = userBets.AsEnumerable().Select(tipster => tipster.Tipster.Name);
            var tipsterQuery = userTipsters.GroupBy(x => x).Select(x => new { TipsterName = x.Key, Count = x.Count() }).OrderByDescending(x => x.Count);
            var mostCommonTipster = tipsterQuery.First().TipsterName;

            double results = userBets.Sum(bet => bet.Result);
            double ROI = results / userBets.Count();

            var pageResults = 10f;
            var pageCount = Math.Ceiling(userBets.Count() / pageResults);

            var bets = await userBets
                .OrderByDescending(bet => bet.EventTime)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var response = new BetResponse
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
}