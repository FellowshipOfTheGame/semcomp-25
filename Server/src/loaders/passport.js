const passport = require('passport');
let GoogleStrategy = require('passport-google-oauth20').Strategy;
// let FacebookStrategy = require('passport-facebook').Strategy;

const configEnv = require('../config')
// const Player = require('../models/Player');
const PlayerController = require('../controllers/userController');

const { logger } = require('../config/logger');

module.exports = function (passport) {

    passport.serializePlayer(function(player, done){
        done(null, {
            id: player._id,
        });
        // done(null, user);
    });
 
    passport.deserializePlayer(function(obj, done){
        User.findById(obj.id, function(err,user){
            done(err, user);    
        });
        // done(null, obj);
    });

    if(configEnv.GOOGLE_CLIENT_ID !== undefined){
        passport.use(new GoogleStrategy({
                clientID: configEnv.GOOGLE_CLIENT_ID,
                clientSecret: configEnv.GOOGLE_CLIENT_SECRET,
                callbackURL: configEnv.GOOGLE_CALLBACK_URL,
            },
            function(accessToken, refreshToken, profile, done) {
                PlayerController.findOrCreate(profile, (err, player) => {
                    if (err) {
                        logger.error({
                            message: `at Google Login: ${err}`
                        })
                        return done(null, null, { message: "unable to create or find user" })
                    }

                    if (!player) {
                        return done(null, null, { message: "user not created or not found" });
                    }
                    
                    return done(null, player);
                });
            }
        ));
    }
}