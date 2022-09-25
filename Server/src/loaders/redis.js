var redis   = require("redis");
var session = require('express-session');
const config = require("../config")
const { logger } = require("../config/logger")

var redisStore = require('connect-redis')(session)

// Redis Database 1 - Sessions Storages
const sessionClient  = redis.createClient({
    db: 1,
    url: config.REDIS_HOST,
    port: config.REDIS_PORT,
    legacyMode: true,
})

// Redis Database 2 - OTP Codes Storages
const otpClient = redis.createClient({ 
    db: 2,
    url: config.REDIS_HOST,
    port: config.REDIS_PORT, 
    legacyMode: true,
})


// Redis Database 3 - Match Storages
const matchClient = redis.createClient({ 
    db: 3,
    url: config.REDIS_HOST,
    port: config.REDIS_PORT, 
    legacyMode: true,
})


// Redis Clients Logs
const clientList = [ sessionClient, otpClient, matchClient ]

clientList.forEach(async instanceClient => {
    instanceClient.on('connect', () => {
        logger.info({
            message: `at Redis[${instanceClient.options.db}]: Frequency connected!`
        })
    })

    instanceClient.on('error', (err) => {
        logger.error({
            message: `at Redis[${instanceClient.options.db}]: ${err}`
        })
    })
    await instanceClient.connect().catch(console.error)
    
})

//Configure redis client
module.exports = {
    sessionClient,
    sessionStore: new redisStore({ client: sessionClient, ttl: 3600 }),
    otpClient,
    matchClient,
    session
}