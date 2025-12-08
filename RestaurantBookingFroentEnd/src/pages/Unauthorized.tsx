import React from "react";
import { Link } from "react-router-dom";
import "../Styles/Authentication/Unauthorized.css";

const Unauthorized: React.FC = () => {
  return (
    <div className="unauth-container">
      <h1 className="unauth-code">403</h1>
      <h2 className="unauth-title">Unauthorized</h2>
      <p className="unauth-text">
        You don't have permission to view this page.
      </p>

      <Link to="/" className="unauth-btn">
        Go Back to Home
      </Link>
    </div>
  );
};

export default Unauthorized;
