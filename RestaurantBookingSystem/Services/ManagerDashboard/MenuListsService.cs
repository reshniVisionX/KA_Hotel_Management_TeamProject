using RestaurantBookingSystem.Dtos;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Model.Restaurant;
using RestaurantBookingSystem.Utils;
namespace RestaurantBookingSystem.Services
{
    public class MenuListsService : IMenuListService
    {
        private readonly IMenuListRepository _repo;
        private readonly IRestaurantService _restaurantService;

        public MenuListsService(IMenuListRepository repo, IRestaurantService restaurantService)
        {
            _repo = repo;
            _restaurantService = restaurantService;
        }

        public async Task<IEnumerable<MenuListsDto>> GetByRestaurantIdAsync(int restaurantId)
        {
            var items = await _repo.GetByRestaurantIdAsync(restaurantId);
            return items.Select(MapToDto);
        }

        public async Task<MenuListsDto?> GetByIdAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<MenuListsDto> AddAsync(CreateMenuListsDto dto, byte[]? imageBytes = null)
        {
            // Validate restaurant exists using service layer
            var restaurant = await _restaurantService.GetByIdAsync(dto.RestaurantId);
            if (restaurant == null)
                throw new AppException($"Restaurant with ID {dto.RestaurantId} does not exist");

            try
            {
                var entity = new MenuList
                {
                    // ItemId is auto-generated, don't set it
                    RestaurantId = dto.RestaurantId,
                    ItemName = dto.ItemName,
                    Description = dto.Description,
                    AvailableQty = dto.AvailableQty,
                    Discount = dto.Discount,
                    Price = dto.Price,
                    IsVegetarian = dto.IsVegetarian,
                    Tax = dto.Tax,
                    Image = imageBytes
                };

                var created = await _repo.AddAsync(entity);
                return MapToDto(created);
            }
            catch (Exception ex)
            {
                throw new AppException("Failed to create menu item", ex);
            }
        }

        public async Task<MenuListsDto?> UpdateAsync(int id, UpdateMenuListsDto dto, byte[]? imageBytes = null)
        {
            try
            {
                var existing = await _repo.GetByIdAsync(id);
                if (existing == null) return null;

                // Only update fields that are provided (not null)
                if (!string.IsNullOrEmpty(dto.ItemName))
                    existing.ItemName = dto.ItemName;
                
                if (!string.IsNullOrEmpty(dto.Description))
                    existing.Description = dto.Description;
                
                if (dto.AvailableQty.HasValue)
                    existing.AvailableQty = dto.AvailableQty.Value;
                
                if (dto.Discount.HasValue)
                    existing.Discount = dto.Discount.Value;
                
                if (dto.Price.HasValue)
                    existing.Price = dto.Price.Value;
                
                if (dto.IsVegetarian.HasValue)
                    existing.IsVegetarian = dto.IsVegetarian.Value;
                
                if (dto.Tax.HasValue)
                    existing.Tax = dto.Tax.Value;
                
                // Handle image from parameter (takes priority over DTO)
                if (imageBytes != null)
                    existing.Image = imageBytes;
                else if (dto.Image != null)
                    existing.Image = dto.Image;

                await _repo.UpdateAsync(existing);
                return MapToDto(existing);
            }
            catch (Exception ex)
            {
                throw new AppException("Failed to update menu item", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        private static MenuListsDto MapToDto(MenuList item)
        {
            return new MenuListsDto
            {
                ItemId = item.ItemId,
                RestaurantId = item.RestaurantId,
                ItemName = item.ItemName,
                Description = item.Description,
                AvailableQty = item.AvailableQty,
                Discount = item.Discount,
                Price = item.Price,
                IsVegetarian = item.IsVegetarian,
                Tax = item.Tax,
                Image = item.Image
            };
        }
    }
}