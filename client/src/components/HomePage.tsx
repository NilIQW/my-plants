import React, { useEffect, useState } from "react";
import { plantsClient } from "../apiControllerClients.ts";
import {PlantResponseDto, StringConstants} from "../generated-client.ts";
import "../css/HomePage.css";
import {useNavigate} from "react-router-dom";
import {CreatePlantRoute} from "../routeConstants.ts";
import {WaterNowButton} from "./WaterManuallyButton.tsx";
import {useWsClient} from "ws-request-hook";
import {WateringLogsButton} from "./WateringLogsButton.tsx";

const HomePage: React.FC = () => {
    const [plants, setPlants] = useState<PlantResponseDto[]>([]);
    const [selectedPlant, setSelectedPlant] = useState<PlantResponseDto | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [editData, setEditData] = useState<Partial<PlantResponseDto>>({});
    const navigate = useNavigate();
    const {readyState, send, onMessage} =  useWsClient();
    const [liveMoistureLevels, setLiveMoistureLevels] = useState<Record<string, number>>({});


    useEffect(() => {
        if (readyState !== 1) return;

        console.log("WebSocket ready, setting up onMessage");

        onMessage<WrapperForDto>(StringConstants.PlantDto, (wrapper) => {
            const plantDto = wrapper.dto;
            console.log("Received dto:", plantDto);

            setLiveMoistureLevels(prev => ({
                ...prev,
                [plantDto.id]: plantDto.moistureLevel,
            }));
        });
    }, [readyState]);




    useEffect(() => {
        fetchPlants();
    }, []);

    async function fetchPlants() {
        try {
            const plantsData = await plantsClient.getAll();
            setPlants(plantsData);
        } catch (error) {
            console.error("Failed to fetch plants:", error);
        }
    }

    const handleSelectChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const plantId = e.target.value;
        const plant = plants.find(p => p.id === plantId) || null;
        setSelectedPlant(plant);
        setIsEditing(false);
    };

    const handleEditClick = () => {
        if (selectedPlant) {
            setEditData({ ...selectedPlant });
            setIsEditing(true);
        }
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value, type, checked } = e.target;
        setEditData(prev => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value
        }));
    };

    const handleNavigate = async () => {
        navigate(CreatePlantRoute)
    }

    const handleDeleteClick = async () => {
        if (selectedPlant) {
            const confirmDelete = window.confirm(`Are you sure you want to delete "${selectedPlant.plantName}"?`);
            if (!confirmDelete) return;

            try {
                await plantsClient.delete(selectedPlant.id);

                setPlants(prevPlants => prevPlants.filter(p => p.id !== selectedPlant.id));
                setSelectedPlant(null);
                setIsEditing(false);
            } catch (error) {
                console.error("Failed to delete plant:", error);
            }
        }
    };

    const handleSaveClick = async () => {
        if (selectedPlant && editData) {
            try {
                await plantsClient.update(selectedPlant.id, {
                    plantName: editData.plantName!,
                    plantType: editData.plantType!,
                    moistureThreshold: Number(editData.moistureThreshold),
                    isAutoWateringEnabled: editData.isAutoWateringEnabled!,
                });

                const updatedPlant: PlantResponseDto = {
                    ...selectedPlant,
                    plantName: editData.plantName!,
                    plantType: editData.plantType!,
                    moistureThreshold: Number(editData.moistureThreshold),
                    isAutoWateringEnabled: editData.isAutoWateringEnabled!,
                };

                setPlants(prevPlants =>
                    prevPlants.map(p => (p.id === updatedPlant.id ? updatedPlant : p))
                );
                setSelectedPlant(updatedPlant);
                setIsEditing(false);
            } catch (error) {
                console.error("Failed to update plant:", error);
            }
        }
    };

    return (
        <>
            <div className="container">
                <h1>Choose a Plant</h1>
                <select className="select" onChange={handleSelectChange} value={selectedPlant?.id || ""}>
                    <option value="">-- Select a plant --</option>
                    {plants.map((plant) => (
                        <option key={plant.id} value={plant.id}>
                            {plant.plantName}
                        </option>
                    ))}
                </select>

                {selectedPlant && (
                    <div className="plant-details">
                        <h2>Plant Details</h2>
                        {isEditing ? (
                            <div className="form">
                                <div>
                                    <label>Name: </label>
                                    <input
                                        type="text"
                                        name="plantName"
                                        value={editData.plantName || ""}
                                        onChange={handleInputChange}
                                        className="input"
                                    />
                                </div>
                                <div>
                                    <label>Type: </label>
                                    <input
                                        type="text"
                                        name="plantType"
                                        value={editData.plantType || ""}
                                        onChange={handleInputChange}
                                        className="input"
                                    />
                                </div>
                                <div>
                                    <label>Moisture Threshold: </label>
                                    <input
                                        type="number"
                                        name="moistureThreshold"
                                        value={editData.moistureThreshold || 0}
                                        onChange={handleInputChange}
                                        className="input"
                                    />
                                </div>
                                <div>
                                    <label>Auto Watering Enabled: </label>
                                    <input
                                        type="checkbox"
                                        name="isAutoWateringEnabled"
                                        checked={editData.isAutoWateringEnabled || false}
                                        onChange={handleInputChange}
                                    />
                                </div>
                                <button className="save-button" onClick={handleSaveClick}>Save</button>
                            </div>
                        ) : (
                            <div>
                                <p><strong>Name:</strong> {selectedPlant.plantName}</p>
                                <p><strong>Type:</strong> {selectedPlant.plantType}</p>
                                <p><strong>Current Moisture:</strong> {liveMoistureLevels[selectedPlant.id] ?? selectedPlant.moistureLevel}</p>
                                <p><strong>Moisture Threshold:</strong> {selectedPlant.moistureThreshold}</p>
                                <p><strong>Auto Watering Enabled:</strong> {selectedPlant.isAutoWateringEnabled ? "Yes" : "No"}</p>
                                <div className="button-group">
                                    <button className="edit-button" onClick={handleEditClick}>Edit</button>
                                    <button className="delete-button" onClick={handleDeleteClick}>Delete</button>
                                    <WateringLogsButton plantId={selectedPlant.id}/>
                                    <WaterNowButton plantId={selectedPlant.id}/>
                                </div>
                            </div>
                        )}
                    </div>
                )}
                <button onClick={handleNavigate} className="create-button">Create Plant</button>
            </div>
        </>
    );
};

export default HomePage;

export interface WrapperForDto  {
    eventType: string;
    dto: PlantDto;
}

export interface PlantDto {
    id: string;
    plantName?: string;
    plantType?: string;
    moistureLevel: number;
    moistureThreshold?: number;
    isAutoWateringEnabled?: boolean;
}