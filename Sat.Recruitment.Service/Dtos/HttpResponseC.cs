using System;
using System.Collections.Generic;
using System.Text;

namespace Sat.Recruitment.Service.Dtos
{
    public class HttpResponseC
    { 
            public string type { get; set; }
            public string title { get; set; }
            public int status { get; set; }
            public string traceId { get; set; }
         
    }
}
