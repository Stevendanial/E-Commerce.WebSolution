using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce.Shared.CommonResult
{
    public class Result
    {
        private readonly List<Error> _errors = [];
        public bool IsSuccess => _errors.Count() == 0;

        public bool IsFailure =>!IsSuccess;

        public IReadOnlyList<Error> Errors => _errors;


        //ok=success

        protected Result() { }

        // fail with Error

        protected Result(Error errors)
        {
            _errors.Add(errors);
        }

        // fail with Errorsss
        protected Result(List<Error> errors)
        {
            _errors.AddRange( errors);
        }


        //ok=success
        public static Result Ok()=>new Result();
        
        // fail with Error
        public static Result Fail(Error error)=>new Result(error);


        // fail with Errorsss
        public static Result Fail(List<Error> errors)=>new Result(errors);
    }
    public class Result<Tvalue> : Result 
    {
        private readonly Tvalue _value;

        public Tvalue value => IsSuccess ? _value : throw new InvalidOperationException("Can not access the value ");


        //ok 
        private Result(Tvalue value):base()
        {
            _value = value;
        }

        // fail with Error

        private Result(Error errors) : base()
        {
            _value = default!;
        }

        // fail with Errorsss
        private Result(List<Error> errors) : base()
        {
            _value =default!;
        }


        //ok=success
        public static Result<Tvalue> Ok(Tvalue value) => new Result<Tvalue>(value);

        // fail with Error
        public static new Result<Tvalue> Fail(Error error) => new Result<Tvalue>(error);


        // fail with Errorsss
        public static new Result<Tvalue> Fail(List<Error> errors) => new (errors);

        public static implicit operator Result<Tvalue>(Tvalue value) => Ok(value);

        public static implicit operator Result<Tvalue>(Error error) => Fail(error);

        public static implicit operator Result<Tvalue>(List<Error> error) => Fail(error);




    } 


}
