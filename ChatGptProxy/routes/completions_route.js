import axios from "axios";
import { Configuration, OpenAIApi } from "openai";

import { DEBUG, MODERATION, COMPLETIONS_URL } from "../config.js";
import { streamCompletion, generateId, getOpenAIKey } from "../utils.js";

async function completions(req, res) {
    if (!req.body.prompt) {
        console.error("No prompt provided. Return bad request.")

        res.set("Content-Type", "application/json");

        return res.status(400).send({
            status: false,
            error: "No prompt provided"
        });
    }

    let key = getOpenAIKey(DEBUG); // Get OpenAI Key
    let orgId = generateId(); // Generate organistation ID

    if (DEBUG) {
        console.log(`ChatGPT Key: ${key}`);
        console.log(`ChatGPT Org name: ${orgId}`);
        console.log(`Moderation: ${MODERATION}`);
        console.log(`Request promts: ${req.body.prompt}`);
        console.log(`Request max-tokens: ${req.body.max_tokens ?? "unset"}`);
    }

    if (MODERATION) {
        if (DEBUG) {
            console.log("Start prompt moderation");
        }

        try {
            let openAi = new OpenAIApi(new Configuration({ apiKey: key }));

            let response = await openAi.createModeration({
                input: req.body.prompt,
            });

            if (DEBUG) {
                console.log(`Moderation response: ${response.data}`);
            }

            if (response.data.results[0].flagged) {
                res.set("Content-Type", "application/json");

                console.warn(`Prompt contains content that is not allowed. Reason: ${response.data.results[0].reason}`);

                return res.status(400).send({
                    status: false,
                    error: "Your prompt contains content that is not allowed",
                    reason: response.data.results[0].reason,
                });
            }
        }
        catch (e) {
            console.error(`Exception raised: ${e}`);
        }
    }

    if (req.body.stream) { // Stream request
        if (DEBUG) {
            console.log("Start proxing stream request");
        }

        try {
            const response = await axios.post(
                COMPLETIONS_URL,
                req.body,
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
                    const parsed = JSON.parse(message);

                    if (DEBUG) {
                        console.log(`Parsed response: ${parsed}`);
                    }

                    delete parsed.id;
                    delete parsed.created;

                    res.write(`data: ${JSON.stringify(parsed)}\n\n`);
                }
                catch (error) {
                    console.error("Could not JSON parse stream message", message, error);
                }
            }

            res.write(`data: [DONE]`);
            res.end();
        }
        catch (error) {
            if (error.response && error.response.data) {
                let errorResponseStr = "";

                for await (const message of error.response.data) {
                    errorResponseStr += message;
                }

                errorResponseStr = errorResponseStr.replace(/org-[a-zA-Z0-9]+/, orgId);
                const errorResponseJson = JSON.parse(errorResponseStr);

                console.error(`Stream response status: ${error.response.status}. Error: ${errorResponseJson}`);
                return res.status(error.response.status).send(errorResponseJson);
            }
            else {
                console.error("Could not JSON parse stream message", error);
                return res.status(500).send({
                    status: false,
                    error: "Exception raised: something went wrong!"
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
                `https://api.openai.com/v1/completions`, req.body,
                {
                    headers: {
                        Accept: "application/json",
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${key}`,
                    },
                },
            );

            if (DEBUG) {
                console.log(`Got respose from OpenAI: ${response.data}`);
            }

            delete response.data.id;
            delete response.data.created;

            return res.status(200).send(response.data);
        }
        catch (error) {
            try {
                error.response.data.error.message = error.response.data.error.message.replace(/org-[a-zA-Z0-9]+/, orgId);

                console.error(`response status: ${error.response.status}. Error: ${errorResponseJson}`);
                return res.status(error.response.status).send(error.response.data);
            }
            catch (e) {
                console.log(`Exception raised: ${e}`);

                return res.status(500).send({
                    status: false,
                    error: "Something went wrong!"
                });
            }
        }
    }
}

export default completions;