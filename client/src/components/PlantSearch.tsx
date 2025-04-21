import { useState } from "react";
import { searchPlants } from "../trefleClient";

export default function PlantSearch() {
    const [query, setQuery] = useState("");
    const [results, setResults] = useState<any[]>([]);
    const [loading, setLoading] = useState(false);

    const handleSearch = async () => {
        setLoading(true);
        try {
            const data = await searchPlants(query);
            setResults(data);
        } catch (error) {
            console.error("Error fetching plant data:", error);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="p-4">
            <input
                type="text"
                placeholder="Search plant..."
                value={query}
                onChange={(e) => setQuery(e.target.value)}
                className="input input-bordered"
            />
            <button className="btn btn-primary ml-2" onClick={handleSearch}>
                Search
            </button>

            {loading && <p>Loading...</p>}

            <div className="grid gap-4 mt-4">
                {results.map((plant) => (
                    <div key={plant.id} className="card shadow p-4">
                        <h2 className="text-lg font-bold">{plant.common_name || plant.scientific_name}</h2>
                        {plant.image_url && (
                            <img
                                src={plant.image_url}
                                alt={plant.common_name}
                                className="w-full h-48 object-cover mt-2"
                            />
                        )}
                        <p className="text-sm text-gray-500">{plant.scientific_name}</p>
                    </div>
                ))}
            </div>
        </div>
    );
}
