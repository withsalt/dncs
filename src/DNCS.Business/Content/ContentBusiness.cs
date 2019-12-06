using DNCS.Business.Mapping.Content;
using DNCS.Data.Content;
using DNCS.Data.Entity;
using DNCS.Data.Entity.Content;
using DNCS.Data.Entity.System;
using DNCS.Data.Model.Request.Content;
using DNCS.Data.Model.Response.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WithSalt.Common.Date;
using WithSalt.Common.Text;

namespace DNCS.Business.Content
{
    public class ContentBusiness
    {
        public async Task<(int, ContentInfoModel)> SelectRandomContentInfoItem(CustumDbContext dbContext, int type)
        {
            ContentDataManager data = new ContentDataManager(dbContext);
            ContentInfo info = await data.SelectRandomContentInfoItem(type);
            if (info == null)
            {
                return (50002, null);
            }
            ContentInfoModel result = ContentInfoEntityMapModel.Map(info);
            if (result == null)
            {
                return (50003, null);
            }
            return (0, result);
        }
    }
}
