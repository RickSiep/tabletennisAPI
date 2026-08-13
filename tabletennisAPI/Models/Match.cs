namespace TableTennisAPI.Models {
    public class Match(DateTime datePlayed, int winnerScore, int loserScore)
    {
        public int Id { get; set; }
        public DateTime DatePlayed { get; set; } = datePlayed;
        public int WinnerScore { get; set; } = winnerScore;
        public int LoserScore { get; set; } = loserScore;
        public List<User> Users { get; set; } = [];
        public List<UserMatch> UserMatches { get; set; } = [];
    }
}
