using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Galery.Models
{
    //[JsonObject(IsReference = true)]
    [JsonObject(MemberSerialization.OptIn)]
    public class Photo
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("upload")]
        public DateTime UpLoad { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
        //[XmlIgnore]
        public virtual Album Album { get; set; }
    }
}