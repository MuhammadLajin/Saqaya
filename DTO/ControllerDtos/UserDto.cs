using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace SharedDTO.ControllerDtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public bool marketingConsent { get; set; }
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonIgnore]
        public string email { get; set; }

    }
}
