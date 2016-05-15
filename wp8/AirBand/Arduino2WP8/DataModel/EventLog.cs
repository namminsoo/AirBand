using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Arduino2WP8
{
    class EventLog
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        
        [JsonProperty(PropertyName = "phoneNum")]
        public string PhoneNum { get; set; }

        [JsonProperty(PropertyName = "eventTime")]
        public DateTime eventTime { get; set; }
        
        [JsonProperty(PropertyName = "eventGpsX")]
        public string EventGpsX { get; set; }

        [JsonProperty(PropertyName = "eventGpsY")]
        public string EventGpsY { get; set; }
        
        
        [JsonProperty(PropertyName = "emergencyNum1")]
        public string EmergencyNum1 { get; set; }

        [JsonProperty(PropertyName = "emergencyNum2")]
        public string EmergencyNum2 { get; set; }

        [JsonProperty(PropertyName = "emergencyNum3")]
        public string EmergencyNum3 { get; set; }

        [JsonProperty(PropertyName = "autoinsuranceNum")]
        public string AutoinsuranceNum { get; set; }

        
        [JsonProperty(PropertyName = "loginKey")]
        public string LoginKey { get; set; }


        [JsonProperty(PropertyName = "containerName")]
        public string ContainerName { get; set; }

        [JsonProperty(PropertyName = "resourceName")]
        public string ResourceName { get; set; }

        [JsonProperty(PropertyName = "sasQueryString")]
        public string SasQueryString { get; set; }

        [JsonProperty(PropertyName = "imageUri")]
        public string ImageUri { get; set; } 

    }
}
