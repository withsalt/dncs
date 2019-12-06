using DNCS.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DNCS.Data.Interface
{
    public abstract class BaseDataManager : IDataManager
    {
        protected CustumDbContext _dbContext;

        protected BaseDataManager(CustumDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
