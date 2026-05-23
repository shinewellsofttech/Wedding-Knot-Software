using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sahakaar_API.Models
{
    public class Response
    {
        public string Time { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
