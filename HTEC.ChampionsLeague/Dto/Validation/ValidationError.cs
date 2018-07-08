using HTEC.ChampionsLeague.Utils.Constants;
using Newtonsoft.Json;

namespace HTEC.ChampionsLeague.Dto
{
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message != string.Empty ? message : ProjectConstants.InvalidJSON;
        }
    }
}
