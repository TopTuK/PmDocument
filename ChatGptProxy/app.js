import express, { json, urlencoded} from "express";
import { DEBUG, SERVER_PORT, COMPLETIONS_ROUTE, CHAT_COMPLETIONS_ROUTE } from "./config.js";

import corsMiddleware from "./middlewares/cors_middleware.js";
import rateLimitMiddleware from "./middlewares/rate_limit_midleware.js";

import completions from "./routes/completions_route.js";
import chatCompletions from "./routes/chat_completions_route.js";

let app = express();

process.on("uncaughtException", function (err) {
    if (DEBUG) console.error(`Express: Exception raised: ${err}`);
});

// Setup middlewares
app.use(corsMiddleware);
app.use(rateLimitMiddleware);

app.use(json());
app.use(urlencoded({ extended: true }));

// Register routes //
app.all("/", async function (req, res) {
    res.set("Content-Type", "application/json");

    return res.status(200).send({
        status: true,
        name: "S ChatGPT Proxy service",
    });
});

// Register chat routes
app.post(COMPLETIONS_ROUTE, completions);
app.post(CHAT_COMPLETIONS_ROUTE, chatCompletions);

// Start server
app.listen(SERVER_PORT, () => {
    console.log(`S ChatGPT now is listening on ${SERVER_PORT} ...`);
});