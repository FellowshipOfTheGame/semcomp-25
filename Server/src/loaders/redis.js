const session = require('express-session')
const redis = require('redis')
const IORedis = require("ioredis")
const redisStore = require('connect-redis')(session)
const { logger } = require('../config/logger')

const config = require("../config")

// Redis Database 1 - Sessions Storages
const sessionClient = redis.createClient({
    db: 3,
    host: config.REDIS_HOST,
    port: config.REDIS_PORT,
    legacyMode: true,
})

// try {
//     await sessionClient.connect()
// } catch {
//     logger.error({
//         message: `at Redis[${client.options.db}]: ${err}`
//     })
// }
    
// Redis Clients Logs
const clientList = [ sessionClient ]

clientList.forEach(async client => {
        client.on('connect', () => {
            logger.info({
                message: `at Redis[${client.options.db}]: Frequency connected!`
            })
        })
    
    client.on('error', (err) => {
        logger.error({
            message: `at Redis[${client.options.db}]: ${err}`
        })
    })
    await client.connect().catch(console.error)
})

// Module Exports
module.exports = {
    sessionClient,
    sessionStore: new redisStore({ client: sessionClient, ttl: 3600 }),
    session
}