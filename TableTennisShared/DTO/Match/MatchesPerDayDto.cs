namespace TableTennisShared.DTO.Match
{
    public class MatchesPerDayDto
    {
        public DateTime Date { get; set; }
        public List<MatchInformationDto> Matches { get; set; } = [];
    }
}
