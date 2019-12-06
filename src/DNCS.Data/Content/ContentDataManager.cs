using DNCS.Data.Entity;
using DNCS.Data.Entity.Content;
using DNCS.Data.Entity.System;
using DNCS.Data.Interface;
using DNCS.Data.Model.Request.Content;
using DNCS.Data.Provider.NpgsqlProvider;
using DNCS.LogInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WithSalt.Common.Date;
using DNCS.Data.Entity.Extensions;

namespace DNCS.Data.Content
{
    public class ContentDataManager : BaseDataManager
    {
        public ContentDataManager(CustumDbContext context) : base(context)
        {

        }

        public async Task<ContentInfo> SelectRandomContentInfoItem(int type)
        {
            try
            {
                ContentInfo info = await _dbContext.ContentInfo
                        .Include(c => c.ContentType)
                        .Include(c => c.UserInfo)
                        .Where(c => c.TypeId == type && c.IsDelete == false)
                        .OrderBy(x => Guid.NewGuid())
                        .Take(1)
                        .FirstOrDefaultAsync();
                //记录读取次数
                info.ReadTime++;
                await _dbContext.SaveChangesAsync();
                return info;
            }
            catch (Exception ex)
            {
                Log.Error($"Get content info failed. {ex.Message}", ex);
                return null;
            }
        }
    }
}
