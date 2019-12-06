using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WithSalt.Common.Http
{
    public class RequestHelper
    {
        public static string GetRequestIP(IHttpContextAccessor accessor, bool tryUseXForwardHeader = true)
        {
            try
            {
                string ip = null;
                if (tryUseXForwardHeader)
                {
                    if (accessor.HttpContext?.Request?.Headers?.TryGetValue("X-Forwarded-For", out StringValues values) ?? false)
                    {
                        string rawValues = values.ToString();
                        if (!string.IsNullOrEmpty(rawValues))
                        {
                            ip = values.ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(ip))
                    {
                        List<string> result = ip.TrimEnd(',')
                            .Split(',')
                            .AsEnumerable<string>()
                            .Select(s => s.Trim())
                            .ToList();
                        if (result != null && result.Count > 0)
                        {
                            ip = result.FirstOrDefault();
                        }
                    }
                }
                if (string.IsNullOrEmpty(ip) && accessor.HttpContext?.Connection?.RemoteIpAddress != null)
                {
                    ip = accessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
                }
                if (string.IsNullOrEmpty(ip))
                {
                    if (accessor.HttpContext?.Request?.Headers?.TryGetValue("REMOTE_ADDR", out StringValues values) ?? false)
                    {
                        string rawValues = values.ToString();
                        if (!string.IsNullOrEmpty(rawValues))
                        {
                            ip = values.ToString();
                        }
                    }
                }
                if (string.IsNullOrEmpty(ip))
                {
                    ip = "127.0.0.1";
                }
                //检测是否为本地IP
                List<string> localIp = new List<string>()
                {
                    "0.0.0.1","127.0.0.1"
                };
                if (localIp.Contains(ip))
                {
                    ip = "127.0.0.1";
                }
                return ip;
            }
            catch (Exception ex)
            {
                return "127.0.0.1";
            }
        }
    }
}
