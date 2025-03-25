using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TableTennisAPI.DTO.Match;
using TableTennisAPI.Models;
using TableTennisAPI.Services.Matches;

namespace TableTennisAPI.Controllers
{
    [Route("match")]
    [ApiController]
    public class MatchController(MatchService matchService) : ControllerBase
    {
        private readonly MatchService _matchService = matchService;

        [Authorize]
        [HttpPost("save")]
        public async Task<ActionResult<Match>> RegisterMatch([FromBody]MatchDTO request)
        {
            var match = await _matchService.SaveMatchAsync(winnerId: request.MatchWinnerId, loserId: request.MatchLoserId, winnerScore: request.WinnerScore, loserScore: request.LoserScore);

            if (match is null)
                return BadRequest("Something went wrong saving the match");

            return Ok();
        }
    }
}
