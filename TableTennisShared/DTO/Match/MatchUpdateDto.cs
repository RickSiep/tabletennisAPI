using System.Runtime.Serialization;

namespace TableTennisShared.DTO.Match
{
    [DataContract]
    public class MatchUpdateDto
    {
        [DataMember]
        public int MatchId { get; set; }

        [DataMember]
        public MatchSubmissionDto MatchSubmissionDto { get; set; } = new();
    }
}
