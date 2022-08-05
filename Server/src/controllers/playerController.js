/*
 * User Controllers : Contains all user endpoints 
 */

const { Player } = require('../models/player');
const configEnv = require("../config")

module.exports = {
    async findOrCreate (googleId, cb) {
        console.log("aqui")
        if (!googleId) {
            return cb({ message: "An provider_id is required" }, null);
        }

        Player.describe();
        Player.find(googleId);
        
        // try {
        //     var user = await User.findOne({ provider_id: passport_user.id, provider: passport_user.provider });
        // } catch (err) {
        //     return cb(err, null);
        // }

        // if (user) {
        //     if (user.isBanned === false)
        //         return cb(null, user);
        //     else
        //         return cb(null, null)
        // }

        try {
            // user = await User.create({
            //     created_at: new Date(),

            //     provider: passport_user.provider,
            //     provider_id: passport_user.id,
            //     name: (passport_user.provider === 'facebook') ? `${passport_user._json?.first_name} ${passport_user._json?.last_name}` : passport_user._json?.name,
            //     email: passport_user._json?.email,
            // });
        } catch (err) {
            return cb(err, null);
        }

        // logger.info({
        //     message: `User ${user._id} created successfully from ${passport_user.provider}`
        // });

        return cb(null, googleId);
    },
}