import { useEffect, useState, useMemo } from "react";
import {
    Card,
    CardContent,
    CardMedia,
    Typography,
    CircularProgress,
    Box,
    Rating,
    Container
} from "@mui/material";
import { useAppSelector } from "../../redux/hooks";
import { fetchAllRestaurants } from "../../redux/thunks/restaurantThunk";
import { useAppDispatch } from "../../redux/hooks";
import Toast, { type ToastType } from "../../Utils/Toast";
import "../../Styles/Restaurant/ListRestaurants.css";
import { useNavigate } from "react-router-dom";

export const ListRestaurants = () => {
    const dispatch = useAppDispatch();
    const navigate = useNavigate();
    const { restaurants, loading, error } = useAppSelector((state) => state.restaurants);
    const user = useAppSelector((state) => state.auth.user);
    const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);

    const showToast = (message: string, type: ToastType) => {
        setToast({ message, type });
    };

    const filteredRestaurants = useMemo(() => {
        if (!user?.city) return [];
        
        return restaurants.filter(restaurant => 
            restaurant.city?.toLowerCase() === user.city?.toLowerCase() && 
            restaurant.isActive === true
        );
    }, [restaurants, user?.city]);

    useEffect(() => {
        if (restaurants.length === 0) {
            dispatch(fetchAllRestaurants());
        }
    }, [dispatch, restaurants.length]);

    useEffect(() => {
        if (error) {
            showToast(error, "error");
        }
    }, [error]);

    if (loading) {
        return (
            <Box className="loading-container">
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Container className="restaurants-container">
            {toast && (
                <Toast
                    message={toast.message}
                    type={toast.type}
                    onClose={() => setToast(null)}
                />
            )}

            <Typography variant="h4" className="restaurants-title">
                Restaurants in {user?.city}
            </Typography>

            <div className="restaurants-grid">
                {filteredRestaurants.map((restaurant) => (
                    <Card className="restaurant-card" key={restaurant.restaurantId} onClick={() => navigate(`/restaurant-menu/${restaurant.restaurantId}/${encodeURIComponent(restaurant.restaurantName)}`)}>
                        <CardMedia
                            component="img"
                            className="restaurant-image"
                            image={
                                restaurant.images
                                    ? `data:image/jpeg;base64,${restaurant.images}`
                                    : "/default-restaurant.jpg"
                            }
                            alt={restaurant.restaurantName}
                        />

                        <CardContent className="restaurant-content">
                            <Typography variant="h6" className="restaurant-name">
                                {restaurant.restaurantName}
                            </Typography>

                            <Typography variant="body2" className="restaurant-description">
                                {restaurant.description || "No description available"}
                            </Typography>

                            <Box className="rating-container">
                                <Rating
                                    value={restaurant.ratings || 0}
                                    readOnly
                                    size="small"
                                    className="restaurant-rating"
                                />
                                <Typography variant="caption" className="rating-text">
                                    ({restaurant.ratings || 0})
                                </Typography>
                            </Box>
                        </CardContent>
                    </Card>
                ))}
            </div>

            {filteredRestaurants.length === 0 && !loading && (
                <Typography variant="h6" className="no-restaurants">
                    No restaurants available in your city.
                </Typography>
            )}
        </Container>
    );
};
