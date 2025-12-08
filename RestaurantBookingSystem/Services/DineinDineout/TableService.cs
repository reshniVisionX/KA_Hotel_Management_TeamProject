using AutoMapper;
using RestaurantBookingSystem.DTOs;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Model.Restaurant;

namespace RestaurantBookingSystem.Services
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _repository;
        private readonly IMapper _mapper;

        public TableService(ITableRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TableResponseDto>> GetAvailableTablesAsync(int restaurantId)
        {
            var tables = await _repository.GetAvailableTablesAsync(restaurantId);
            return _mapper.Map<List<TableResponseDto>>(tables);
        }
    }
}