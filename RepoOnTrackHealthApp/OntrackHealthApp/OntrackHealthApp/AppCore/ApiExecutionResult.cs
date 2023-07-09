using System;
using System.Collections.Generic;
using System.Text;

namespace OntrackHealthApp.AppCore
{
    public class ApiExecutionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ApiExecutionResult<T> where T : class
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<T> DataList { get; set; }
    }
}
