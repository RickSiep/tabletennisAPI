 namespace TableTennisShared.DTO.Match
{
    public class MatchInformationDto
    {
        public required string WinnerName { get; set; }
        public required string LoserName { get; set; }
        public int WinnerScore { get; set; }
        public int LoserScore { get; set; }
        public int WinnerElo { get; set; }
        public int LoserElo { get; set; }
        public DateTime DatePlayed { get; set; }
        public bool Winner { get; set; }
        public int EloDifference { get; set; }
    }
}
