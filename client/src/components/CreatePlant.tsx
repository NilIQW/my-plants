import React, { useState } from "react";
import { plantsClient } from "../apiControllerClients.ts";
import { CreatePlantDto, PlantResponseDto } from "../generated-client.ts";
import "../css/HomePage.css";
import { getUserIdFromJwt } from "../utils/jwt.ts";

const CreatePlant: React.FC = () => {
    const [newPlantData, setNewPlantData] = useState<Omit<CreatePlantDto, "userId">>({
        plantName: "",
        plantType: "",
        moistureThreshold: 0,
        isAutoWateringEnabled: false,
    });
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value, type, checked } = e.target;
        setNewPlantData(prev => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value,
        }));
    };

    const handleCreatePlant = async () => {
        setIsLoading(true);
        setError(null);

        const userId = getUserIdFromJwt();
        if (!userId) {
            setError("You must be logged in to create a plant.");
            setIsLoading(false);
            return;
        }

        try {
            const plantData: CreatePlantDto = {
                ...newPlantData,
                userId: userId,
            };

            const response: PlantResponseDto = await plantsClient.create(plantData);

            console.log("Plant created successfully:", response);
            setNewPlantData({
                plantName: "",
                plantType: "",
                moistureThreshold: 0,
                isAutoWateringEnabled: false,
            });
            alert("Plant created successfully!");
        } catch (err) {
            setError("Failed to create plant. Please try again.");
            console.error("Error creating plant:", err);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="container">
            <h1>Create a New Plant</h1>
            <div className="plant-details">
                <div>
                    <label htmlFor="plantName">Plant Name:</label>
                    <input
                        id="plantName"
                        className="input"
                        type="text"
                        name="plantName"
                        value={newPlantData.plantName || ""}
                        onChange={handleInputChange}
                    />
                </div>

                <div>
                    <label htmlFor="plantType">Plant Type:</label>
                    <input
                        id="plantType"
                        className="input"
                        type="text"
                        name="plantType"
                        value={newPlantData.plantType || ""}
                        onChange={handleInputChange}
                    />
                </div>

                <div>
                    <label htmlFor="moistureThreshold">Moisture Threshold:</label>
                    <input
                        id="moistureThreshold"
                        className="input"
                        type="number"
                        name="moistureThreshold"
                        value={newPlantData.moistureThreshold || 0}
                        onChange={handleInputChange}
                    />
                </div>

                <div>
                    <label htmlFor="isAutoWateringEnabled">Enable Auto Watering:</label>
                    <input
                        type="checkbox"
                        name="isAutoWateringEnabled"
                        checked={newPlantData.isAutoWateringEnabled || false}
                        onChange={handleInputChange}
                    />
                </div>

                {error && <p style={{ color: "red" }}>{error}</p>}

                <button
                    className="create-button"
                    onClick={handleCreatePlant}
                    disabled={isLoading}
                >
                    {isLoading ? "Creating..." : "Create Plant"}
                </button>
            </div>
        </div>
    );
};

export default CreatePlant;
