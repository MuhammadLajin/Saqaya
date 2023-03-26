using System.ComponentModel.DataAnnotations;

namespace SharedDTO.ControllerDtos
{
    public class ReturnCreateUserDto
    {
        public string Id { get; set; }
        public string accessToken { get; set; }
    }
}
