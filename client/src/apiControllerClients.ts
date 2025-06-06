import {AuthClient, PlantsClient, SubscriptionClient, WateringLogClient} from "./generated-client.ts";

const baseUrl = import.meta.env.VITE_API_BASE_URL
const prod = import.meta.env.PROD

export const subscriptionClient = new SubscriptionClient(prod ? "https://" + baseUrl : "http://" + baseUrl);
export const authClient = new AuthClient(prod ? "https://" + baseUrl : "http://" + baseUrl);
export const plantsClient = new PlantsClient(prod ? "https://" + baseUrl : "http://" + baseUrl);
export const wateringLogClient = new WateringLogClient(prod ? "https://" + baseUrl : "http://" + baseUrl);
