using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Music2.Models
{
    public class PageModel
    {
        [Required]
        public List<Sound> my_Sound { get; set; }

        [Required]
        public List<Genre> my_Genre { get; set; }

    }
}