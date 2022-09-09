/*
 * User Controllers : Contains all user endpoints 
 */

const { Player } = require('../models/player');
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
}