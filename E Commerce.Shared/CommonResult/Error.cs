using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.CommonResult
{
    public  class Error
    {
        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            ErrorType = type;
        }

        public string Code { get; }

        public string Description { get; }

        public ErrorType ErrorType { get; }


        public static Error Failure(string code = "General.Failure", string description = "General.Failure Has Occured")
        {
            return new Error(code, description, ErrorType.Failure);
        }

        public static Error Validation(string code = "General.Validation", string description = "General.Validation Has Occured")
        {
            return new Error(code, description, ErrorType.Validation);
        }

        public static Error NotFound(string code = "General.NotFound", string description = "General.NotFound Has Occured")
        {
            return new Error(code, description, ErrorType.NotFound);
        }

        public static Error Unauthrized(string code = "General.Unauthrized", string description = "General.Unauthrized Has Occured")
        {
            return new Error(code, description, ErrorType.Unauthrized);
        }

        public static Error forbidden(string code = "General.forbidden", string description = "General.forbidden Has Occured")
        {
            return new Error(code, description, ErrorType.forbidden);
        }

        public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "General.InvalidCredentials Has Occured")
        {
            return new Error(code, description, ErrorType.InvalidCredentials);
        }







    }
}
