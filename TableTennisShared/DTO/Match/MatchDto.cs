namespace TableTennisShared.DTO.Match
{
    public class MatchDto
    {
        public required int MatchWinnerId { get; set; }
        public required int MatchLoserId { get; set; }
        public required int WinnerScore { get; set; }
        public required int LoserScore { get; set; }
    }
}
