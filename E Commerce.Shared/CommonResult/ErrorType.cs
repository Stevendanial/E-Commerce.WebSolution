using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.CommonResult
{
    public enum ErrorType
    {
        Failure=0,
        Validation=1,
        NotFound=2,
        Unauthrized=3,
        forbidden=4,
        InvalidCredentials=5
    }
}
