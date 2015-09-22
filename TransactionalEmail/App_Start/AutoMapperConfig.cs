using AutoMapper;
using TransactionalEmail.Core.Objects;

namespace TransactionalEmail
{
    public class AutoMapperConfig
    {
        public static void Bootstrap()
        {
            Mapper.CreateMap<Attachment, Models.Attachment>();

            Mapper.CreateMap<Models.Attachment, Attachment>()
                .ForMember(dst => dst.AttachmentId, opt => opt.Ignore());
        }
    }
}