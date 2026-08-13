using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TableTennisAPI.Models;
using TableTennisAPI.Repositories.UserMatches;
using TableTennisAPI.Services.Matches;
using TableTennisShared.DTO.Match;

namespace TableTennisAPI.Controllers
{
    [Authorize]
    [Route("match")]
    [ApiController]
    public class MatchController(MatchService matchService, IUserMatchRepository userMatchRepository) : ControllerBase
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

        [HttpPost("save/single")]
        public async Task<ActionResult<Match>> RegisterSinglesMatch([FromBody]SinglesMatchDto singlesDto)
        {
            var match = await _matchService.SaveMatchAsync(singlesDto);
            if (match is null)
            {
                return BadRequest("Something went wrong saving the match, try again later");
            }

            return Ok(match);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Match>> UpdateMatch([FromBody] MatchSubmissionDto request)
        {
            var match = await _matchService.UpdateMatchAsync(request);
            if (match is null)
            {
                return BadRequest("Something went wrong updating the match");
            }
            return Ok(match);
        }

        [HttpGet]
        public async Task GetMatches() => Ok(await _matchService.GetAllMatchesAsync());

        [HttpGet("{matchId}")]
        public async Task<ActionResult<MatchSubmissionDto>> GetMatchById(int matchId)
        {
            var userMatches = await userMatchRepository.GetUserMatchesByMatchIdsAsync(matchId);
            if (userMatches.Count == 0)
            {
                NotFound($"No match found with id {matchId}");
            }

            var matchParticipants = new List<MatchParticipantDto>();
            foreach (var userMatch in userMatches)
            {
                matchParticipants.Add(new()
                {
                    UserId = userMatch.UserId,
                    IsWinner = userMatch.IsWinner,
                    TeamNumber = userMatch.TeamNumber,
                });
            }

            var match = await _matchService.GetMatchById(matchId);
            var matchSubmissionDto = new MatchSubmissionDto
            {
                WinnerScore = match?.WinnerScore ?? 0,
                LoserScore = match?.LoserScore ?? 0,
                Participants = matchParticipants
            };

            return Ok(matchSubmissionDto);
        }

        [HttpGet("/match/formatted")]
        public async Task<ActionResult<IEnumerable<MatchInformationDto>>> GetFormattedMatches(int pageIndex = 1, int pageSize = 10) => Ok(await _matchService.GetFormattedMatchesAsync(pageIndex, pageSize));

        [HttpGet("/match/formatted/date")]
        public async Task<ActionResult<MatchesPerDayDto>> GetFormattedMatchesByDate(int pageIndex = 1, int pageSize = 10) 
            => Ok(await _matchService.GetFormattedMatchesByDateAsync(pageIndex, pageSize));

        [HttpDelete("delete/{matchId}")]
        public async Task<ActionResult> DeleteMatch(int matchId)
        {
            await _matchService.DeleteMatchAsync(matchId);
            return Ok();
        }
    }
}
