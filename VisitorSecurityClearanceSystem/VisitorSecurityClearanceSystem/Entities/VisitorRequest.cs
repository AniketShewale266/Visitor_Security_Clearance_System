using Newtonsoft.Json;

namespace VisitorSecurityClearanceSystem.Entities
{
    public class VisitorRequest
    {
        [JsonProperty(PropertyName = "requestId", NullValueHandling = NullValueHandling.Ignore)]
        public string RequestId { get; set; }

        [JsonProperty(PropertyName = "userId", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "location", NullValueHandling = NullValueHandling.Ignore)]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "createdTimestamp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreatedTimestamp { get; set; }

        [JsonProperty(PropertyName = "approvedTimestamp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ApprovedTimestamp { get; set; }

        [JsonProperty(PropertyName = "rejectedTimestamp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime RejectedTimestamp { get; set; }
    }
}
