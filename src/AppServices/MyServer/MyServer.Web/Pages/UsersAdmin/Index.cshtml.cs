using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyServer.Common;
using MyServer.Data;
using MyServer.Data.Models;
using MyServer.Services.Mappings;
using MyServer.Services.Users;
using MyServer.Web.Areas.Shared.Models;
using MyServer.Web.Pages.Base;
using MyServer.Web.Resources;

namespace MyServer.Web.Pages.UsersAdmin
{
    public class IndexModel : BasePageModel
    {
        
        public void OnGet()
        {
        }

        public IndexModel(IUserService userService, IHttpContextAccessor httpContextAccessor) : base(userService, httpContextAccessor)
        {
        }
    }

    public class UsersViewModel : IMapFrom<User>, IHaveCustomMappings
    {
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(Helpers_SharedResource))
        ]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Helpers_SharedResource))]
        [UIHint("KendoTextBox")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(Helpers_SharedResource))
        ]
        [StringLength(50, ErrorMessageResourceName = "ErrorLength",
            ErrorMessageResourceType = typeof(Helpers_SharedResource), MinimumLength = 2)]
        [Display(Name = "Name", ResourceType = typeof(Helpers_SharedResource))]
        [UIHint("KendoTextBox")]
        public string DisplayName { get; set; }

        [Key]
        [HiddenInput(DisplayValue = false)]
        public string RowKey { get; set; }

        [UIHint("KendoDropDownRoles")]
        [Display(Name = "Role", ResourceType = typeof(Helpers_SharedResource))]
        public MyServerRoles Role { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<User, UsersViewModel>()
                .ForMember(m => m.Role, opt => opt.MapFrom(src => MappingFunctions.MapUserRole(src)));
        }
    }
}