using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace Galery.Models
{
    //[JsonObject(IsReference = true)]


[JsonObject(MemberSerialization.OptIn)]
    public class Album
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("number")]
        public int number { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("user")]
        public virtual User User { get; set; }
        [JsonProperty("photos")]
        public virtual ICollection<Photo> Photos { get; set; }
    }
}