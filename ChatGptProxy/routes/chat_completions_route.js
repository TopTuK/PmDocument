import axios from "axios";
import { Configuration, OpenAIApi } from "openai";

import { DEBUG, MODERATION, CHAT_COMPLETIONS_URL } from "../config.js";
import { streamCompletion, generateId, getOpenAIKey } from "../utils.js";

async function chatCompletions(req, res) {
    if (DEBUG) {
        console.log("Start Chat Completions");
    }

    let key = getOpenAIKey(DEBUG); // Get OpenAI Key
    let orgId = generateId(); // Generate organistation ID

    if (DEBUG) {
        console.log(`ChatGPT Key: ${key} DEBUG: ${DEBUG}`);
        console.log(`ChatGPT Org name: ${orgId}`);
        console.log(`Moderation: ${MODERATION}`);
    }

    if (MODERATION) {
        try {
            let prompt = [];

            try {
                req.body.messages.forEach(el => {
                    prompt.push(el.content);
                });

                if (DEBUG) {
                    console.log(`Moderation messages length: ${prompt.length}`);
                }
            }
            catch (e) {
                console.error("Messages is required! and must be an array of objects with content and author properties");

                return res.status(400).send({
                    status: false,
                    error: "Messages is required! and must be an array of objects with content and author properties"
                });
            }

            if (DEBUG) {
                console.log(`Moderation Max-Tokens: ${req.body.max_tokens ?? "unset"}`);
                console.log(`Moderation messages: ${prompt}`);
                console.log("Send promt messages for moderation");
            }

            let openAi = new OpenAIApi(new Configuration({ apiKey: key }));
            let response = await openAi.createModeration({
                input: prompt,
            });

            if (DEBUG) {
                console.log("OpenAI moderation response: ", JSON.stringify(response.data.results));
            }

            if (response.data.results[0].flagged) {
                console.warn("Prompt contains content that is not allowed! Return bad request");
                console.warn(`Moderation reason: ${response.data.results[0].reason}`);

                res.set("Content-Type", "application/json");

                return res.status(400).send({
                    status: false,
                    error: "Your prompt contains content that is not allowed",
                    reason: response.data.results[0].reason,
                });
            }
        }
        catch (error) {
            console.error("Moderation exception: ", JSON.stringify(error));

            return res.status(500).send({
                status: false,
                error: "Something went wrong!"
            });
        }
    }
    else {
        if (DEBUG) {
            console.log(`Moderation is not set. Max-Tokens: ${req.body.max_tokens ?? "unset"}`);
        }
    }

    if (req.body.stream) { // Stream request
        if (DEBUG) {
            console.log("Start proxing stream request");
        }

        try {
            const response = await axios.post(
                CHAT_COMPLETIONS_URL, req.body,
                {
                    responseType: "stream",
                    headers: {
                        Accept: "text/event-stream",
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${key}`,
                    },
                },
            );

            if (DEBUG) {
                console.log("Got stream response from OpenAI");
            }

            res.setHeader("content-type", "text/event-stream");

            for await (const message of streamCompletion(response.data)) {
                try {
                    if (DEBUG) {
                        console.log(`Message response: ${message}`);
                    }

                    const parsed = JSON.parse(message);

                    delete parsed.id;
                    delete parsed.created;

                    const { content } = parsed.choices[0].delta;

                    if (content) {
                        res.write(`data: ${JSON.stringify(parsed)}\n\n`);
                    }
                }
                catch (error) {
                    console.error("Could not JSON parse stream message", message, JSON.stringify(error));
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

                    console.error(`Stream response status: ${error.response.status}. Error: ${JSON.stringify(errorResponseJson)}`);
                    return res.status(error.response.status).send(errorResponseJson);
                }
                else {
                    console.error("Could not JSON parse stream message", error);
                    return res.status(500).send({
                        status: false,
                        error: "Something went wrong!"
                    });
                }
            }
            catch (e) {
                console.error("Exception raised.", JSON.stringify(e));

                return res.status(500).send({
                    status: false,
                    error: "Something went wrong!"
                });
            }
        }
    }
    else { // Common request (not stream)
        if (DEBUG) {
            console.log("Start proxing common request");
        }

        try {
            const response = await axios.post(
                CHAT_COMPLETIONS_URL, req.body,
                {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${key}`,
                    },
                },
            );

            if (DEBUG) {
                console.log(`Got respose from OpenAI -> Proxing`);
            }

            delete response.data.id;
            delete response.data.created;

            return res.status(200).send(response.data);
        }
        catch (error) {
            try {
                error.response.data.error.message = error.response.data.error.message.replace(/org-[a-zA-Z0-9]+/, orgId);

                console.error(`Response error status: ${error.response.status}. Message: `, JSON.stringify(error.response.data));
                return res.status(error.response.status).send(error.response.data);
            }
            catch (e) {
                console.error("Exception raised", JSON.stringify(e));

                return res.status(500).send({
                    status: false,
                    error: "something went wrong!"
                });
            }
        }
    }
}

export default chatCompletions;