var redis   = require("redis");
var session = require('express-session');
const config = require("../config")
const { logger } = require("../config/logger")

var redisStore = require('connect-redis')(session);

// Redis Database 1 - Sessions Storages
const sessionClient  = redis.createClient({
    db: 1,
    host: config.REDIS_HOST,
    port: config.REDIS_PORT,
    legacyMode: true,
})

// Redis Database 2 - OTP Codes Storages
const otpClient = redis.createClient({ 
    db: 2,
    host: config.REDIS_HOST,
    port: config.REDIS_PORT, 
})


// Redis Clients Logs
const clientList = [ sessionClient, otpClient ]

clientList.forEach(async instanceClient => {
    instanceClient.on('connect', () => {
        logger.info({
            message: `at Redis: Frequency connected!`
        })
    })

    instanceClient.on('error', (err) => {
        logger.error({
            message: `at Redis: ${err}`
        })
    })
    await instanceClient.connect().catch(console.error)
    
})

//Configure redis client
module.exports = {
    sessionClient,
    otpClient,
    sessionStore: new redisStore({ client: sessionClient, ttl: 3600 }),
    session
}