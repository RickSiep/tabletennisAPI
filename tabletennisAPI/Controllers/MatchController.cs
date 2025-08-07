using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TableTennisAPI.Models;
using TableTennisAPI.Services.Matches;
using TableTennisShared.DTO.Match;

namespace TableTennisAPI.Controllers
{
    //[Authorize]
    [Route("match")]
    [ApiController]
    public class MatchController(MatchService matchService) : ControllerBase
    {
        private readonly MatchService _matchService = matchService;

        [HttpPost("save")]
        public async Task<ActionResult<Match>> RegisterMatch([FromBody]MatchSubmissionDto request)
        {
            var match = await _matchService.SaveMatchAsync(request);

            if (match is null)
                return BadRequest("Something went wrong saving the match");

            return Ok();
        }

        [HttpGet]
        public async Task GetMatches() => Ok(await _matchService.GetAllMatchesAsync());

        [HttpGet("/match/formatted")]
        public async Task<ActionResult<IEnumerable<MatchInformationDto>>> GetFormattedMatches(int pageIndex = 1, int pageSize = 10) => Ok(await _matchService.GetFormattedMatchesAsync(pageIndex, pageSize));
    }
}
