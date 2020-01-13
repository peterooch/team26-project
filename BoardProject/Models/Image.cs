using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BoardProject.Models
{
    public class Image
    {
        public int ID { get; set; }  // Identifier
        [Display(Name = "Source URL")]
        public string Source { get; set; }      // Image file URL location
        [Display(Name = "Image Name")]
        public string ImageName { get; set; }   // Image display name
        public string Category { get; set; }    // Image category
        public int ReferenceCount { get; set; } // How many times the image is being used (Popularity)
    }
}
