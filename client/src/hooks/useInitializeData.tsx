import {useEffect} from "react";
import {useAtom} from "jotai";
import {DeviceLogsAtom, JwtAtom} from "../atoms.ts";

export default function useInitializeData() {

    const [jwt] = useAtom(JwtAtom);


}