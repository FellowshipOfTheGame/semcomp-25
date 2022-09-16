const { match } = require('../models/match')
const config = require("../config/index")
const { createHmac } = require("crypto")
const { Player } = require('../models/player')
const { logger } = require('../config/logger')
const { sessionClient } = require('../loaders/redis');
const { findOrCreate } = require('./playerController');

async function index(req, res) {
    return res.status(200).send("Match alive!");
}

async function start(req, res) {
    const userId = req.user.id;
    const startedAt = new Date().getTime();
    
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
    const userId = req.user.id;
    const score = req.body.score;
    const sign  = req.body?.sign?.toString().trim()

    if (score === undefined) return res.status(400).end()

    logger.info(`User ${userId} is trying to finish a match`)

    const reqSign = createHmac('sha256', config.REQUEST_SIGNATURE_KEY).update(JSON.stringify({score, sign: ""})).digest('base64')
    if(sign !== reqSign) {
        logger.warn({
            message: `at Race.finish(): Invalid signature for user ${userId}`
        })    

        return res.status(400).json({ message: "incorrect signature" })
    }

    return await sessionClient.multi()
        .get(`${userId}_match`)
        .del(`${userId}_match`)
        .exec( async (err, results) => {
            const startedAt = results[0]
            const hasDeleted = results[1]

            if (err) {
                logger.error(`Failed to finish match for user: ${userId}`)
                return res.status(500).end()
            }

            if (!hasDeleted) {
                logger.warn(`Tryed to finish unexisting match for user: ${userId}`)
                return res.status(400).end()
            }

            const topScore = await Player
                    .findOneById(userId)
                    .then((user) => {
                        return user
                    })
            console.log(topScore)
            
            await match.create({
                userId,
                score,
                startedAt,
                finishedAt: new Date().getTime()
            })

            return res.status(200).end()
        });
}

async function savepoint(req, res) {
    const userId = req.user.id;
    const score = req.body.score;
    const sign  = req.body?.sign?.toString().trim()

    if (score === undefined) return res.status(400).end()

    logger.info(`User ${userId} is trying to save a match`)

    const reqSign = createHmac('sha256', config.REQUEST_SIGNATURE_KEY).update(JSON.stringify({score, sign: ""})).digest('base64')
    if(sign !== reqSign) {
        logger.warn({
            message: `at match.savepoint(): Invalid signature for user ${userId}`
        })    

        return res.status(400).json({ message: "incorrect signature" })
    }

    return await sessionClient.multi()
        .get(`${userId}_match`)
        .exec( async (err, results) => {
            const startedAt = results[0]

            if (err) {
                logger.error(`Failed to finish match for user: ${userId}`)
                return res.status(500).end()
            }

            if (startedAt === null) {
                logger.warn(`Tryed to finish unexisting match for user: ${userId}`)
                return res.status(400).end()
            }

            console.log(startedAt)
            
            await match.create({
                userId,
                score,
                startedAt,
                finishedAt: null
            })

            return res.status(200).end()
        });
}

// function getScoreFromRequest(req) {
//     try {
//         return req.body.score;
//     }
//     catch (err) {
//         logger.error(`Unable to parse request body`)
//         return null;
//     }
// }

module.exports = {
    index,
    start,
    finish,
    savepoint
}