import { toBoolean } from "./utils.js";

// Set app name and version
export const APP_NAME = "SChatGpt Proxy";
export const VERSION = "0.9.9";

// SET DEBUG
export const DEBUG = toBoolean(process.env.DEBUG_MODE); // Debug mode

// Server configuration
export const SERVER_PORT = process.env.SERVER_PORT; // Server port

// Rate limit
export const RATE_PERIOD = process.env.RATE_PERIOD; // 15 seconds
export const RATE_LIMIT = process.env.RATE_LIMIT; // 50 requests per 15 seconds

// Completions URLS
export const COMPLETIONS_URL = "https://api.openai.com/v1/completions";
export const CHAT_COMPLETIONS_URL = "https://api.openai.com/v1/chat/completions";

// Whitelisted IPs
export const USE_WHITELISTED_IPS = toBoolean(process.env.USE_WHITELISTED_IPS);
export const WHITELISTED_IPS = process.env.WHITELISTED_IPS.split(',');
/*export const WHITELISTED_IPS = [
    "127.0.0.1",
    "::1",
];*/

// Prompt Moderation before sending to OpenAI
export const MODERATION = toBoolean(process.env.MODERATION); // Moderation mode

// OpenAI API Keys
export let OPENAI_KEY = process.env.OPENAI_KEY;
export let OPENAI_KEYS = process.env.OPENAI_KEYS.split(',');

export let COMPLETIONS_ROUTE = "/v1/completions";
export let CHAT_COMPLETIONS_ROUTE = "/v1/chat/completions";