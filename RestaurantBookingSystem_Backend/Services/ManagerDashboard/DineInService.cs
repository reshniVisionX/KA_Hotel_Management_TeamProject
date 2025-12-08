using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Utils;
namespace RestaurantBookingSystem.Services
{
    public class DineInService : IDineInService
    {
        private readonly IDineInRepository _repo;
        private readonly IRestaurantService _restaurantService;

        public DineInService(IDineInRepository repo, IRestaurantService restaurantService)
        {
            _repo = repo;
            _restaurantService = restaurantService;
        }

        public async Task<IEnumerable<DineInDto>> GetByRestaurantIdAsync(int restaurantId)
        {
            var tables = await _repo.GetByRestaurantIdAsync(restaurantId);
            return tables.Select(MapToDto);
        }

        public async Task<DineInDto?> GetByIdAsync(int id)
        {
            var table = await _repo.GetByIdAsync(id);
            return table == null ? null : MapToDto(table);
        }

        public async Task<DineInDto> AddAsync(CreateDineInDto dto)
        {
            // Validate restaurant exists
            var restaurant = await _restaurantService.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
                throw new AppException($"Restaurant with ID {dto.RestaurantId} does not exist");

            try
            {
                var entity = new DineIn
                {
                    // TableId is auto-generated, don't set it
                    RestaurantId = dto.RestaurantId,
                    TableNo = dto.TableNo,
                    Capacity = dto.Capacity,
                    Status = TableStatus.Available
                };

                var created = await _repo.AddAsync(entity);
                return MapToDto(created);
            }
            catch (Exception ex)
            {
                throw new AppException("Failed to create table", ex);
            }
        }

        public async Task<DineInDto?> UpdateAsync(int id, UpdateDineInDto dto)
        {
            try
            {
                var existing = await _repo.GetByIdAsync(id);
                if (existing == null) return null;

                // Only update fields that are provided (not null)
                if (!string.IsNullOrEmpty(dto.TableNo))
                    existing.TableNo = dto.TableNo;
                
                if (dto.Capacity.HasValue)
                    existing.Capacity = dto.Capacity.Value;
                
                if (dto.Status.HasValue)
                    existing.Status = dto.Status.Value;

                await _repo.UpdateAsync(existing);
                return MapToDto(existing);
            }
            catch (Exception ex)
            {
                throw new AppException("Failed to update table", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        private static DineInDto MapToDto(DineIn table)
        {
            return new DineInDto
            {
                TableId = table.TableId,
                RestaurantId = table.RestaurantId,
                TableNo = table.TableNo,
                Capacity = table.Capacity,
                Status = table.Status
            };
        }
    }
}