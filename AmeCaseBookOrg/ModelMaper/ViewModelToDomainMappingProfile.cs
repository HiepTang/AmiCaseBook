using AmeCaseBookOrg.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmeCaseBookOrg.ModelMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<UserViewModel, ApplicationUser>().ForMember(u => u.UserName, map => map.MapFrom(vm => vm.Email)).ForSourceMember(s => s.UploadImage, y => y.Ignore()).ForMember(m => m.UploadImage, n => n.Ignore());
            Mapper.CreateMap<UserCreateViewModel, ApplicationUser>().ForMember(u => u.UserName, map => map.MapFrom(vm => vm.Email));
            Mapper.CreateMap<CategoryViewModel, Category>();
            Mapper.CreateMap<AnnouncementViewModel, Announcement>().ForSourceMember(s => s.AttachmentFiles, y => y.Ignore()).ForMember( m => m.AttachmentFiles, n => n.Ignore());
            Mapper.CreateMap<CommunityTopicViewModel, CommunityTopic> ().ForSourceMember(s => s.AttachmentFiles, y => y.Ignore()).ForMember(m => m.AttachmentFiles, n => n.Ignore());
            Mapper.CreateMap<DataItemViewModel, DataItem>().ForSourceMember(s => s.Images, y => y.Ignore()).ForMember(m => m.Images, n => n.Ignore())
                .ForSourceMember(s => s.AttachFiles, y => y.Ignore()).ForMember(m => m.AttachFiles, n => n.Ignore())
                .ForSourceMember(s => s.MainMenu, y => y.Ignore()).ForMember(m => m.MainMenu, n => n.Ignore());
        }
    }
}