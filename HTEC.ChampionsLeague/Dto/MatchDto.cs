using HTEC.ChampionsLeague.Utils.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace HTEC.ChampionsLeague.Dto
{
    public class MatchDto
    {
        [Required]
        public string LeagueTitle { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int Matchday { get; set; }

        [Required]
        [StringLength(1)]
        public string Group { get; set; }

        [Required]
        public string HomeTeam { get; set; }

        [Required]
        public string AwayTeam { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DateTimeValidation]
        public string KickoffAt { get; set; }

        /// <summary>
        /// Format "1:0"
        /// </summary>
        [Required]
        [ScoreValidation]
        public string Score { get; set; }
    }
}
