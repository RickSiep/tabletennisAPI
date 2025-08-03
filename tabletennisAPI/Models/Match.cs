namespace TableTennisAPI.Models {
    public class Match {
        public int Id { get; set; }
        public DateTime DatePlayed { get; set; }
        public List<User> Users { get; set; } = [];
        public List<UserMatch> UserMatches { get; set; } = [];
    }
}
