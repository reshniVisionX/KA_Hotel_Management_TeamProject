using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class Restaurant_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryPerson",
                columns: table => new
                {
                    DeliveryPersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MobileNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LicenseNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    otp = table.Column<int>(type: "int", maxLength: 6, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalDeliveries = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPerson", x => x.DeliveryPersonId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Cart_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAddress",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Pincode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Landmark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddress", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_DeliveryAddress_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ManagerDetails",
                columns: table => new
                {
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerDetails", x => x.ManagerId);
                    table.ForeignKey(
                        name: "FK_ManagerDetails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    ItemsList = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    QtyOrdered = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    PreferenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NotificationsEnabled = table.Column<bool>(type: "bit", nullable: true),
                    PreferredCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PreferredFoodType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.PreferenceId);
                    table.ForeignKey(
                        name: "FK_UserPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wishlist",
                columns: table => new
                {
                    WishlistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlist", x => x.WishlistId);
                    table.ForeignKey(
                        name: "FK_Wishlist_Users_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    RestaurantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Images = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ratings = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: true),
                    RestaurantCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestaurantType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(505)", maxLength: 505, nullable: false),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ContactNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DeliveryCharge = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: true),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.RestaurantId);
                    table.ForeignKey(
                        name: "FK_Restaurants_ManagerDetails_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "ManagerDetails",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Delivery",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    DeliveryPersonId = table.Column<int>(type: "int", nullable: false),
                    DeliveryStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Delivery", x => x.DeliveryId);
                    table.ForeignKey(
                        name: "FK_Delivery_DeliveryAddress_AddressId",
                        column: x => x.AddressId,
                        principalTable: "DeliveryAddress",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Delivery_DeliveryPerson_DeliveryPersonId",
                        column: x => x.DeliveryPersonId,
                        principalTable: "DeliveryPerson",
                        principalColumn: "DeliveryPersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Delivery_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payment_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DineIn",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    TableNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestaurantsRestaurantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DineIn", x => x.TableId);
                    table.ForeignKey(
                        name: "FK_DineIn_Restaurants_RestaurantsRestaurantId",
                        column: x => x.RestaurantsRestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantId");
                });

            migrationBuilder.CreateTable(
                name: "ManagerPayments",
                columns: table => new
                {
                    ManagerPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerPayments", x => x.ManagerPaymentId);
                    table.ForeignKey(
                        name: "FK_ManagerPayments_ManagerDetails_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "ManagerDetails",
                        principalColumn: "ManagerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManagerPayments_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuList",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailableQty = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    IsVegetarian = table.Column<bool>(type: "bit", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuList", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_MenuList_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RestaurantId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<decimal>(type: "decimal(3,1)", precision: 3, scale: 1, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Review_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TableId = table.Column<int>(type: "int", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReservationInTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ReservationOutTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReseveredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_Reservation_DineIn_TableId",
                        column: x => x.TableId,
                        principalTable: "DineIn",
                        principalColumn: "TableId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservation_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DeliveryPerson",
                columns: new[] { "DeliveryPersonId", "AverageRating", "DeliveryName", "Email", "LicenseNumber", "MobileNo", "Status", "TotalDeliveries", "otp" },
                values: new object[] { 1, 4.7999999999999998, "Arjun Kumar", "arjun@delivery.com", "TN45DL7890", "9876543210", "Available", 100, null });

            migrationBuilder.InsertData(
                table: "DineIn",
                columns: new[] { "TableId", "Capacity", "RestaurantId", "RestaurantsRestaurantId", "Status", "TableNo" },
                values: new object[,]
                {
                    { 1, 4, 1, null, "Available", "T1" },
                    { 2, 6, 1, null, "Reserved", "T2" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Description", "RoleName" },
                values: new object[,]
                {
                    { 1, "Standard customer account", "Customer" },
                    { 2, "Manages Restaurant", "Manager" },
                    { 3, "Restaurant administrator", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "IsActive", "LastLogin", "Mobile", "Password", "RoleId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "john@example.com", true, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "hashedPassword1", 2 },
                    { 2, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", true, new DateTime(2023, 1, 2, 12, 0, 0, 0, DateTimeKind.Unspecified), null, "hashedPassword2", 1 }
                });

            migrationBuilder.InsertData(
                table: "Cart",
                columns: new[] { "CartId", "AddedAt", "ItemId", "Quantity", "RestaurantId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 1, 1 },
                    { 2, new DateTime(2023, 1, 2, 12, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "DeliveryAddress",
                columns: new[] { "AddressId", "Address", "City", "ContactNo", "IsDefault", "Landmark", "Mobile", "Pincode", "State", "UserId" },
                values: new object[,]
                {
                    { 1, "123 MG Road", "Chennai", "9876543210", true, "Near Park", "9876543210", "600001", "Tamil Nadu", 1 },
                    { 2, "456 T Nagar", "Chennai", "9876543220", false, "Near Mall", "9876543220", "600017", "Tamil Nadu", 1 }
                });

            migrationBuilder.InsertData(
                table: "ManagerDetails",
                columns: new[] { "ManagerId", "CreatedAt", "Email", "IsActive", "ManagerName", "PasswordHash", "PhoneNumber", "UpdatedAt", "UserId" },
                values: new object[] { 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "arjun@manager.com", true, "Arjun Kumar", "hashed123", "9876543210", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "ItemsList", "OrderDate", "OrderType", "QtyOrdered", "RestaurantId", "Status", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 1, "[{\"ItemId\":1,\"Quantity\":2}]", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "DineIn", 2, 0, "InProgress", 536m, 1 },
                    { 2, "[{\"ItemId\":2,\"Quantity\":1}]", new DateTime(2023, 1, 2, 12, 0, 0, 0, DateTimeKind.Unspecified), "DineOut", 1, 0, "Pending", 157m, 1 }
                });

            migrationBuilder.InsertData(
                table: "UserPreferences",
                columns: new[] { "PreferenceId", "NotificationsEnabled", "PreferredCity", "PreferredFoodType", "Theme", "UserId" },
                values: new object[,]
                {
                    { 1, true, "Chennai", "Indian", "Light", 1 },
                    { 2, false, "Bangalore", "Desserts", "Dark", 2 }
                });

            migrationBuilder.InsertData(
                table: "Wishlist",
                columns: new[] { "WishlistId", "CreatedAt", "ItemId", "RestaurantId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1 },
                    { 2, new DateTime(2023, 1, 2, 12, 0, 0, 0, DateTimeKind.Unspecified), 2, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "PaymentId", "Amount", "OrderId", "PayMethod", "PaymentDate", "Status" },
                values: new object[,]
                {
                    { 1, 536m, 1, "UPI", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "Completed" },
                    { 2, 157m, 2, "Cash", new DateTime(2023, 1, 2, 12, 0, 0, 0, DateTimeKind.Unspecified), "Pending" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "RestaurantId", "City", "ContactNo", "CreatedAt", "DeliveryCharge", "Description", "Images", "IsActive", "Location", "ManagerId", "Ratings", "RestaurantCategory", "RestaurantName", "RestaurantType" },
                values: new object[] { 1, "Chennai", "9876543210", new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), 50m, "Authentic Indian Cuisine", null, true, "MG Road, Chennai", 1, 9.2m, "Restaurant", "Spice Garden", "Both" });

            migrationBuilder.InsertData(
                table: "MenuList",
                columns: new[] { "ItemId", "AvailableQty", "Description", "Discount", "Image", "IsVegetarian", "ItemName", "Price", "RestaurantId", "Tax" },
                values: new object[,]
                {
                    { 1, 50, "Classic Indian curry", 10m, null, false, "Butter Chicken", 250m, 1, 18m },
                    { 2, 30, "Rich chocolate dessert", 5m, null, true, "Chocolate Cake", 150m, 1, 5m }
                });

            migrationBuilder.InsertData(
                table: "Review",
                columns: new[] { "ReviewId", "Comments", "Rating", "RestaurantId", "ReviewDate", "UserId" },
                values: new object[,]
                {
                    { 1, "Excellent food and service!", 9m, 1, new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, "Loved the desserts", 8m, 1, new DateTime(2023, 1, 2, 12, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_UserId",
                table: "Cart",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_AddressId",
                table: "Delivery",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_DeliveryPersonId",
                table: "Delivery",
                column: "DeliveryPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Delivery_OrderId",
                table: "Delivery",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddress_UserId",
                table: "DeliveryAddress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPerson_MobileNo",
                table: "DeliveryPerson",
                column: "MobileNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DineIn_RestaurantId_TableNo",
                table: "DineIn",
                columns: new[] { "RestaurantId", "TableNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DineIn_RestaurantsRestaurantId",
                table: "DineIn",
                column: "RestaurantsRestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerDetails_Email",
                table: "ManagerDetails",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManagerDetails_UserId",
                table: "ManagerDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerPayments_ManagerId",
                table: "ManagerPayments",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerPayments_RestaurantId",
                table: "ManagerPayments",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuList_RestaurantId",
                table: "MenuList",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderId",
                table: "Payment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_TableId",
                table: "Reservation",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_UserId",
                table: "Reservation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_ManagerId",
                table: "Restaurants",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_RestaurantName",
                table: "Restaurants",
                column: "RestaurantName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_RestaurantId",
                table: "Review",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_UserId",
                table: "Review",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RoleName",
                table: "Roles",
                column: "RoleName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_RestaurantId",
                table: "Wishlist",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_UserId_ItemId_RestaurantId",
                table: "Wishlist",
                columns: new[] { "UserId", "ItemId", "RestaurantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Delivery");

            migrationBuilder.DropTable(
                name: "ManagerPayments");

            migrationBuilder.DropTable(
                name: "MenuList");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Wishlist");

            migrationBuilder.DropTable(
                name: "DeliveryAddress");

            migrationBuilder.DropTable(
                name: "DeliveryPerson");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "DineIn");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "ManagerDetails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
