using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Music2.Models
{
    public class SoundModel
    {
        public List<Sound> sound { get; set; }
        public List<Genre> genre { get; set; }
        public List<Sound_gener> sound_gener { get; set; }
    }
}