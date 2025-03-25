using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TableTennisAPI.Models {
    public class Match {
        public int Id { get; set; }

        public int WinnerScore { get; set; }
        public int LoserScore { get; set; }

        [ForeignKey("MatchWinner")]
        public int? MatchWinnerID { get; set; }
        public User? MatchWinner { get; set; }

        [ForeignKey("MatchLoser")]
        public int? MatchLoserID { get; set; }
        public User? MatchLoser { get; set; }
    }
}
