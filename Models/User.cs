namespace TableTennisAPI.Models {
    public record User {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
        public int? Elo { get; set; }

        public ICollection<Match> MatchAsWinner { get; set; }
        public ICollection<Match> MatchAsLoser { get; set; }
    }
}
