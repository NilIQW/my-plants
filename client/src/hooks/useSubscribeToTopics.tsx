import {useEffect} from "react";
import {ChangeSubscriptionDto, StringConstants} from "../generated-client.ts";
import toast from "react-hot-toast";
import {randomUid} from "../components/App.tsx";
import {useWsClient} from "ws-request-hook";
import {useAtom} from "jotai";
import {JwtAtom} from "../atoms.ts";
import {subscriptionClient} from "../apiControllerClients.ts";

export default function useSubscribeToTopics() {

    const [jwt] = useAtom(JwtAtom);
    const {readyState} = useWsClient();

    useEffect(() => {
        if (readyState != 1 || jwt == null || jwt.length < 1)
            return;
        const subscribeDto: ChangeSubscriptionDto = {
            clientId: randomUid,
            topicIds: [StringConstants.PlantDto],
        };
        subscriptionClient.subscribe(jwt, subscribeDto).then(r => {
            toast("You are subscribed to live moisture levels of your plants!");
        })

        console.log(subscribeDto , "dfgkdgm,ok")

    }, [readyState, jwt])
}