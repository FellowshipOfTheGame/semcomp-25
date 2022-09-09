const { Player } = require('../models/player')
const { logger } = require('../config/logger')
const { sessionClient } = require('../loaders/redis')

async function index(req, res) {
    return res.status(200).send("Match alive!");
}

async function start(req, res) {
    const userId = parseInt(req.user.id);
    const startedAt = new Date().toISOString();
    
    sessionClient.multi()
        .set(`${userId}_match`, startedAt)
        .exec( (err, results) => {

            if (err) {
                logger.error(`Failed to start match for user: ${userId}`)
                return res.status(500).end()
            }

            logger.info(`User ${userId} started a match`)
        })

    return res.status(201).end();
}

async function finish(req, res) {
    const userId = parseInt(req.user.id);
    var error;
    var hasDeleted = false;
    logger.info(`User ${userId} is trying to finish a match`)

    return await sessionClient.multi()
        .del(`${userId}_match`)
        .exec( (err, results) => {
            hasDeleted = results[0]
            if (err) {
                logger.error(`Failed to finish match for user: ${userId}`)
                return res.status(500).end()
            }

            if (!hasDeleted) {
                logger.warn(`Tryed to finish unexisting match for user: ${userId}`)
                return res.status(400).end()
            }
            
            return res.status(200).end()
        });
}

module.exports = {
    index,
    start,
    finish
}