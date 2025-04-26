import React, { useEffect, useState } from "react";
import { plantsClient } from "../apiControllerClients.ts";

interface PlantDto {
    id: string;
    plantName: string;
    plantType: string;
    moistureLevel: number;
    moistureThreshold: number;
    isAutoWateringEnabled: boolean;
}

const HomePage: React.FC = () => {
    const [plants, setPlants] = useState<PlantDto[]>([]);
    const [selectedPlant, setSelectedPlant] = useState<PlantDto | null>(null);
    const [isEditing, setIsEditing] = useState(false);
    const [editData, setEditData] = useState<Partial<PlantDto>>({});

    useEffect(() => {
        fetchPlants();
    }, []);

    async function fetchPlants() {
        try {
            const fileResponse = await plantsClient.getAll();
            const blob = fileResponse.data;
            const text = await blob.text();
            const data = JSON.parse(text) as PlantDto[];
            setPlants(data);
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

                // Optimistically update local plants list
                const updatedPlant: PlantDto = {
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
        <div style={{ padding: "2rem" }}>
            <h1>Choose a Plant</h1>
            <select onChange={handleSelectChange} value={selectedPlant?.id || ""}>
                <option value="">-- Select a plant --</option>
                {plants.map((plant) => (
                    <option key={plant.id} value={plant.id}>
                        {plant.plantName}
                    </option>
                ))}
            </select>

            {selectedPlant && (
                <div style={{ marginTop: "2rem", border: "1px solid #ccc", padding: "1rem" }}>
                    <h2>Plant Details</h2>
                    {isEditing ? (
                        <div>
                            <div>
                                <label>Name: </label>
                                <input
                                    type="text"
                                    name="plantName"
                                    value={editData.plantName || ""}
                                    onChange={handleInputChange}
                                />
                            </div>
                            <div>
                                <label>Type: </label>
                                <input
                                    type="text"
                                    name="plantType"
                                    value={editData.plantType || ""}
                                    onChange={handleInputChange}
                                />
                            </div>
                            <div>
                                <label>Moisture Threshold: </label>
                                <input
                                    type="number"
                                    name="moistureThreshold"
                                    value={editData.moistureThreshold || 0}
                                    onChange={handleInputChange}
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
                            <button onClick={handleSaveClick}>Save</button>
                        </div>
                    ) : (
                        <div>
                            <p><strong>Name:</strong> {selectedPlant.plantName}</p>
                            <p><strong>Type:</strong> {selectedPlant.plantType}</p>
                            <p><strong>Moisture Level:</strong> {selectedPlant.moistureLevel}</p>
                            <p><strong>Moisture Threshold:</strong> {selectedPlant.moistureThreshold}</p>
                            <p><strong>Auto Watering
                                Enabled:</strong> {selectedPlant.isAutoWateringEnabled ? "Yes" : "No"}</p>
                            <div style={{marginTop: "1rem"}}>
                                <button onClick={handleEditClick}>Edit</button>
                                {" "}
                                <button onClick={handleDeleteClick} style={{color: "red"}}>
                                    Delete
                                </button>
                            </div>
                        </div>
                    )}
                </div>
            )}
            <button>Create Plant</button>
        </div>
    );
};

export default HomePage;
