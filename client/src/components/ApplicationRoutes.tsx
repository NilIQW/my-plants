import {Route, Routes, useNavigate} from "react-router";
import useInitializeData from "../hooks/useInitializeData.tsx";
import {
    CreatePlantRoute,
    HomePageRoute,
    RegisterRoute,
    SignInRoute
} from '../routeConstants.ts';
import useSubscribeToTopics from "../hooks/useSubscribeToTopics.tsx";
import SignIn from "./SignIn.tsx";
import {useEffect} from "react";
import {useAtom} from "jotai";
import {JwtAtom} from "../atoms.ts";
import toast from "react-hot-toast";
import Register from "./Register.tsx";
import HomePage from "./HomePage.tsx";
import CreatePlant from "./CreatePlant.tsx";
import Logout from "./LogOut.tsx";

export default function ApplicationRoutes() {
    
    const navigate = useNavigate();
    const [jwt] = useAtom(JwtAtom);
    useInitializeData();
    useSubscribeToTopics();

    useEffect(() => {
        if (jwt == undefined || jwt.length < 1) {
            navigate(SignInRoute)
            toast("Please sign in to continue")
        }
    }, [])
    
    return (<>
        {/*the browser router is defined in main tsx so that i can use useNavigate anywhere*/}
        <Logout/>
        <Routes>
            <Route element={<SignIn/>} path={SignInRoute}/>
            <Route element={<Register/>} path={RegisterRoute}/>
            <Route element={<HomePage/>} path={HomePageRoute}/>
            <Route element={<CreatePlant/>} path={CreatePlantRoute}/>
        </Routes>
    </>)
}