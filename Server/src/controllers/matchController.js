const { createHmac } = require("crypto")

const configEnv = require("../config/index")
const { logger } = require('../config/logger')

// Models
const { Player, Score } = require('../models/player')
const { match, MatchHistory } = require('../models/match')

const { matchClient } = require('../loaders/redis');
const { unwatchFile } = require("fs")

async function index(req, res) {
    return res.status(200).send("Match alive!");
}

async function start(req, res) {
    const userId = req.user.provider_id;
    const remTime = req.body.rem_time;
    const startedAt = new Date().getTime();
    
    if(userId === undefined) {
        logger.error(`Failed to start match for user undefined`)
        return res.status(403).end()
    }

    if(remTime === undefined) {
        logger.error(`Failed to start match for remaning time undefined`)
        return res.status(403).end()
    }

    matchClient.multi()
        .set(`${userId}_match`, startedAt)
        //.expireat(`${userId}_match`, parseInt((+new Date)/1000) + parseInt(configEnv.MATCH_RESPONSE_TIMEOUT))
        .del(`${userId}_scores`)
        .rPush(`${userId}_scores`, ['0'])
        .del(`${userId}_times`)
        .rPush(`${userId}_times`, ['0'])
        .del(`${userId}_rem_time`)
        .rPush(`${userId}_rem_time`, [String(remTime)])
        .del(`${userId}_paused`)
        .rPush(`${userId}_paused`, ['0'])
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
    const userId = req.user.provider_id

    const matchInfo = {
        score: parseInt(req.body.score),
        rem_time: req.body.rem_time,
        sign: ""
    }

    const sign  = req.body?.sign?.toString().trim()

    if (userId === undefined || matchInfo.score === undefined) return res.status(400).end()

    logger.info(`User ${userId} is trying to finish a match`)

    // const reqSign = createHmac('sha256', configEnv.REQUEST_SIGNATURE_KEY).update(JSON.stringify(matchInfo)).digest('base64')
    // if(sign !== reqSign) {
    //     logger.warn({
    //         message: `at Race.finish(): Invalid signature for user ${userId}`
    //     })    
    //     return res.status(400).json({ message: "incorrect signature" })
    // }

    matchInfo.time = new Date().getTime()
    matchInfo.is_paused = '0'

    return await matchClient.multi()
        .get(`${userId}_match`)
        .rPush(`${userId}_scores`, [String(matchInfo.score)])
        .rPush(`${userId}_times`, [matchInfo.time])
        .rPush(`${userId}_rem_time`, [String(matchInfo.rem_time)])
        .rPush(`${userId}_paused`, [String(matchInfo.is_paused)])
        .lRange(`${userId}_scores`, 0, -1)
        .lRange(`${userId}_times`, 0, -1)
        .lRange(`${userId}_rem_time`, 0, -1)
        .lRange(`${userId}_paused`, 0, -1)
        .del(`${userId}_match`)
        .del(`${userId}_times`)
        .del(`${userId}_rem_time`)
        .del(`${userId}_scores`)
        .del(`${userId}_paused`)
        .exec( async (err, results) => {
            const startedAt = parseInt(results[0])
            const finishedAt = new Date().getTime()
            const scoreHistory = results[5]
            const timeHistory = results[6]
            const remTimeHistory = results[7]
            const pausedHistory = results[8]

            if (err) {
                logger.error(`Failed to finish match for user: ${userId}`)
                return res.status(500).end()
            }

            // Check if the results was successful deleted
            if (!results[2] || !results[13] || !results[10] || !results[11] || !results[12]) {
                logger.warn(`Tryed to finish unexisting match for user: ${userId}`)
                return res.status(400).end()
            }
            
            const matchHistory = {
                userId: userId,
                matchId: startedAt,
                scoreHistory: scoreHistory,
                timeHistory: timeHistory,
                remTimeHistory: remTimeHistory,
                pausedHistory: pausedHistory
            }

            let playerScore = 0

            try {
                playerScore = await Score.findOneById(userId)
                                
                await match.create({
                    userId,
                    score: matchInfo.score,
                    startedAt,
                    finishedAt: finishedAt,
                })   

                // update score 
                if(playerScore && matchInfo.score > playerScore.top_score) {
                    playerScore.top_score = matchInfo.score;
                    playerScore.match_id = startedAt;
                    playerScore.provider_id = userId
                    await Score.createOrUpdate(playerScore)
                    await MatchHistory.createOrUpdate(matchHistory)
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
    
    const userId = req.user.provider_id
    
    const matchInfo = {
        score: parseInt(req.body.score),
        rem_time: req.body.rem_time,
        is_paused: (req.body.is_paused === false)? '0' : '1',
        sign: ""
    }

    const sign  = req.body?.sign?.toString().trim()
    
    if (userId === undefined || matchInfo.score === undefined || 
        (matchInfo.is_paused != '0' && matchInfo.is_paused != '1')) 
        return res.status(400).end()

    logger.info(`User ${userId} is trying to save a match`)

    // const reqSign = createHmac('sha256', configEnv.REQUEST_SIGNATURE_KEY).update(JSON.stringify(matchInfo)).digest('base64')
    // if(sign !== reqSign) {
    //     logger.warn({
    //         message: `at match.savepoint(): Invalid signature for user ${userId}`
    //     })    

    //     return res.status(400).json({ message: "incorrect signature" })
    // }

    matchInfo.time = new Date().getTime()

    return await matchClient.multi()
        .get(`${userId}_match`)
        .rPush(`${userId}_scores`, [String(matchInfo.score)])
        .rPush(`${userId}_times`, [matchInfo.time])
        .rPush(`${userId}_rem_time`, [matchInfo.rem_time])
        .rPush(`${userId}_paused`, [String(matchInfo.is_paused)])
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