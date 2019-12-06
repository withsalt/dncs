using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using WithSalt.Common.Text.IdentifyEncoding;

namespace WithSalt.Common.Text
{
    public class TextFileEncoding
    {
        public static Encoding GetFileEncoding(FileStream stream)
        {
            return GetEncoding(stream);
        }

        public static Encoding GetFileEncoding(string path)
        {
            Encoding encoding;
            if (!File.Exists(path))
            {
                throw new NullReferenceException("The file is not exist.");
            }
            //获取编码
            using (var fs = File.OpenRead(path))
            {
                //获取编码
                encoding = GetEncoding(fs);
            }
            return encoding;
        }

        public static Encoding GetWebPageEncoding(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            using (var stream = request.GetResponse().GetResponseStream())
            {
                //读取响应到内存流
                var ms = new MemoryStream();
                var length = 0;
                var buffer = new byte[1024];
                while ((length = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, length);
                }

                //获取编码
                var encoding = GetEncoding(ms);
                return encoding;
            }
        }


        /// <summary>
        /// 获取字节流编码
        /// </summary>
        /// <param name="stream">字节流</param>
        /// <returns></returns>
        private static Encoding GetEncoding(Stream stream)
        {
            if (stream != null && stream.Length > 0)
            {
                //每次分配1024字节，进行编码判断
                var buffer = new byte[1024];

                var seek = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);

                var ud = new UniversalDetector(null);
                while (!ud.IsDone() && stream.Read(buffer, 0, buffer.Length) > 0)
                {
                    ud.HandleData(buffer, 0, buffer.Length);
                }
                ud.DataEnd();

                stream.Seek(seek, SeekOrigin.Begin);

                var encoding = ud.GetDetectedCharset();
                if (encoding != null)
                {
                    if (encoding == Constants.CHARSET_X_ISO_10646_UCS_4_2143 || encoding == Constants.CHARSET_X_ISO_10646_UCS_4_3412)
                    {
                        encoding = "UTF-32";
                    }

                    return Encoding.GetEncoding(encoding);
                }
            }

            return Encoding.Default;
        }
    }
}
