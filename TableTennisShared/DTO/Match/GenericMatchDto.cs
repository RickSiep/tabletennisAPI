namespace TableTennisShared.DTO.Match
{
    public class GenericMatchDto<TIn>
    {
        public required TIn MatchesDto { get; set; }
    }
}
