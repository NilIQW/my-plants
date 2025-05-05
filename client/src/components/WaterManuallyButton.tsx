import React from "react";
import { useWsClient, useWsSubscription } from "ws-request-hook";
import {
    WaterNowClientDto,
    WaterNowServerResponse,
    ServerSendsErrorMessage,
    StringConstants,
} from "../generated-client.ts";
import toast from "react-hot-toast";

interface WaterNowButtonProps {
    plantId: string;
}

export const WaterNowButton: React.FC<WaterNowButtonProps> = ({ plantId }) => {
    const { readyState, send } = useWsClient();

    useWsSubscription<WaterNowServerResponse>(StringConstants.WaterNowServerResponse, (message) => {
        if (message?.message) {
            toast.success(message.message);
        } else {
            toast.success("Watering successful!");
        }
    });

    useWsSubscription<ServerSendsErrorMessage>(StringConstants.ServerSendsErrorMessage, (message) => {
        if (message?.message) {
            toast.error(`Error: ${message.message}`);
        } else {
            toast.error("Something went wrong while watering.");
        }
    });

    const handleClick = () => {
        if (readyState !== WebSocket.OPEN) {
            alert("WebSocket is not connected.");
            return;
        }

        const dto: WaterNowClientDto = {
            eventType: StringConstants.WaterNowClientDto,
            requestId: crypto.randomUUID(),
            plantId,
        };

        console.log("Sending WaterNowClientDto:", dto);
        toast("Watering the plant now...");
        send(dto);
    };

    return (
        <button
            onClick={handleClick}
            disabled={readyState !== WebSocket.OPEN}
            className={`px-4 py-2 rounded text-white transition ${
                readyState === WebSocket.OPEN
                    ? "bg-cyan-600 hover:bg-cyan-700"
                    : "bg-gray-400 cursor-not-allowed"
            }`}
        >
            {readyState === WebSocket.OPEN ? "Water Now" : "Connecting..."}
        </button>
    );
};
