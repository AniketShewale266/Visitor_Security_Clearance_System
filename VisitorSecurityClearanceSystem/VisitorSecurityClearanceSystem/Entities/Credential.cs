using Newtonsoft.Json;

namespace VisitorSecurityClearanceSystem.Entities
{
    public class Credential
    {
        [JsonProperty(PropertyName = "credentialId", NullValueHandling = NullValueHandling.Ignore)]
        public int CredentialId { get; set; }

        [JsonProperty(PropertyName = "userId", NullValueHandling = NullValueHandling.Ignore)]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "credentialType", NullValueHandling = NullValueHandling.Ignore)]
        public string CredentialType { get; set; }
    }
}
