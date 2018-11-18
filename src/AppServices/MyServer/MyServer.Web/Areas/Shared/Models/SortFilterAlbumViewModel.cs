namespace MyServer.Web.Areas.Shared.Models
{
    using System.ComponentModel.DataAnnotations;

    using MyServer.Common;

    using Resources;

    public class SortFilterAlbumViewModel
    {
        [UIHint("EnumSort")]
        [Display(Name = "Sort", ResourceType = typeof(Helpers_SharedResource))]
        public MyServerSortType SortType { get; set; }

        [Display(Name = "Search", ResourceType = typeof(Helpers_SharedResource))]
        public string SearchString { get; set; }        
    }
}