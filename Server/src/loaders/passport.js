const passport = require('passport');
let GoogleStrategy = require('passport-google-oauth20').Strategy;
let GoogleTokenStrategy =  require("./../lib/passport-google-verify-token").Strategy;

// let FacebookStrategy = require('passport-facebook').Strategy;

const configEnv = require('../config')
const { Player } = require('../models/player');
const PlayerController = require('../controllers/playerController');

const { logger } = require('../config/logger');

module.exports = function (passport) {

    passport.serializeUser(function(player, done){
        done(null, player);
    });
 
    passport.deserializeUser(function(obj, done){
        done(null, obj);
    });

    if(configEnv.GOOGLE_CLIENT_ID !== undefined) {
        passport.use(new GoogleStrategy({
                clientID: configEnv.GOOGLE_CLIENT_ID,
                clientSecret: configEnv.GOOGLE_CLIENT_SECRET,
                callbackURL: configEnv.GOOGLE_CALLBACK_URL,
            },
            function(accessToken, refreshToken, params, profile, done) {
                const provider = 'google'

                PlayerController.findOrCreate(profile.id, provider, profile._json, (err, player) => {
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
        
    if(configEnv.GOOGLE_CLIENT_ID !== undefined) {
        passport.use(new GoogleTokenStrategy({
                clientID: configEnv.GOOGLE_CLIENT_ID,
                // If other clients (such as android / ios apps) also access the google api:
                // audience: [CLIENT_ID_FOR_THE_BACKEND, CLIENT_ID_ANDROID, CLIENT_ID_IOS, CLIENT_ID_SPA]
                audience: [configEnv.GOOGLE_CLIENT_ID_ANDROID],
                clientSecret: configEnv.GOOGLE_CLIENT_SECRET,
                // getGoogleCerts: optionalCustomGetGoogleCerts
            },
            function(parsedToken, googleId, done) {
                const provider = 'google'

                PlayerController.findOrCreate(googleId, provider, parsedToken, (err, player) => {
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