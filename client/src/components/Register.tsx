import { authClient } from "../apiControllerClients.ts";
import toast from "react-hot-toast";
import { useAtom } from "jotai";
import { JwtAtom } from "../atoms.ts";
import { useState } from "react";
import {
    Link,
    useNavigate
} from "react-router-dom";
import { RegisterRoute, SignInRoute } from "../routeConstants.ts";

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
            navigate(SignInRoute);
        } catch (error: any) {
            toast.error("Registration failed.");
            console.error(error);
        }
    };

    return (
        <div className="flex items-center justify-center h-screen">
            <div className="w-full max-w-xs flex flex-col space-y-4">
                <h1 className="text-xl text-center">Register</h1>
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
                <button className="btn btn-primary w-full" onClick={handleRegister}>
                    Register
                </button>
                <button className="btn btn-link w-full" onClick={() => navigate(SignInRoute)}>
                    Already have an account? Sign in
                </button>
            </div>
        </div>
    );
}
