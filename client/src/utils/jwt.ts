import {jwtDecode} from "jwt-decode";

export interface DecodedJwt {
    Id: string;
    email: string;
    role: string;
    exp: string;
}

export function getUserIdFromJwt(): string | null {
    const token = localStorage.getItem("jwt");
    console.log(token)
    if (!token) return null;

    try {
        const decoded = jwtDecode<DecodedJwt>(token);
        console.log(decoded.Id);

        return decoded.Id;
    } catch {
        return null;
    }
}
