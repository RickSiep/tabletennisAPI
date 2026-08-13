using System.ComponentModel.DataAnnotations;

namespace TableTennisShared.DTO.Match
{
    public class SinglesMatchDto
    {
        public int WinnerId { get; set; }
        public int LoserId { get; set; }
        [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
        public int WinnerScore { get; set; }
        [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
        public int LoserScore { get; set; }
    }
}
