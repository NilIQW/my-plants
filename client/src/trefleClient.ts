// src/trefleClient.ts
import axios from "axios";

/**
 * Search plants via backend proxy (which calls Trefle API).
 * @param query Plant name or partial name
 * @returns Array of plant objects
 */
export async function searchPlants(query: string) {
    try {
        const response = await axios.get(`http://localhost:8080/api/plants/search?q=${encodeURIComponent(query)}`);

        // Make sure your backend returns the data in this format
        // Adjust if needed
        return response.data.data;
    } catch (error) {
        console.error("Error searching for plants:", error);
        throw error;
    }
}
