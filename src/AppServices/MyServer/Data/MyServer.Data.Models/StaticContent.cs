namespace MyServer.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using MyServer.Data.Common.Models;

    public class StaticContent : BaseModel
    {
        public StaticContent(string contentKey)
            : base("staticcontent", contentKey)
        {
            this.ContentKey = contentKey;
        }

        [Required]
        [MaxLength(50)]
        public string ContentKey { get; set; }

        [Required]
        [MaxLength(30000)]
        public string ContentValueBg { get; set; }

        [Required]
        [MaxLength(30000)]
        public string ContentValueEn { get; set; }
    }
}