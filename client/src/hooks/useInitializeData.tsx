import {useAtom} from "jotai";
import {JwtAtom} from "../atoms.ts";

export default function useInitializeData() {

    const [jwt] = useAtom(JwtAtom);


}