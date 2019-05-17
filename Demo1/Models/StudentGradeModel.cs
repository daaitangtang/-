using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo1.Models
{
    public partial class StudentGradeModel
    {
        public User user { get; set; }

        public TestInfo testInfo { get;set; }

        public Student_grade studentGrade { get; set; }
    }

    public partial class StudentTestModel
    {
        public int userid { get; set; }
        public int testid { get; set; }
    }
}