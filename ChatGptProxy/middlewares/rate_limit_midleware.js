import { USE_WHITELISTED_IPS, WHITELISTED_IPS, RATE_LIMIT, RATE_PERIOD, DEBUG } from '../config.js';

const rateLimit = new Map();

async function rateLimitMiddleware(req, res, next) {
    let ip = req.headers["CF-Connecting-IP"]
        ?? req.headers["cf-connecting-ip"]
        ?? req.headers["X-Forwarded-For"]
        ?? req.headers["x-forwarded-for"] 
        ?? req.ip;
    
    if (DEBUG) {
        console.log(`Client request from ${ip}`);
        console.log(`Use whitelist ip ${USE_WHITELISTED_IPS}`);
    }

    if ((true === USE_WHITELISTED_IPS) && (!WHITELISTED_IPS.includes(ip))) {
        console.warn(`Whitelist of ip adresses doesn\'t containt ${ip}. Return bad request.`);

        return res.status(429).send({
            status: false,
            error: `Bad IP address: ${ip}`
        });
    }
        
    if (!rateLimit.has(ip)) {
        if (DEBUG) {
            console.log(`Set rate limit for ip ${ip}`);
        }

        rateLimit.set(ip, {
            requests: 1,
            lastRequestTime: Date.now(),
        });
    }
    else { //
        if (DEBUG) {
            console.log(`Fount ${ip} in rate limit map.`);
        }

        const currentTime = Date.now();
        const timeSinceLastRequest = currentTime - rateLimit.get(ip).lastRequestTime;
        const isTimeDelta = timeSinceLastRequest > RATE_PERIOD;

        if (DEBUG) {
            console.log(`For ${ip} timeSinceLastRequest > RATE_PERIOD: ${isTimeDelta}`);
        }

        if (isTimeDelta) {
            if (DEBUG) {
                console.log(`Set first request for ip ${ip}. Current time: ${currentTime}`);
            }

            rateLimit.set(ip, {
                requests: 1,
                lastRequestTime: currentTime
            });
        }
        else {
            let requestsCount = rateLimit.get(ip).requests + 1;

            if (DEBUG) {
                console.log(`Increment request count for ${ip}. Total requests: ${requestsCount}`);
            }

            if (requestsCount > RATE_LIMIT) {
                if (DEBUG) {
                    console.warn(`For ip ${ip} total requests (${requestsCount}) > RATE_LIMIT. Returning bad request`);
                }

                return res.status(429).send({
                    status: false,
                    error: "Too many requests, please try again later",
                });
            }

            rateLimit.set(ip, {
                requests: requestsCount,
                lastRequestTime: rateLimit.get(ip).lastRequestTime
            });
        }
    }

    next();
}

export default rateLimitMiddleware;