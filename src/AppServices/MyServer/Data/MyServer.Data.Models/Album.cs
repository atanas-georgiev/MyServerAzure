namespace MyServer.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MyServer.Data.Common.Models;

    public class Album : BaseModel
    {
        public Album(string id)
            : base("album", id)
        {
        }

        public Album()
            : base("album", Guid.NewGuid().ToString())
        {
        }

        public IEnumerable<Image> Images { get; set; }

        public int Access { get; set; }

        public string CoverId { get; set; }

        [MaxLength(3000)]
        public string DescriptionBg { get; set; }

        [MaxLength(3000)]
        public string DescriptionEn { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        public string TitleBg { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        public string TitleEn { get; set; }
    }
}