using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo1.Models
{
    /// <summary>
    /// 在线考试页面所用Model
    /// </summary>
    public class OnlineTestModel
    {
        public TestInfo TestInfo { get; set; }

        public IEnumerable<Question> Questions { get; set; }
    }
}