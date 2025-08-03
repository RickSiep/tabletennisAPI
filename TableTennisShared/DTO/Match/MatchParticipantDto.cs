namespace TableTennisShared.DTO.Match
{
    public class MatchParticipantDto
    {
        public int UserId { get; set; }
        public bool IsWinner { get; set; }
        public int? TeamNumber { get; set; }
    }
}
