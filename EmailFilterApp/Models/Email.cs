using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EmailFilterApp.Models
{
    public class Email
    {
        public string From { get; set; }
        public string ReadStatus { get; set; }
        public string Body { get; set; }
        public string ApplicantName { get; set; }
        public string ContactNo { get; set; }
        public HttpPostedFileBase AttachedFile { get; set; }
        public DateTime DateTime { get; set; }
    }
}
