import { useAtom } from "jotai";
import { JwtAtom } from "../atoms";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { authClient } from "../apiControllerClients";
import toast from "react-hot-toast";
import {HomePageRoute, PlantSearchRoute, RegisterRoute} from "../routeConstants.ts";

export default function SignIn() {
    const [jwt, setJwt] = useAtom(JwtAtom);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        if (jwt && jwt.length > 0) {
            navigate(HomePageRoute); // Redirect to home if already signed in
        }
    }, [jwt, navigate]);

    const handleLogin = async () => {
        try {
            const response = await authClient.login({ email, password });
            toast.success("Logged in successfully!");
            localStorage.setItem("jwt", response.jwt);
            setJwt(response.jwt);
            navigate("/"); // Redirect after login
        } catch (error) {
            toast.error("Login failed");
            console.error(error);
        }
    };

    return (
        <div className="flex flex-col items-center justify-center h-screen space-y-4">
            <h1 className="text-xl">Sign In</h1>
            <input
                type="email"
                className="input input-bordered"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
            />
            <input
                type="password"
                className="input input-bordered"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <button className="btn btn-primary" onClick={handleLogin}>
                Sign In
            </button>
            <button className="btn btn-link" onClick={() => navigate(RegisterRoute)}>
                Register
            </button>
        </div>
    );
}
