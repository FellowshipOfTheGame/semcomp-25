const createHmac = require('create-hmac')

// Models
const { Player } = require('../models/player');

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
            console.log("Fail")
            return cb(err, null);
        }

        if (player) {
                console.log("Found player")
            if (player.isBanned === false)
                 return cb(null, player);
             else
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
        } catch (err) {
            console.log("Error create user")
            console.log(err)
            return cb(err, null);
        }

        logger.info({
            message: `Player ${player.provider_id} created successfully from ${provider}`
        });

        return cb(null, player);
    },

    async getInfoWithSession(req, res) { 
        if(!req.user){ 
            return res.status(400).json({ message: "invalid user session" });
        } else { 
            console.log("VALIDE")
            console.log(req.user)
            let userInfo = { 
                message: "ok", 
                name: req.user.first_name + ' ' + req.user.surname_name,
                game_count: req.user.games_count,
                top_score: req.user.top_score,
                top_score_date: req.user.top_score_date,
                sign: ""
            }
            
            userInfo.sign =  createHmac('sha256', configEnv.RESPONSE_SIGNATURE_KEY).update(JSON.stringify(userInfo)).digest('base64')
            return res.json(userInfo)
        }
    }
}