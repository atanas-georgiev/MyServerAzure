namespace MyServer.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MyServer.Data.Common.Models;

    public class Album : BaseModel
    {
        public Album(Guid id)
            : base("album", id.ToString())
        {
        }

        public Album()
            : base("album", Guid.NewGuid().ToString())
        {
        }

        public int Access { get; set; }

        public Guid? CoverId { get; set; }

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