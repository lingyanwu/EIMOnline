using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Wysnan.EIMOnline.Common.Poco
{
    [Table("zMetaLookup")]
    public class Lookup : IBaseEntity
    {
        public int ID { get; set; }

        [ConcurrencyCheck]
        [Timestamp]
        public byte? SystemStatus { get; set; }

        public byte[] TimeStamp { get; set; }

        /// <summary>
        /// Lookup Name
        /// </summary>
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 分组标识
        /// </summary>
        [StringLength(20)]
        public string Code { get; set; }
    }
}
