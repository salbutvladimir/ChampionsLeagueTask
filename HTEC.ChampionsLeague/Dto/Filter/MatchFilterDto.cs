using HTEC.ChampionsLeague.Utils.Constants;
using System;
using static HTEC.ChampionsLeague.Utils.Validation.CustomValidation;

namespace HTEC.ChampionsLeague.Dto
{
    public class MatchFilterDto
    {
        public DateTime? DateFrom { get; set; }

        [DateGreaterThanAttribute(otherPropertyName = ProjectConstants.DateFrom, ErrorMessage = ("DateTo should be greater than DateFrom"))]
        public DateTime? DateTo { get; set; }

        public string[] Group { get; set; }

        public string[] Team { get; set; }
    }
}
