using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Fang
{
    public class PageUrl
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(500)]
        public string Url { get; set; }
        public bool HasGet { get; set; }
        public bool IsPersonPost { get; set; }
        [MaxLength(1000)]
        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 该帖被屏蔽
        /// </summary>
        public bool IsBlock { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [MaxLength(50)]
        public string Author { get; set; }
    }
}
