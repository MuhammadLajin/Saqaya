using System.Collections.Generic;

namespace DomainLayer.Models
{
    public class User : BaseEntity
    {
        public User()
        {
        }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public bool marketingConsent { get; set; }
        public string accessToken { get; set; }
    }
}
