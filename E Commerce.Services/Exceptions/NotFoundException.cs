using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Exceptions
{
    public class NotFoundException(string Message):Exception(Message)
    {
    }
    public sealed class ProductNotFoundException(int Id)
        : NotFoundException($"Product with Id {Id} was not found.")
    {
    }

    public sealed class BrandNotFoundException(string Id)
        : NotFoundException($"Brand with Id {Id} was not found.")
    {
    }

}
