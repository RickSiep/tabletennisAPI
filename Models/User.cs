namespace TableTennisAPI.Models {
    public class User {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public bool? IsAdmin { get; set; }
        public int? Elo { get; set; }

        public ICollection<Match> MatchAsWinner { get; set; }
        public ICollection<Match> MatchAsLoser { get; set; }
    }
}
