import { authClient } from "../apiControllerClients.ts";
import toast from "react-hot-toast";
import { useAtom } from "jotai";
import { JwtAtom } from "../atoms.ts";
import { useState } from "react";
import { Link } from "react-router-dom";

export default function SignIn() {
    const [jwt, setJwt] = useAtom(JwtAtom);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleLogin = async () => {
        try {
            const response = await authClient.login({ email, password });
            toast.success("Logged in successfully!");
            localStorage.setItem("jwt", response.jwt);
            setJwt(response.jwt);
        } catch (error: any) {
            toast.error("Login failed. Please check your credentials.");
            console.error(error);
        }
    };

    const handleLogout = () => {
        localStorage.removeItem("jwt");
        setJwt("");
        toast("You have been signed out");
    };

    return (
        <div className="flex flex-col items-center justify-center h-screen space-y-4">
            {(jwt == null || jwt.length < 1) ? (
                <>
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
                    <Link to="/register" className="btn btn-link">
                        Don’t have an account? Register
                    </Link>
                </>
            ) : (
                <>
                    <h1 className="text-xl">You are already signed in.</h1>
                    <button className="btn btn-secondary" onClick={handleLogout}>
                        Sign Out
                    </button>
                </>
            )}
        </div>
    );
}
