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

        [MapToApiVersion("1.0")]
        [HttpPost("addbet")]
        public async Task<IActionResult> AddBet([FromBody] Bet bet)
        {
            var user = await GetUser();
            //var bookie = await _context.Bookies.FirstOrDefaultAsync(bookie => bookie.Id == bet.Bookie.Id);
            //var user = await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower().Equals(email.ToLower()));

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

        [HttpGet("{page}")]
        public async Task<ActionResult<List<Bet>>> GetBets(int page)
        {
            var user = await GetUser();

            var amountOfBets = _context.Bets.Where(bet => bet.UserId == user.Id).Count();

            var pageResults = 10f;
            var pageCount = Math.Ceiling(amountOfBets / pageResults);

            var bets = await _context.Bets
                .Where(bet => bet.UserId == user.Id)
                .OrderByDescending(bet => bet.EventTime)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var response = new BetResponse
            {
                Bets = bets,
                CurrentPage = page,
                Pages = (int)pageCount,
                AmountOfBets = amountOfBets
            };

            return Ok(response);
        }

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

