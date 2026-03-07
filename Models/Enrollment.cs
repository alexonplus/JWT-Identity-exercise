using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Models
{

    public enum EnrollmentStatus
    {
        Pending,
        Approved
    }


    public class Enrollment
    {
        public int Id {get; set;}
        public string StudentId {get; set;} = string.Empty;
        public int CourseId { get; set; } = 0;

        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Pending;
    }
}
