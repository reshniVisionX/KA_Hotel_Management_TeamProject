import { useState } from "react";
import { useNavigate } from "react-router-dom";
import Toast, { type ToastType } from "../Utils/Toast";
import { registerApi } from "../Service/auth.api";
import type { Register } from "../Types/Dashboard/Register";
import "../Styles/Authentication/SignUp.css";

export const Signup = () => {
  const navigate = useNavigate();

  const [form, setForm] = useState<Register>({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    mobile: "",
  });

  const [errors, setErrors] = useState<Record<string, string>>({});
  const [toast, setToast] = useState<{ message: string; type: ToastType | null }>({
    message: "",
    type: null,
  });

  const [loading, setLoading] = useState(false);

  // ----------------- VALIDATION RULES -----------------
  const validators: Record<string, (value: string) => string> = {
    firstName: (v) =>
      !v ? "First name is required" : /^[A-Za-z\s]+$/.test(v) ? "" : "Only letters allowed",

    lastName: (v) =>
      v && !/^[A-Za-z\s]+$/.test(v) ? "Only letters allowed" : "",

    email: (v) =>
      !v
        ? "Email is required"
        : /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v)
        ? ""
        : "Invalid email format",

    mobile: (v) =>
      !v
        ? "Mobile number is required"
        : /^[7-9][0-9]{9}$/.test(v)
        ? ""
        : "Mobile must be 10 digits and start with 7-9",

    password: (v) =>
      !v ? "Password is required" : v.length < 5 ? "Password must be at least 5 characters" : "",
  };

  // ----------------- INPUT HANDLER -----------------
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    let filtered = value;

    if (name === "mobile") filtered = value.replace(/\D/g, "").slice(0, 10);
    if (name === "firstName" || name === "lastName")
      filtered = value.replace(/[^A-Za-z\s]/g, "");

    const updatedForm = { ...form, [name]: filtered };
    setForm(updatedForm);

    const msg = validators[name]?.(filtered) || "";
    setErrors((prev) => ({ ...prev, [name]: msg }));
  };

  const showToast = (message: string, type: ToastType) => {
    setToast({ message, type });
  };

  // ----------------- HANDLE SIGNUP -----------------
  const handleSignup = async () => {
    const newErrors: Record<string, string> = {};

    Object.keys(validators).forEach((field) => {
      const value = form[field as keyof Register] as string;
      const errMsg = validators[field](value);
      if (errMsg) newErrors[field] = errMsg;
    });

    setErrors(newErrors);

    if (Object.keys(newErrors).length > 0) {
      showToast("Please fix the errors above.", "error");
      return;
    }

    try {
      setLoading(true);
      const res = await registerApi(form);
      const body = res.data;

      if (body.success === false) {
        showToast(body.message, "error");
        setLoading(false);
        return;
      }

      showToast("Registration successful!", "success");
      setTimeout(() => navigate("/login"), 1200);
    } catch (err) {
      const message = err instanceof Error ? err.message : "Server error";
      showToast(message, "error");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="signup-container">
      <div className="signup-card">
        <h2 className="signup-title">📝 Create Your Account</h2>
        <p className="signup-subtitle">Register to access your dashboard</p>

        {/* FIRST NAME */}
        <input
          name="firstName"
          placeholder="First Name"
          className="signup-input"
          value={form.firstName}
          onChange={handleChange}
        />
        <div className="signup-error">{errors.firstName}</div>

        {/* LAST NAME */}
        <input
          name="lastName"
          placeholder="Last Name (optional)"
          className="signup-input"
          value={form.lastName}
          onChange={handleChange}
        />
        <div className="signup-error">{errors.lastName}</div>

        {/* EMAIL */}
        <input
          name="email"
          type="email"
          placeholder="Email Address"
          className="signup-input"
          value={form.email}
          onChange={handleChange}
        />
        <div className="signup-error">{errors.email}</div>

        {/* MOBILE */}
        <input
          name="mobile"
          placeholder="Mobile Number"
          className="signup-input"
          value={form.mobile}
          onChange={handleChange}
        />
        <div className="signup-error">{errors.mobile}</div>

        {/* PASSWORD */}
        <input
          name="password"
          type="password"
          placeholder="Password"
          className="signup-input"
          value={form.password}
          onChange={handleChange}
        />
        <div className="signup-error">{errors.password}</div>

        {/* BUTTON */}
        <button className="signup-btn" disabled={loading} onClick={handleSignup}>
          {loading ? "Registering..." : "Sign Up"}
        </button>

        <p className="signup-login-text">
          Already have an account?{" "}
          <span className="signup-login-link" onClick={() => navigate("/login")}>
            Login
          </span>
        </p>
      </div>

      {toast.message && toast.type && (
        <Toast
          message={toast.message}
          type={toast.type}
          onClose={() => setToast({ message: "", type: null })}
        />
      )}
    </div>
  );
};
