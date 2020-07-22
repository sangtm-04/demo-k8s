using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.WebApp.Entities
{
    public class AjaxResult
    {
        public int Code { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
