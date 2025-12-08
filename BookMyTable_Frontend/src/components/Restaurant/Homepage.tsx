import { useEffect } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import {
    Typography,
    CircularProgress,
    Box,
    Container
} from "@mui/material";
import { useAppSelector } from "../../redux/hooks";
import { fetchAllMenuItems } from "../../redux/thunks/menuThunk";
import { fetchAllRestaurants } from "../../redux/thunks/restaurantThunk";
import type { AppDispatch } from "../../redux/store";
import "../../Styles/Restaurant/Homepage.css";

export const Homepage = () => {
    const dispatch = useDispatch<AppDispatch>();
    const navigate = useNavigate();
    const user = useAppSelector((state) => state.auth.user);
    const { menuItems, loading: menuLoading } = useAppSelector((state) => state.menu);
    const { restaurants, loading: restaurantLoading } = useAppSelector((state) => state.restaurants);
const foodTypes: Record<number, string> = {
  0: "Veg",
  1: "Non-Veg",
  2: "Veg & Non-Veg",
};
    useEffect(() => {
        if (menuItems.length === 0) {
            dispatch(fetchAllMenuItems(user?.userId));
        }
        if (restaurants.length === 0) {
            dispatch(fetchAllRestaurants());
        }
    }, [dispatch, menuItems.length, restaurants.length, user?.userId]);

    const handleRestaurantClick = (restaurantId: number, restaurantName: string) => {
        navigate(`/restaurant-menu/${restaurantId}/${encodeURIComponent(restaurantName)}`);
    };

    const handleMenuItemClick = (restaurantId: number) => {
        const restaurant = restaurants.find(r => r.restaurantId === restaurantId);
        if (restaurant) {
            handleRestaurantClick(restaurantId, restaurant.restaurantName);
        }
    };

    // Get unique menu items (first 8 for display)
    const featuredMenuItems = menuItems.slice(0, 8);
    const featuredRestaurants = restaurants.slice(0, 6);

    if (menuLoading || restaurantLoading) {
        return (
            <Box className="loading-container">
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Container className="homepage-container">
            {/* Hero Section */}
            <div className="hero-section">
                <Typography variant="h2" className="hero-title">
                    Welcome to InstantEats
                </Typography>
                <Typography variant="h6" className="hero-subtitle">
                    Discover delicious food from your favorite restaurants
                </Typography>
            </div>

            {/* Featured Menu Items */}
            <div className="section">
                <Typography variant="h4" className="section-title">
                    Popular Dishes
                </Typography>
                <div className="menu-items-container">
                    {featuredMenuItems.map((item) => (
                        <div 
                            key={item.itemId} 
                            className="menu-item-card"
                            onClick={() => handleMenuItemClick(item.restaurantId)}
                        >
                            <div className="menu-item-image">
                                <img
                                    src={
                                        item.image
                                            ? `data:image/jpeg;base64,${item.image}`
                                            : "/default-food.jpg"
                                    }
                                    alt={item.itemName}
                                />
                            </div>
                            <div className="menu-item-info">
                                <Typography variant="body2" className="menu-item-name">
                                    {item.itemName}
                                </Typography>
                                <Typography variant="caption" className="menu-item-price">
                                    ₹{item.price}
                                </Typography>
                                <div className={`veg-indicator ${item.isVegetarian ? 'veg' : 'non-veg'}`}>
                                    {item.isVegetarian ? '🟢' : '🔴'}
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>

            {/* Featured Restaurants */}
            <div className="section">
                <Typography variant="h4" className="section-title">
                    Top Restaurants
                </Typography>
                <div className="restaurants-grid">
                    {featuredRestaurants.map((restaurant) => (
                        <div 
                            key={restaurant.restaurantId} 
                            className="restaurant-card-home"
                            onClick={() => handleRestaurantClick(restaurant.restaurantId, restaurant.restaurantName)}
                        >
                            <div className="restaurant-image-home">
                                <img
                                    src={
                                        restaurant.images
                                            ? `data:image/jpeg;base64,${restaurant.images}`
                                            : "/default-restaurant.jpg"
                                    }
                                    alt={restaurant.restaurantName}
                                />
                            </div>
                            <div className="restaurant-info-home">
                                <Typography variant="h6" className="restaurant-name-home">
                                    {restaurant.restaurantName}
                                </Typography>
                                <Typography variant="body2" className="restaurant-location">
                                    {foodTypes[restaurant.restaurantType]}
                                </Typography>
                                <div className="restaurant-rating">
                                    ⭐ {restaurant.ratings || 0}
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </Container>
    );
};
