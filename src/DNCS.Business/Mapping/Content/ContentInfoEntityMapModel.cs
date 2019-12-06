using AutoMapper;
using DNCS.Data.Entity.Content;
using DNCS.Data.Model.Response.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace DNCS.Business.Mapping.Content
{
    public class ContentInfoEntityMapModel
    {
        public static ContentInfoModel Map(ContentInfo info)
        {
            if (info == null)
            {
                return null;
            }
            MapperConfiguration config = null;
            try
            {
                config = new MapperConfiguration(m => m.CreateMap<ContentInfo, ContentInfoModel>()
                    .ForMember(dest => dest.TypeName, opts => opts.MapFrom(src => src.ContentType.Name))
                    .ForMember(dest => dest.CreateUserName, opts => opts.MapFrom(src => src.UserInfo.UserName))
                    );
                var mapper = config.CreateMapper();
                return mapper.Map<ContentInfoModel>(info);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<ContentInfoModel> Map(List<ContentInfo> infos)
        {
            List<ContentInfoModel> models = new List<ContentInfoModel>();
            if (infos.Count == 0)
            {
                return models;
            }
            foreach (var item in infos)
            {
                models.Add(Map(item));
            }
            return models;
        }
    }
}
