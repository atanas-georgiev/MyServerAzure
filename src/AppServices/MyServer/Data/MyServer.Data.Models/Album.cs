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

        public int ImagesCount { get; set; }

        public DateTime? FirstTmageTaken { get; set; }

        public DateTime? LastTmageTaken { get; set; }

        public int Access { get; set; }

        public string CoverAlbumId { get; set; }

        public string CoverAlbumFilename { get; set; }

        public int CoverLowWidth { get; set; }

        public int CoverLowHeight { get; set; }

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