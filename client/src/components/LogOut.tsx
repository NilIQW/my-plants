import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { SignInRoute } from "../routeConstants.ts";

export default function Logout() {
    const navigate = useNavigate();
    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);

    useEffect(() => {
        const token = localStorage.getItem("jwt");
        setIsLoggedIn(!!token);
    }, []); // Runs once on mount

    const handleLogout = () => {
        localStorage.removeItem("jwt");
        setIsLoggedIn(false);
        navigate(SignInRoute);
        window.location.reload();
    };

    if (!isLoggedIn) return null;

    return (
        <span className="absolute top-[-10px] right-5">
            <div className="indicator">
                <button onClick={handleLogout} className="btn">
                    Log Out
                </button>
            </div>
        </span>
    );
}
