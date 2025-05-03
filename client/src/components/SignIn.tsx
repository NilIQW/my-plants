import { useAtom } from "jotai";
import { JwtAtom } from "../atoms";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { authClient } from "../apiControllerClients";
import toast from "react-hot-toast";
import {HomePageRoute,  RegisterRoute} from "../routeConstants.ts";

export default function SignIn() {
    const [jwt, setJwt] = useAtom(JwtAtom);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        if (jwt && jwt.length > 0) {
            navigate(HomePageRoute);
        }
    }, [jwt, navigate]);

    const handleLogin = async () => {
        try {
            const response = await authClient.login({ email, password });
            toast.success("Logged in successfully!");
            localStorage.setItem("jwt", response.jwt);
            setJwt(response.jwt);
            navigate(HomePageRoute);
        } catch (error) {
            toast.error("Login failed");
            console.error(error);
        }
    };

    return (
        <div className="flex items-center justify-center h-screen">
            <div className="w-full max-w-xs flex flex-col space-y-4">
                <h1 className="text-xl text-center">Sign In</h1>
                <input
                    type="email"
                    className="input input-bordered w-full"
                    placeholder="Email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <input
                    type="password"
                    className="input input-bordered w-full"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                <button className="btn btn-primary w-full" onClick={handleLogin}>
                    Sign In
                </button>
                <button className="btn btn-link w-full" onClick={() => navigate(RegisterRoute)}>
                    Register
                </button>
            </div>
        </div>

    );
}
