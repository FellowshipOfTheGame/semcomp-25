const passport = require('passport');
let GoogleStrategy = require('passport-google-oauth20').Strategy;
let GoogleTokenStrategy =  require("passport-google-verify-token").Strategy;

// let FacebookStrategy = require('passport-facebook').Strategy;

const configEnv = require('../config')
// const Player = require('../models/Player');
const PlayerController = require('../controllers/playerController');

const { logger } = require('../config/logger');

module.exports = function (passport) {

    passport.serializeUser(function(player, done){
        // done(null, {
        //     id: player._id,
        // });
         // testin return the google profile
        done(null, player);
    });
 
    passport.deserializeUser(function(obj, done){
        // find user in firebase
        // User.findById(obj.id, function(err,user){
        //     done(err, user);    
        // });
        
        // testin return the google profile
        done(null, obj);
    });

    if(configEnv.GOOGLE_CLIENT_ID !== undefined) {
        passport.use(new GoogleStrategy({
                clientID: configEnv.GOOGLE_CLIENT_ID,
                clientSecret: configEnv.GOOGLE_CLIENT_SECRET,
                callbackURL: configEnv.GOOGLE_CALLBACK_URL,
            },
            function(accessToken, refreshToken, params, profile, done) {
                console.log(params)
     
                // PlayerController.findOrCreate(profile, (err, player) => {
                //     if (err) {
                //         logger.error({
                //             message: `at Google Login: ${err}`
                //         })
                //         return done(null, null, { message: "unable to create or find user" })
                //     }

                //     if (!player) {
                //         return done(null, null, { message: "user not created or not found" });
                //     }
                    
                //     return done(null, player);
                // });

                // while testing, just return the google profile 
                console.log(profile);
                console.log("This re: " + refreshToken);
                console.log("This is the accessToken: " + accessToken)
                return done(null, profile);
            }
        ));
    }
        
    if(configEnv.GOOGLE_CLIENT_ID !== undefined) {
        passport.use(new GoogleTokenStrategy({
                clientID: configEnv.GOOGLE_CLIENT_ID,
                // If other clients (such as android / ios apps) also access the google api:
                // audience: [CLIENT_ID_FOR_THE_BACKEND, CLIENT_ID_ANDROID, CLIENT_ID_IOS, CLIENT_ID_SPA]
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