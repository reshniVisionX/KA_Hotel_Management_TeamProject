import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useDispatch } from "react-redux";
import {
    Card,
    CardContent,
    CardMedia,
    Typography,
    CircularProgress,
    Box,
    Container,
    Chip,
    IconButton,
    TextField,
    InputAdornment
} from "@mui/material";
import AddIcon from '@mui/icons-material/Add';
import FavoriteIcon from '@mui/icons-material/Favorite';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import SearchIcon from '@mui/icons-material/Search';
import { useAppSelector } from "../../redux/hooks";
import { fetchAllMenuItems } from "../../redux/thunks/menuThunk";
import { updateMenuItemLike } from "../../redux/slices/menuSlice";
import type { AppDispatch } from "../../redux/store";
import Toast, { type ToastType } from "../../Utils/Toast";
import { addToCart, addToWishlist, removeFromWishlist } from "../../Service/restaurant.api";
import type { AppError } from "../../Types/Dashboard/ApiError";
import "../../Styles/Restaurant/RestaurantsMenuList.css";

export const RestaurantsMenuList = () => {
    const { id, name } = useParams<{ id: string; name: string }>();
    const dispatch = useDispatch<AppDispatch>();
    const { menuItems, loading, error } = useAppSelector((state) => state.menu);
    const user = useAppSelector((state) => state.auth.user);
    const [toast, setToast] = useState<{ message: string; type: ToastType } | null>(null);
    const [searchTerm, setSearchTerm] = useState("");

    const restaurantId = parseInt(id || "0");
    const restaurantName = decodeURIComponent(name || "Restaurant Menu");

    const showToast = (message: string, type: ToastType) => {
        setToast({ message, type });
    };

    const handleAddToCart = async (itemId: number, restaurantId: number, availableQty: number) => {
        if (!user?.userId) {
            showToast("Please login to add items to cart", "error");
            return;
        }

        if (availableQty === 0) {
            showToast("Item is out of stock", "error");
            return;
        }

        try {
            await addToCart(user.userId, {
                itemId,
                restaurantId,
            
            });
            showToast("Item added to cart successfully!", "success");
        } catch (err) {
            const error = err as AppError;
            showToast(error.message, "error");
        }
    };

    const handleToggleWishlist = async (itemId: number, restaurantId: number, isCurrentlyLiked: boolean) => {
        if (!user?.userId) {
            showToast("Please login to manage wishlist", "error");
            return;
        }

        try {
            if (isCurrentlyLiked) {
                // Find wishlist item to get wishlistId for removal
                // Note: You might need to store wishlistId in the menu item or fetch it
                // For now, we'll assume the API handles removal by itemId and userId
                await removeFromWishlist(itemId); // This might need adjustment based on your API
                dispatch(updateMenuItemLike({ itemId, isLiked: false }));
                showToast("Item removed from wishlist", "success");
            } else {
                await addToWishlist(user.userId, {
                    itemId,
                    restaurantId
                });
                dispatch(updateMenuItemLike({ itemId, isLiked: true }));
                showToast("Item added to wishlist successfully!", "success");
            }
        } catch (err) {
            const error = err as AppError;
            showToast(error.message, "error");
        }
    };

    useEffect(() => {
        if (menuItems.length === 0) {
            dispatch(fetchAllMenuItems(user?.userId));
        }
    }, [dispatch, menuItems.length, user?.userId]);

    useEffect(() => {
        if (error) {
            showToast(error, "error");
        }
    }, [error]);

    const filteredMenuItems = menuItems
        .filter(item => item.restaurantId === restaurantId)
        .filter(item => 
            item.itemName.toLowerCase().includes(searchTerm.toLowerCase())
        );

    if (loading) {
        return (
            <Box className="loading-container">
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Container className="menu-container">
            {toast && (
                <Toast
                    message={toast.message}
                    type={toast.type}
                    onClose={() => setToast(null)}
                />
            )}

            <Typography variant="h4" className="menu-title">
                {restaurantName}
            </Typography>

            <Box className="search-container">
                <TextField
                    fullWidth
                    variant="outlined"
                    placeholder="Search menu items..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    InputProps={{
                        startAdornment: (
                            <InputAdornment position="start">
                                <SearchIcon />
                            </InputAdornment>
                        ),
                    }}
                    className="search-input"
                />
            </Box>

            <div className="menu-grid">
                {filteredMenuItems.map((item) => {
                    const isOutOfStock = item.availableQty === 0;
                    const isInWishlist = item.isLiked || false;

                    return (
                        <Card className="menu-card" key={item.itemId}>
                            <Box className="menu-image-container">
                                <CardMedia
                                    component="img"
                                    className="menu-image"
                                    image={
                                        item.image
                                            ? `data:image/jpeg;base64,${item.image}`
                                            : "/default-food.jpg"
                                    }
                                    alt={item.itemName}
                                />
                                <IconButton
                                    className="wishlist-btn"
                                    onClick={() => handleToggleWishlist(item.itemId, item.restaurantId, isInWishlist)}
                                    size="small"
                                >
                                    {isInWishlist ? (
                                        <FavoriteIcon className="wishlist-icon filled" />
                                    ) : (
                                        <FavoriteBorderIcon className="wishlist-icon" />
                                    )}
                                </IconButton>
                            </Box>
                            <CardContent className="menu-content">
                                <Typography variant="h6" className="menu-name">
                                    {item.itemName}
                                </Typography>

                                <Typography variant="body2" className="menu-description">
                                    {item.description}
                                </Typography>

                                <Box className="menu-details">
                                    <Typography variant="h6" className="menu-price">
                                        ₹{item.price}
                                    </Typography>
                                    {item.discount > 0 && (
                                        <Chip 
                                            label={`${item.discount}% OFF`} 
                                            color="success" 
                                            size="small"
                                            className="discount-chip"
                                        />
                                    )}
                                </Box>

                                <Box className="menu-tags">
                                    <Chip 
                                        label={item.isVegetarian ? "Veg" : "Non-Veg"} 
                                        color={item.isVegetarian ? "success" : "error"}
                                        size="small"
                                    />
                                    <IconButton 
                                        className={`add-to-cart-btn ${isOutOfStock ? 'disabled' : ''}`}
                                        onClick={() => handleAddToCart(item.itemId, item.restaurantId, item.availableQty)}
                                        size="small"
                                        disabled={isOutOfStock}
                                    >
                                        <AddIcon />
                                    </IconButton>
                                </Box>

                                {isOutOfStock && (
                                    <Typography variant="caption" className="out-of-stock-text">
                                        Out of Stock
                                    </Typography>
                                )}
                            </CardContent>
                        </Card>
                    );
                })}
            </div>
        </Container>
    );
};
