using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.Interface;
using RestaurantBookingSystem.Interface.IRepository;
using RestaurantBookingSystem.Interface.IService;
using RestaurantBookingSystem.Interfaces.Admin;
using RestaurantBookingSystem.Interfaces.IRepository;
using RestaurantBookingSystem.Interfaces.IService;
using RestaurantBookingSystem.Mappings;
using RestaurantBookingSystem.Middlewares;
using RestaurantBookingSystem.Repository;
using RestaurantBookingSystem.Repository.Admin;
using RestaurantBookingSystem.Services;
using RestaurantBookingSystem.Services.Admin;
using RestaurantBookingSystem.Utils;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });

builder.Services.AddDbContext<BookingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConn")));

builder.Services.AddScoped<IUsers, UserRepository>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminServices>();
builder.Services.AddScoped<IManagerRequestService, AdminManagerService>();
builder.Services.AddScoped<IManagerRequestRepository, AdminManagerRepository>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddTransient<EmailToManager>();
builder.Services.AddScoped<SendEmail>();


//dashboard -start
//repository
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
builder.Services.AddScoped<IWishlistsRepository, WishlistsRepository>();
builder.Services.AddScoped<RestaurantBookingSystem.Interface.IRepository.IReviewsRepository, DashboardReviewsRepository>();
builder.Services.AddScoped<IOrderHistoryRepository, OrderHistoryRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IRestaurantsRepository, Restaurants_Repository>();
builder.Services.AddScoped<ITableBookingRepository, TableBookingRepository>();
builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();

builder.Services.AddScoped<IAdminDeliveryService, AdminDeliveryService>();
builder.Services.AddScoped<IDeliveryRespository, AdminDeliveryRepository>();
//services
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IUserPreferenceService, UserPreferenceService>();
builder.Services.AddScoped<IWishlistsService, WishlistsService>();
builder.Services.AddScoped<RestaurantBookingSystem.Interface.IService.IReviewsService, DashboardReviewsService>();
builder.Services.AddScoped<IOrderHistoryService, OrderHistoryService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<ITableBookingService, TableBookingService>();
builder.Services.AddScoped<IUserAddressService, UserAddressService>();
//dashboard -end


builder.Services.AddScoped<IRestaurantRepository, ManagerRestaurantRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IMenuListRepository, MenuListsRepository>();
builder.Services.AddScoped<IManagerDashboardRepository, ManagerDashboardRepository>();
builder.Services.AddScoped<IDineInRepository, DineInRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentsRepository>();
builder.Services.AddScoped<RestaurantBookingSystem.Interface.IReviewsRepository, ManagerReviewsRepository>();

builder.Services.AddScoped<IRestaurantService, RestaurantsService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMenuListService, MenuListsService>();
// Removed ManagerDashboard - using simple order endpoints instead
builder.Services.AddScoped<IDineInService, DineInService>();
builder.Services.AddScoped<IPaymentService, PaymentsService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IReservationsRepository, ReservationsRepository>();
builder.Services.AddScoped<IReservationsService, ReservationsService>();



//DineDineout-------------------------------------------------------------------------------------------------------------
// Add AutoMapper
builder.Services.AddAutoMapper(typeof(CartMappingProfile));

// Register Repositories
builder.Services.AddScoped<IRestaurant, RestaurantRepository>();
builder.Services.AddScoped<IMenuList, MenuListRepository>();
builder.Services.AddScoped<IOrders, OrdersRepository>();
builder.Services.AddScoped<IPayment, PaymentRepository>();
builder.Services.AddScoped<IDelivery, DeliveryRepository>();
builder.Services.AddScoped<IDeliveryPerson, DeliveryPersonRepository>();
builder.Services.AddScoped<IDeliveryAddress, DeliveryAddressRepository>();

// Register Services
builder.Services.AddScoped<RestaurantService>();
builder.Services.AddScoped<MenuListService>();
builder.Services.AddScoped<OrdersService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<DeliveryService>();
builder.Services.AddScoped<DeliveryPersonService>();
builder.Services.AddScoped<DeliveryAddressService>();

// Register repositories and services
builder.Services.AddScoped<RestaurantBookingSystem.Interfaces.IWishlistRepository, RestaurantBookingSystem.Repository.WishlistRepository>();
builder.Services.AddScoped<RestaurantBookingSystem.Interfaces.IWishlistService, RestaurantBookingSystem.Services.WishlistService>();
builder.Services.AddScoped<RestaurantBookingSystem.Interfaces.ICartRepository, RestaurantBookingSystem.Repository.CartRepository>();
builder.Services.AddScoped<RestaurantBookingSystem.Interfaces.ICartService, RestaurantBookingSystem.Services.CartService>();
builder.Services.AddScoped<RestaurantBookingSystem.Interfaces.ITableRepository, RestaurantBookingSystem.Repository.TableRepository>();
builder.Services.AddScoped<RestaurantBookingSystem.Interfaces.ITableService, RestaurantBookingSystem.Services.TableService>();
builder.Services.AddScoped<RestaurantBookingSystem.Interfaces.IReservationService, RestaurantBookingSystem.Services.ReservationService>();
builder.Services.AddScoped<RestaurantBookingSystem.Interfaces.IReservationRepository, RestaurantBookingSystem.Repository.ReservationRepository>();
builder.Services.AddScoped<RestaurantBookingSystem.Interfaces.IReviewRepository, RestaurantBookingSystem.Repository.ReviewRepository>();
builder.Services.AddScoped<RestaurantBookingSystem.Interfaces.IReviewService, RestaurantBookingSystem.Services.ReviewService>();
//Dineoute-------------------------------------------------------------------------------------------------------------


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new

    SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]!)),
        ValidateIssuer = false,
        ValidateAudience = false

    };
});


builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
{
new OpenApiSecurityScheme
{
Reference = new OpenApiReference
{
Type = ReferenceType.SecurityScheme,
Id = "Bearer"
}
},
Array.Empty<string>()
}
});
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // React app URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // optional if using cookies
        });
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFile("Logs/restaurant-{Date}.txt");
var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
