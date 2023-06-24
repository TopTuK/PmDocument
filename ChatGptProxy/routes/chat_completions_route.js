import axios from "axios";
import { Configuration, OpenAIApi } from "openai";

import { DEBUG, MODERATION } from "../config.js";
import { streamCompletion, generateId, getOpenAIKey } from "../utils.js";

async function chatCompletions(req, res) {
    let orgId = generateId(); // Generate organistation ID
    let key = getOpenAIKey(DEBUG); // Get OpenAI Key

    if (MODERATION) {
        try {
            let prompt = [];

            try {
                req.body.messages.forEach(el => {
                    prompt.push(el.content);
                });
            }
            catch (e) {
                return res.status(400).send({
                    status: false,
                    error: "Messages is required! and must be an array of objects with content and author properties"
                });
            }

            if (DEBUG) {
                console.log(`[CHAT MESSAGE] [${req.user.data.id}] [${req.user.data.name}] [MAX-TOKENS:${req.body.max_tokens ?? "unset"}] ${prompt}`);
            }

            let openAi = new OpenAIApi(new Configuration({ apiKey: key }));
            let response = await openAi.createModeration({
                input: prompt,
            });

            if (response.data.results[0].flagged) {
                res.set("Content-Type", "application/json");

                return res.status(400).send({
                    status: false,
                    error: "Your prompt contains content that is not allowed",
                    reason: response.data.results[0].reason,
                });
            }
        }
        catch (error) {
            if (DEBUG) {
                console.log(error);
            }

            return res.status(500).send({
                status: false,
                error: "Something went wrong!"
            });
        }
    }
    else {
        if (DEBUG) {
            console.log(`[CHAT] [${req.user.data.id}] [${req.user.data.name}] [MAX-TOKENS:${req.body.max_tokens ?? "unset"}]`);
        }
    }

    if (req.body.stream) { // Stream request
        try {
            const response = await axios.post(
                `https://api.openai.com/v1/chat/completions`, req.body,
                {
                    responseType: "stream",
                    headers: {
                        Accept: "text/event-stream",
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${key}`,
                    },
                },
            );

            res.setHeader("content-type", "text/event-stream");

            for await (const message of streamCompletion(response.data)) {
                try {
                    const parsed = JSON.parse(message);

                    delete parsed.id;
                    delete parsed.created;

                    const { content } = parsed.choices[0].delta;

                    if (content) {
                        res.write(`data: ${JSON.stringify(parsed)}\n\n`);
                    }
                }
                catch (error) {
                    if (DEBUG) {
                        console.error("Could not JSON parse stream message", message, error);
                    }
                }
            }

            res.write(`data: [DONE]`);
            res.end();
        }
        catch (error) {
            try {
                if (error.response && error.response.data) {
                    let errorResponseStr = "";

                    for await (const message of error.response.data) {
                        errorResponseStr += message;
                    }

                    errorResponseStr = errorResponseStr.replace(/org-[a-zA-Z0-9]+/, orgId);
                    const errorResponseJson = JSON.parse(errorResponseStr);
                    return res.status(error.response.status).send(errorResponseJson);
                }
                else {
                    if (DEBUG) {
                        console.error("Could not JSON parse stream message", error);
                    }

                    return res.status(500).send({
                        status: false,
                        error: "Something went wrong!"
                    });
                }
            }
            catch (e) {
                if (DEBUG) { 
                    console.log(e);
                }

                return res.status(500).send({
                    status: false,
                    error: "Something went wrong!"
                });
            }
        }
    }
    else { // Common request (not stream)
        try {
            const response = await axios.post(
                `https://api.openai.com/v1/chat/completions`, req.body,
                {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${key}`,
                    },
                },
            );

            delete response.data.id;
            delete response.data.created;

            return res.status(200).send(response.data);
        }
        catch (error) {
            try {
                error.response.data.error.message = error.response.data.error.message.replace(/org-[a-zA-Z0-9]+/, orgId);
                return res.status(error.response.status).send(error.response.data);
            }
            catch (e) {
                if (DEBUG) {
                    console.log(e);
                }

                return res.status(500).send({
                    status: false,
                    error: "something went wrong!"
                });
            }
        }
    }
}

export default chatCompletions;