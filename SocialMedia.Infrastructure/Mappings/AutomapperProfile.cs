using AutoMapper;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.Mappings
{
    /**
     * In this class we will register all the mappings of the dtos.
     */
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            // Map from Post to PostDto
            CreateMap<Post, PostDto>();
            // Map from PostDto to Post
            CreateMap<PostDto, Post>();

            
            // CreateMap<SecurityDto, Security>();
            // (18) Better do the following to save time. The following also registers the reverse mapping.
            CreateMap<Security, SecurityDto>().ReverseMap();
        }
    }
}
