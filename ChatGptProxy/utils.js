import { OPENAI_KEY, OPENAI_KEYS } from "./config.js";

function toBoolean(value) {
    const bools = ["true", "yes", "y"];

    value = value.toString();
    value = value.trim();
    value = value.toLowerCase();

    // Empty string is considered a falsy value
    if (!value.length) {
        return false;
    }
    else if (!isNaN(Number(value))) {
        return value > 0; // Any number above zero is considered a truthy value
    }
    else {
        return bools.indexOf(value) >= 0; // Any value not marked as a truthy value is automatically falsy
    }
}

async function* chunksToLines(chunksAsync) {
    let previous = "";

    for await (const chunk of chunksAsync) {
        const bufferChunk = Buffer.isBuffer(chunk) ? chunk : Buffer.from(chunk);
        previous += bufferChunk;

        let eolIndex;
        while ((eolIndex = previous.indexOf("\n")) >= 0) {
            // line includes the EOL
            const line = previous.slice(0, eolIndex + 1).trimEnd();

            if (line === "data: [DONE]") break;
            if (line.startsWith("data: ")) yield line;

            previous = previous.slice(eolIndex + 1);
        }
    }
}

async function* linesToMessages(linesAsync) {
    for await (const line of linesAsync) {
        const message = line.substring("data :".length);

        yield message;
    }
}

async function* streamCompletion(data) {
    yield* linesToMessages(chunksToLines(data));
}

function generateId() {
    const chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    let id = "org-";

    for (let i = 0; i < 24; i++) {
        id += chars.charAt(Math.floor(Math.random() * chars.length));
    }

    return id;
}

function getOpenAIKey(mainKey) {
    return (mainKey)
        ? OPENAI_KEY
        : OPENAI_KEYS[Math.floor(Math.random() * OPENAI_KEYS.length)];
}

export { toBoolean, generateId, getOpenAIKey, streamCompletion }