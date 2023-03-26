using System.ComponentModel.DataAnnotations;

namespace SharedDTO.ControllerDtos
{
    public class CreateUserDto
    {
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string firstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string lastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string email { get; set; }
                
        public bool marketingConsent { get; set; }
    }
}
