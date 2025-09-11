namespace TableTennisShared.DTO.Match
{
    public class MatchSubmissionDto
    {
        public List<MatchParticipantDto> Participants { get; set; } = [];
        public int WinnerScore { get; set; }
        public int LoserScore { get; set; }
    }
}
