using AmeCaseBookOrg.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmeCaseBookOrg.ModelMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ApplicationUser, UserViewModel>().ForMember(u => u.Email, map => map.MapFrom(vm => vm.UserName)).ForMember(u => u.UploadImage , map => map.MapFrom(vm => vm.UploadImage));
            Mapper.CreateMap<Category, CategoryViewModel>();
            Mapper.CreateMap<Announcement, AnnouncementViewModel>();
            Mapper.CreateMap<CommunityTopic, CommunityTopicViewModel>();
            Mapper.CreateMap<DataItem, DataItemViewModel>();
        }
    }
}