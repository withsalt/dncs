using System;
using System.Collections.Generic;
using System.Text;

namespace DNCS.Config
{
    public class AppSettingsNode
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsDebug { get; set; }

        public string PasswdSalt { get; set; }

        public string WebTokenPassword { get; set; }

        public int WebTokenSaveTime { get; set; }

    }
}
