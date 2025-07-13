using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using TableTennisAPI.Models;
using TableTennisAPI.Services.Matches;
using TableTennisShared.DTO.Match;

namespace TableTennisAPI.Controllers
{
    [Authorize]
    [Route("match")]
    [ApiController]
    public class MatchController(MatchService matchService) : ControllerBase
    {
        private readonly MatchService _matchService = matchService;

        [HttpPost("save")]
        public async Task<ActionResult<Match>> RegisterMatch([FromBody]MatchDto request)
        {
            var match = await _matchService.SaveMatchAsync(winnerId: request.MatchWinnerId, loserId: request.MatchLoserId, winnerScore: request.WinnerScore, loserScore: request.LoserScore);

            if (match is null)
                return BadRequest("Something went wrong saving the match");

            return Ok();
        }

        [HttpGet]
        public async Task GetMatches() => Ok(new Match());
    }
}
