using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Music2.Models
{
    public class EditSoundGenreModel
    {
        [Required]
        public Sound sound { set; get; }
        [Required]
        public List<Genre> genre { set; get; }
    }
}