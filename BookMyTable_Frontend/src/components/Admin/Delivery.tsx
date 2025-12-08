import { useNavigate } from 'react-router-dom';
import { useAppSelector } from '../../redux/hooks';
import { DeliveryPersonList } from '../Restaurant/DeliveryPerson';
import '../../Styles/Admin/Delivery.css';

const Delivery = () => {
  const navigate = useNavigate();
  const user = useAppSelector((state) => state.auth.user);

  return (
    <div className="admin-delivery-container">
      <div className="delivery-header">
        <h1 className="delivery-title">Delivery Management</h1>
        {user?.roleName === "Admin" && (
          <button 
            className="add-delivery-btn"
            onClick={() => navigate('/admin/add-delivery-person')}
          >
            + Add Delivery Person
          </button>
        )}
      </div>
      <DeliveryPersonList />
    </div>
  );
};

export default Delivery;
