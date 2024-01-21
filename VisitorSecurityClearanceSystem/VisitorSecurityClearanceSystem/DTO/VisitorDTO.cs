using Newtonsoft.Json;

namespace VisitorSecurityClearanceSystem.DTO
{
    public class VisitorDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Purpose { get; set; }
        public string VisitingTo { get; set; }
    }
}
