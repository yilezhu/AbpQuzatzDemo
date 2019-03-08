using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czar.AbpDemo.Result
{
    public class BaseResult
    {
        public int Code { get; set; }=0;

        public string Message { get; set; } = "操作成功";

    }
}
