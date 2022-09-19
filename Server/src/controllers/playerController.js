const createHmac = require('create-hmac')

// Models
const { Player, Score } = require('../models/player');

// Config
const configEnv = require("../config")
const { logger } = require('../config/logger');

module.exports = {
    async findOrCreate (provider_id, provider, parsedToken, cb) {
        let player = null;

        if (!provider_id) {
            return cb({ message: "An provider_id is required" }, null);
        }    

        try {
            player = await Player.findOneById(provider_id);
        } catch (err) {
            logger.error({
                message: `Fail to find player.`
            }) 
            return cb(err, null);
        }

        if (player) {
            
            logger.info({
                message: `Player ${player.provider_id} logged successfully from ${provider}`
            });

            if (player.is_banned === false)
                 return cb(null, player);
            
            logger.error({
                message: `Player ${player.provider_id} tried to log in, but is banned.`
            });

            return cb(null, null)
        }

        try {
            player = await Player.create({
                 provider: provider,
                 provider_id: provider_id,
                 email: parsedToken.email,
                 // email_verified: parsedToken.email_verified,
                 first_name: parsedToken.given_name,
                 surname_name: parsedToken.family_name
            });

            Score.createOrUpdate({ provider_id: player.provider_id, name: player.first_name + ' ' + player.surname_name, top_score: 0 })

        } catch (err) {
            logger.error({
                message: `Error create user.`
            }) 
            return cb(err, null);
        }

        logger.info({
            message: `Player ${player.provider_id} created successfully from ${provider}`
        });

        return cb(null, player);
    },

    async getInfoWithSession(req, res) { 

        if(!req.user)
            return res.status(400).json({ message: "invalid user session" })
        
        console.log(req.user)

        let userInfo = { 
            message: "incomplete", 
            name: req.user.first_name + ' ' + req.user.surname_name,
            game_count: 0,
            top_score: 0,
            match_id: 0,
            sign: ""
        }
        let user = undefined

        try {
            user = await Player.findOneById(req.user.provider_id);
            score = await Score.findOneById(req.user.provider_id);

        } catch (err) {
            logger.error({
                message: `at User.show(): failed to find user ${req.user.id}`
            })
            return res.status(500).json({ message: "internal server error" });
        }

        if(user) {
            userInfo = { 
                message: "ok", 
                name: req.user.first_name + ' ' + req.user.surname_name,
                game_count: user.games_count,
                top_score: score.top_score,
                match_id: score.match_id,
                sign: ""
            }
            
        } 
    
        userInfo.sign =  createHmac('sha256', configEnv.RESPONSE_SIGNATURE_KEY).update(JSON.stringify(userInfo)).digest('base64')
        return res.status(200).json(userInfo) 
        
    },
    async getRanking(req, res) { 
        if(!req.user)
            return res.status(400).json({ message: "invalid user session" })
        
        try {
            var allPlayers = await Score.findAll()

        } catch (err) {
            logger.error({
                message: `at User.getRanking(): failed to find user ${req.user.id}`
            })
            console.log(err)
            return res.status(500).json({ message: "internal server error" })
        }
    
        return res.status(200).json(allPlayers) 
    }
}   