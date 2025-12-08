import { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { loginUser } from "../redux/thunks/authThunk";
import { generateOtpApi } from "../Service/auth.api";
import type { AppDispatch, RootState } from "../redux/store";
import Toast, { type ToastType } from "../Utils/Toast";
import type { Login } from "../Types/Dashboard/Login";
import "../Styles/Authentication/Login.css";

export const LoginForm = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const { user, loading, error } = useSelector((state: RootState) => state.auth);

  const [loginMode, setLoginMode] = useState<"email" | "mobile">("email");

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [mobile, setMobile] = useState("");
  const [otp, setOtp] = useState("");

  const [showPassword, setShowPassword] = useState(false);
  const [otpModal, setOtpModal] = useState(false);

  const [toast, setToast] = useState<{ message: string; type: ToastType | null }>({
    message: "",
    type: null,
  });

  // --------------------- VALIDATION ---------------------
  const validateEmail = (value: string) => {
    if (!value.trim()) return "Email is required.";
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(value) ? "" : "Invalid email format.";
  };

  const validatePassword = (value: string) => {
    if (!value.trim()) return "Password is required.";
    if (value.length < 6) return "Password must be at least 6 characters.";
    return "";
  };

  const validateMobile = (value: string) => {
    if (!value.trim()) return "Mobile number is required.";
    if (!/^[0-9]{10}$/.test(value)) return "Mobile must be 10 digits.";
    return "";
  };

  // --------------------- SEND OTP ---------------------
  const handleSendOtp = async () => {
    const mobileErr = validateMobile(mobile);
    if (mobileErr) {
      setToast({ message: mobileErr, type: "error" });
      return;
    }

    try {
    await generateOtpApi(mobile);

      setToast({
        message: `OTP sent successfully!`,
        type: "success",
      });

      setOtpModal(true);
    } catch (err) {
      const message = err instanceof Error ? err.message : "Unknown error";
      setToast({ message: message, type: "error" });
    }
  };

  // --------------------- LOGIN SUBMIT ---------------------
  const handleLogin = () => {
    let loginDTO: Login;

    if (loginMode === "email") {
      const emailErr = validateEmail(email);
      const passwordErr = validatePassword(password);
      if (emailErr || passwordErr) {
        setToast({ message: "Invalid email or password.", type: "error" });
        return;
      }
      loginDTO = { email, password };
    } else {
      const mobileErr = validateMobile(mobile);
      if (mobileErr || !otp.trim()) {
        setToast({ message: "Enter valid mobile number and OTP.", type: "error" });
        return;
      }
      loginDTO = { mobile, otp };
    }

    dispatch(loginUser(loginDTO));
  };

  // --------------------- EFFECTS ---------------------
  useEffect(() => {
    if (error) setToast({ message: error, type: "error" });
  }, [error]);

  useEffect(() => {
    if (user) {
      setToast({ message: `Welcome ${user.firstName}!`, type: "success" });
      setTimeout(() => {
        const role = user.roleName.toLowerCase();
        if (role === "admin") navigate("/admin-dashboard");
        else if (role === "manager") navigate("/manager-dashboard");
        else navigate("/customer-dashboard");
      }, 800);
    }
  }, [user]);

  return (
    <div className="login2-container">

      {/* -------------------- SIDEBAR -------------------- */}
      <div className="login2-sidebar">
        <h2 className="login2-title">Login Options</h2>

        <button
          className={`login2-side-btn ${loginMode === "email" ? "active" : ""}`}
          onClick={() => setLoginMode("email")}
        >
          Login by Email
        </button>

        <button
          className={`login2-side-btn ${loginMode === "mobile" ? "active" : ""}`}
          onClick={() => setLoginMode("mobile")}
        >
          Login by Mobile
        </button>

        <p className="login2-signup-text">
          New here? <span onClick={() => navigate("/signup")}>Create account</span>
        </p>
      </div>

      {/* -------------------- MAIN FORM -------------------- */}
      <div className="login2-form-area">
        <div className="login2-card">

          {loginMode === "email" && (
            <>
              <h2 className="login2-form-title">Email Login</h2>

              <input
                type="email"
                placeholder="Enter email"
                className="login2-input"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />

              <div className="login2-password-group">
                <input
                  type={showPassword ? "text" : "password"}
                  placeholder="Enter password"
                  className="login2-input"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
                <span
                  className="login2-toggle-password"
                  onClick={() => setShowPassword(!showPassword)}
                >
                  {showPassword ? "🙈" : "👁️"}
                </span>
              </div>

              <button className="login2-btn" onClick={handleLogin}>
                {loading ? "Logging in..." : "Login"}
              </button>
            </>
          )}

          {loginMode === "mobile" && (
            <>
              <h2 className="login2-form-title">Mobile Login</h2>

              <input
                type="text"
                placeholder="Enter mobile number"
                className="login2-input"
                value={mobile}
                onChange={(e) => setMobile(e.target.value.replace(/\D/g, "").slice(0, 10))}
              />

              <button className="login2-btn" onClick={handleSendOtp}>
                Send OTP
              </button>
            </>
          )}
        </div>
      </div>

      {/* -------------------- OTP MODAL -------------------- */}
      {otpModal && (
        <div className="login2-otp-overlay">
          <div className="login2-otp-card">
            <h3>Enter OTP</h3>

            <input
              type="text"
              maxLength={6}
              placeholder="Enter OTP"
              className="login2-input"
              value={otp}
              onChange={(e) => setOtp(e.target.value.replace(/\D/g, "").slice(0, 6))}
            />

            <button className="login2-btn" onClick={handleLogin}>
              Validate OTP & Login
            </button>

            <button className="login2-resend" onClick={handleSendOtp}>
              Resend OTP
            </button>

            <span className="login2-close" onClick={() => setOtpModal(false)}>
              ✖
            </span>
          </div>
        </div>
      )}

      {/* Toast */}
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
