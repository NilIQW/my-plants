import React, { useState } from "react";
import { WateringLogDto } from "../generated-client";
import { wateringLogClient } from "../apiControllerClients";

interface Props {
    plantId: string;
}

export const WateringLogsButton: React.FC<Props> = ({ plantId }) => {
    const [logs, setLogs] = useState<WateringLogDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [visible, setVisible] = useState(false);

    function getWateringMethodLabel(method?: number): string {
        const wateringMethodLabels: Record<number, string> = {
            0: "Auto",
            1: "Manual",
        };
        return method !== undefined ? wateringMethodLabels[method] ?? "Unknown" : "Unknown";
    }



    const fetchLogs = async () => {
        try {
            setLoading(true);
            const response = await wateringLogClient.getByPlantId(plantId);
            setLogs(response);
            setVisible(true);
        } catch (error) {
            console.error("Failed to fetch watering logs", error);
            alert("Could not load watering logs.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <>
            <button
                onClick={fetchLogs}
                disabled={loading}
                className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700 disabled:opacity-50"
            >
                {loading ? "Loading..." : "Show Watering Logs"}
            </button>

            {/* Modal Overlay */}
            {visible && (
                <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-40">
                    <div className="relative bg-white rounded-lg shadow-xl w-full max-w-lg p-6">
                        {/* Close button */}
                        <button
                            onClick={() => setVisible(false)}
                            className="absolute top-2 right-2 text-gray-500 hover:text-gray-800"
                        >
                            ✕
                        </button>

                        <h2 className="text-xl font-semibold mb-4 text-center">Watering Logs</h2>

                        {logs.length > 0 ? (
                            <ul className="space-y-3 max-h-96 overflow-y-auto text-sm">
                                {logs.map((log) => (
                                    <li key={log.id} className="border-b pb-2">
                                        <div className="font-medium">
                                            {log.timestamp
                                                ? new Date(log.timestamp).toLocaleString()
                                                : "No timestamp"}
                                        </div>
                                        <div className="text-gray-600">
                                            Method: {getWateringMethodLabel(log.method)}
                                        </div>


                                    </li>
                                ))}
                            </ul>
                        ) : (
                            <p className="text-gray-500 text-sm text-center">No logs found for this plant.</p>
                        )}
                    </div>
                </div>
            )}
        </>
    );
};
