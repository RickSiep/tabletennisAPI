namespace TableTennisShared.DTO.Match
{
    public class MatchInformationWithTotalMatchesDto
    {
        public List<MatchInformationDto> MatchInformations { get; set; } = [];
        public int TotalMatches { get; set; }
    }
}
