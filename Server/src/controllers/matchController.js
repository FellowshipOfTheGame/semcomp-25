const { createHmac } = require("crypto")

const configEnv = require("../config/index")
const { logger } = require('../config/logger')

const { Player, Score } = require('../models/player')
const { match } = require('../models/match')

const { sessionClient } = require('../loaders/redis');

async function index(req, res) {
    return res.status(200).send("Match alive!");
}

async function start(req, res) {
    const userId = req.user.provider_id;
    const startedAt = new Date().getTime();
    
    if(userId === undefined) {
        logger.error(`Failed to start match for user undefined`)
        return res.status(403).end()
    }

    sessionClient.multi()
        .set(`${userId}_match`, startedAt)
        //.expireat(`${userId}_match`, parseInt((+new Date)/1000) + parseInt(configEnv.MATCH_RESPONSE_TIMEOUT))
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
    const userId = req.user.provider_id;
    const score = parseInt(req.body.score);
    const sign  = req.body?.sign?.toString().trim()

    if (score === undefined) return res.status(400).end()

    logger.info(`User ${userId} is trying to finish a match`)

    const reqSign = createHmac('sha256', configEnv.REQUEST_SIGNATURE_KEY).update(JSON.stringify({score, sign: ""})).digest('base64')
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
            const startedAt = parseInt(results[0])
            const hasDeleted = results[1]
            const finishedAt = new Date().getTime()

            if (err) {
                logger.error(`Failed to finish match for user: ${userId}`)
                return res.status(500).end()
            }

            if (!hasDeleted) {
                logger.warn(`Tryed to finish unexisting match for user: ${userId}`)
                return res.status(400).end()
            }
            
            let playerScore = 0

            try {
                playerScore = await Score.findOneById(userId)
                                
                await match.create({
                    userId,
                    score: score,
                    startedAt,
                    finishedAt: finishedAt
                })   

                // update score 
                if(playerScore && score > playerScore.top_score) {
                    playerScore.top_score = score;
                    playerScore.match_id = startedAt;
                    playerScore.provider_id = userId
                    await Score.createOrUpdate(playerScore)
                }
                
            } catch (err) {
                logger.error({
                    message: `Error match finish (firebase - ACCESS DENIED)`,
                });
                return res.status(500).end()
            }
                                        
            return res.status(200).json({top_score: playerScore.top_score})
        });
}

async function savepoint(req, res) {
    const userId = req.user.provider_id;
    const score = req.body.score;
    
    const sign  = req.body?.sign?.toString().trim()

    if (score === undefined) return res.status(400).end()

    logger.info(`User ${userId} is trying to save a match`)

    const reqSign = createHmac('sha256', configEnv.REQUEST_SIGNATURE_KEY).update(JSON.stringify({score, sign: ""})).digest('base64')
    if(sign !== reqSign) {
        logger.warn({
            message: `at match.savepoint(): Invalid signature for user ${userId}`
        })    

        return res.status(400).json({ message: "incorrect signature" })
    }

    // return await sessionClient.multi()
    //    .set(`${userId}_match_savepoint`)

    return await sessionClient.multi()
        .get(`${userId}_match`)
        //.expireat(`${userId}_match`, parseInt((+new Date)/1000) + parseInt(configEnv.MATCH_RESPONSE_TIMEOUT))
        .exec( async (err, results) => {

            const startedAt = parseInt(results[0])

            if (err) {
                logger.error(`Failed to save match for user: ${userId}`)
                return res.status(500).end()
            }

            if (isNaN(startedAt)) {
                logger.warn(`Tryed to save unexisting match for user: ${userId}`)
                return res.status(400).end()
            }
            
            return res.status(200).end()
        });
}

module.exports = {
    index,
    start,
    finish,
    savepoint
}