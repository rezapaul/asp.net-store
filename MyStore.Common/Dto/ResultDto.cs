using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Common.Dto
{
    public class ResultDto
    {
        public string Message { get; set; }
        public bool IsSucceed { get; set; }
    }

    public class ResultDto<T>
    {
        public T Data {get;set;}
        public string Message { get; set; }
        public bool IsSucceed { get; set; }
    }
}
