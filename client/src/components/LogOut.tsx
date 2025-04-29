import {useNavigate} from "react-router-dom";
import {SignInRoute} from "../routeConstants.ts";

export default function Logout() {


    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem("jwt");
        navigate(SignInRoute);
        window.location.reload();
    }

    return (<>
         <span className="absolute top-0 right-0 m-10">
        <div className="indicator">
            <button onClick={handleLogout} className="btn">Log Out</button>
        </div>
     </span>
    </>)

}