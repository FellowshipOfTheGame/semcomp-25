/*
 * User Controllers : Contains all user endpoints 
 */
// const Player = require('../models/Player');
const configEnv = requere("../config")

module.exports = {
    async findOrCreate (passport_user, cb) {
        if (!passport_user?.id) {
            return cb({ message: "An provider_id is required" }, null);
        }
        
        try {
            var user = await User.findOne({ provider_id: passport_user.id, provider: passport_user.provider });
        } catch (err) {
            return cb(err, null);
        }

        if (user) {
            if (user.isBanned === false)
                return cb(null, user);
            else
                return cb(null, null)
        }

        try {
            user = await User.create({
                created_at: new Date(),

                provider: passport_user.provider,
                provider_id: passport_user.id,
                name: (passport_user.provider === 'facebook') ? `${passport_user._json?.first_name} ${passport_user._json?.last_name}` : passport_user._json?.name,
                email: passport_user._json?.email,
            });
        } catch (err) {
            return cb(err, null);
        }

        logger.info({
            message: `User ${user._id} created successfully from ${passport_user.provider}`
        });

        return cb(null, user);
    },
}