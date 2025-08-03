namespace TableTennisAPI.Models
{
    public class UserMatch
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;

        public bool IsWinner { get; set; }
        public int? TeamNumber { get; set; }
    }
}
