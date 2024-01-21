namespace VisitorSecurityClearanceSystem.DTO
{
    public class VisitorRequestDTO
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime ApprovedTimestamp { get; set; }
        public DateTime RejectedTimestamp { get; set; }
    }
}
