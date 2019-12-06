using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DNCS.Data.Entity;
using DNCS.Business.Content;
using DNCS.Data.Model.Common.ResultInfo;
using DNCS.Data.Model.Common.JsonObjectNode;
using DNCS.LogInfo;
using DNCS.Config;
using DNCS.Data.Model.Response.Content;

namespace DNCS.Web.Main.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CustumDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, CustumDbContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Scan()
        {
            return View();
        }

        /// <summary>
        /// 随机获取一条
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DoChange(int type = 1)
        {
            var result = await new ContentBusiness().SelectRandomContentInfoItem(_dbContext, type);
            if (result.Item1 != 0)
            {
                return Json(CreateContentInfoResultStatus(result.Item1, "Get content info failed"));
            }
            else
            {
                return Json(CreateContentInfoResultStatus(0, "Success", result.Item2));
            }
        }

        private ResultModel<IChild> CreateContentInfoResultStatus(int code, string message, IChild data = null)
        {
            ResultModel<IChild> status = new ResultModel<IChild>()
            {
                Code = code,
                Message = message,
                Data = data
            };
            return status;
        }
    }
}
