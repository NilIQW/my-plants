import { authClient } from "../apiControllerClients.ts";
import toast from "react-hot-toast";
import { useAtom } from "jotai";
import { JwtAtom } from "../atoms.ts";
import { useState } from "react";
import {
    Link,
    useNavigate
} from "react-router-dom";
import {RegisterRoute} from "../routeConstants.ts";

export default function Register() {
    const [jwt, setJwt] = useAtom(JwtAtom);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const handleRegister = async () => {
        try {
            const response = await authClient.register({ email, password });
            toast.success("Registered successfully!");
            localStorage.setItem("jwt", response.jwt);
            setJwt(response.jwt);
            navigate(RegisterRoute);
        } catch (error: any) {
            toast.error("Registration failed.");
            console.error(error);
        }
    };

    return (
        <div className="flex flex-col items-center justify-center h-screen space-y-4">
            <h1 className="text-xl">Register</h1>
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
            <button className="btn btn-primary" onClick={handleRegister}>
                Register
            </button>
            <Link to="/signin" className="btn btn-link">
                Already have an account? Sign in
            </Link>
        </div>
    );
}
