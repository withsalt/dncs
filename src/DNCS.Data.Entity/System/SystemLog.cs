using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DNCS.Data.Entity.System
{
    [Table("Sys_Logs")]
    public class SystemLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        public string LogType { get; set; }

        [MaxLength(300)]
        public string Describe { get; set; }

        [MaxLength(300)]
        public string Localtion { get; set; }

        public string Stack { get; set; }

        public int CreateTime { get; set; }
    }
}
