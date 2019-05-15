using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Galery.Models
{
    //   [JsonObject(IsReference = true)]
    [JsonObject(MemberSerialization.OptIn)]
    public class OtherAlbumModel
    {

        [JsonProperty("Albums")]
        [Required]
        public List<Album> albums { set; get; }
        [JsonProperty("Count")]
        [Required]
        public string count { set; get; }

    }
}