using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entites.Basket_Module;
using E_Commerce.Service.Abstraction;
using E_Commerce.Shared.DTOs.BasketDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket)
        {
            var CustomerBasket = _mapper.Map<CustomerBasket>(basket);
            var CreatedOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(CustomerBasket);
            return _mapper.Map<BasketDTO>(CreatedOrUpdatedBasket);

        }

        public async Task<bool> DeleteBasketAsync(string id) => await _basketRepository.DeleteBasketAsync(id);


        public async Task<BasketDTO> GetBasketByIdAsync(string id)
        {
            var Basket = await _basketRepository.GetBasketAsync(id);
            return _mapper.Map<BasketDTO>(Basket);

        }
    }
}
