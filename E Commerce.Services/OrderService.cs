using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entites.OrderModule;
using E_Commerce.Domain.Entites.Product_Module;
using E_Commerce.Service.Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IBasketRepository basketRepository;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.basketRepository = basketRepository;
        }

        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email)
        {
            var orderAddress = mapper.Map<OrderAddress>(orderDTO.Address);

            var Basket = await basketRepository.GetBasketAsync(orderDTO.BasketId);

            if (Basket == null)
            {
                return Error.NotFound("Basket not found",$"Basket with Id{orderDTO.BasketId}is not found ");
            }
            List<OrderItem> OrderItems=new List<OrderItem>();

            foreach (var item in Basket.Item)
            {
                var Product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (Product == null)
                {
                    return Error.NotFound("Product not found", $"Product with Id {item.Id} is not found");
                }
              OrderItems.Add(CreateOrderItem(item, Product));
            }
            var DeliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DelivaryMethodId);
            if (DeliveryMethod == null)return Error.NotFound("Delivery method not found", $"Delivery method with Id {orderDTO.DelivaryMethodId} is not found");

            var subtotal = OrderItems.Sum(item => item.Price * item.Quantity);

            var Order = new Order()
            {
                UserEmail = Email,
                Items = OrderItems,
                Address = orderAddress,
                DeliveryMethod = DeliveryMethod,
                Subtotal = subtotal
            };
           await unitOfWork.GetRepository<Order, int>().AddAsync(Order);
            var result= await unitOfWork.SaveChangeAsync()>0;
            if (!result) return Error.Failure("Order.Failure", "order can not be craeted");

            return mapper.Map<OrderToReturnDTO>(Order);







        }

        private static OrderItem CreateOrderItem(Domain.Entites.Basket_Module.BasketItem item, Product Product)
        {
            return new OrderItem()
            {

                productItemOrdered = new ProductItemOrdered() { ProductID = Product.Id, ProductName = Product.Name, PictureUrl = Product.PicturUrl },
                Price = Product.Price,
                Quantity = item.Quantity

            };
        }
    }
}
