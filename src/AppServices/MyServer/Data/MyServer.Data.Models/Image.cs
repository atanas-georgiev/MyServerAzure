namespace MyServer.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MyServer.Data.Common.Models;

    public class Image : BaseModel
    {
        public Image(Guid album)
            : base(album.ToString(), Guid.NewGuid().ToString())
        {
            this.AlbumId = album;
        }

        public Image(Guid album, Guid id)
            : base(album.ToString(), id.ToString())
        {
            this.AlbumId = album;
        }

        public Guid AlbumId { get; set; }

        [MaxLength(50)]
        public string Aperture { get; set; }

        [MaxLength(50)]
        public string CameraMaker { get; set; }

        [MaxLength(50)]
        public string CameraModel { get; set; }

        public DateTime? DateTaken { get; set; }

        [MaxLength(50)]
        public string ExposureBiasStep { get; set; }

        [Required]
        [MaxLength(200)]
        public string FileName { get; set; }

        [MaxLength(50)]
        public string FocusLen { get; set; }

        public int Height { get; set; }

        [MaxLength(50)]
        public string Iso { get; set; }

        public int LowHeight { get; set; }

        public int LowWidth { get; set; }

        public int MidHeight { get; set; }

        public int MidWidth { get; set; }

        [Required]
        [MaxLength(200)]
        public string OriginalFileName { get; set; }

        [MaxLength(50)]
        public string ShutterSpeed { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        public int Width { get; set; }
    }
}