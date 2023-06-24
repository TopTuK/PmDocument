import { WHITELISTED_IPS, RATE_LIMIT, RATE_PERIOD, DEBUG } from '../config.js';

const rateLimit = new Map();

async function rateLimitMiddleware(req, res, next) {
    let ip = req.headers["CF-Connecting-IP"]
        ?? req.headers["cf-connecting-ip"]
        ?? req.headers["X-Forwarded-For"]
        ?? req.headers["x-forwarded-for"] 
        ?? req.ip;
    
    if (DEBUG) {
        console.log(`Get request from ${ip}`);
    }
    
    if (!WHITELISTED_IPS.includes(ip)) {
        return res.status(429).send({
            status: false,
            error: `Bad IP address: ${ip}`
        });
    }
        
    if (!rateLimit.has(ip)) {
        rateLimit.set(ip, {
            requests: 1,
            lastRequestTime: Date.now(),
        });
    }
    else { // 
        const currentTime = Date.now();
        const timeSinceLastRequest = currentTime - rateLimit.get(ip).lastRequestTime;

        if (timeSinceLastRequest > RATE_PERIOD) {
            rateLimit.set(ip, {
                requests: 1,
                lastRequestTime: currentTime
            });
        }
        else {
            let requestsCount = rateLimit.get(ip).requests + 1;

            if (requestsCount > RATE_LIMIT) {
                return res.status(429).send({
                    status: false,
                    error: "Too many requests, please try again later"
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