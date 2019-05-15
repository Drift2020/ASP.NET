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
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("login")]
        public string Login { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("salt")]
        public string Salt { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}