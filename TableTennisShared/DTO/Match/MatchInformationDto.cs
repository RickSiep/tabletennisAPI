 namespace TableTennisShared.DTO.Match
{
    public class MatchInformationDto
    {
        public required string FirstName { get; set; }
        public int Elo { get; set; }
        public DateTime DatePlayed { get; set; }
        public bool Winner { get; set; }
        public int EloDifference { get; set; }
        public required string PlayedAgainst { get; set; }
    }
}
