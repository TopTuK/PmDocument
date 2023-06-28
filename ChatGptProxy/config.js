// Set app name and version
export const APP_NAME = "SChatGpt Proxy";
export const VERSION = 0.9;

// SET DEBUG
export const DEBUG = true; // Debug mode

// Server configuration
export const SERVER_PORT = 3000; // Server port

// Rate limit
export const RATE_PERIOD = 15 * 1000; // 15 seconds
export const RATE_LIMIT = 50; // 50 requests per 15 seconds

// Completions URLS
export const CHAT_COMPLETIONS_URL = "https://api.openai.com/v1/chat/completions";

// Whitelisted IPs
export const WHITELISTED_IPS = [
    "127.0.0.1",
    "::1",
    "51.250.108.29",
];

// Prompt Moderation before sending to OpenAI
export const MODERATION = true; // Moderation mode

// OpenAI API Keys
export let OPENAI_KEY = "sk-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
export let OPENAI_KEYS = [
    "sk-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
    "sk-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
    "sk-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
    "sk-xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
];

export let COMPLETIONS_ROUTE = "/v1/completions";
export let CHAT_COMPLETIONS_ROUTE = "/v1/chat/completions";