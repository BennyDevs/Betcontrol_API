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

        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<IActionResult> GetUserBets()
        {
            var user = await GetUser();
            var bets = await _context.Bets.Where(bet => bet.UserId == user.Id).ToListAsync();

            if (bets == null)
                return NotFound("No bets found!");

            return Ok(bets);
        }

        //[MapToApiVersion("1.0")]
        //[HttpGet("getbookies")]
        //public async Task<IActionResult> GetUserBookies()
        //{
        //    var user = await GetUser();
        //    var bookies = await _context.Bets.Join(
        //        _context.Bookies,
        //        bet => bet.Id,
        //        bookie => bookie.Id,
        //        (bet, bookie) => new
        //        {
        //            BookieId = bookie.Id,
        //            Bet = bet,
        //            BookieName = bookie.Name
        //        }).ToListAsync();

        //    return Ok(bookies);
        //}

        [MapToApiVersion("1.0")]
        [HttpPost("addbet")]
        public async Task<IActionResult> AddBet([FromBody] Bet bet)
        {
            var user = await GetUser();

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
                Result = bet.Result
            };

            _context.Bets.Add(newBet);
            await _context.SaveChangesAsync();
            return Ok(user.Bets);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveBet(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid ID.");

            var user = await GetUser();

            var bet = await _context.Bets.FindAsync(id);
            if (bet == null)
                return BadRequest("Bet not found!");
            
            _context.Bets.Remove(bet);
            await _context.SaveChangesAsync();
            return Ok(user.Bets);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("usersports")]
        public async Task<ActionResult<List<Bet>>> GetUserSports()
        {
            var user = await GetUser();
            var userBets = await (from bet in _context.Bets.Where(bet => bet.UserId == user.Id) join sports in _context.Sports on bet.Sport.Id equals sports.Id select sports).ToListAsync();

            return Ok(userBets);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{page}")]
        public async Task<ActionResult<List<Bet>>> GetBets(int page)
        {
            var user = await GetUser();

            //var userBets = _context.Bets.Where(bet => bet.UserId == user.Id);

            var userBets = (from bet in _context.Bets.Where(bet => bet.UserId == user.Id) join sports in _context.Sports on bet.Sport.Id equals sports.Id select bet).AsNoTracking();

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
                AmountOfBets = userBets.Count()
            };

            return Ok(response);
        }

        //[HttpGet]
        //public async Task<ActionResult<List<Bet>>> GetUserBookies()
        //{
        //    var user = await GetUser();
        //    var userBookies = _context.Bets.Where(bet => bet.UserId == user.Id);

        //}

        //[HttpPost]
        //public ActionResult<List<Bet>> AddBet(Bet bet)
        //{
        //    return _betService.AddBet(bet);
        //}

        //[HttpDelete("{id}")]
        //public ActionResult<List<Bet>> RemoveBet(int id)
        //{
        //    return _betService.RemoveBet(id);
        //}
    }
}

