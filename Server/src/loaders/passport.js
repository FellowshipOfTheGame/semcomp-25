let GoogleStrategy = require('passport-google-oauth20').Strategy;
let FacebookStrategy = require('passport-facebook').Strategy;

const config = require('../config')
const User = require('../models/User');
const UserController = require('../controllers/userController');

const { logger } = require('../config/logger');

module.exports = function (passport) {

    passport.serializeUser(function(user, done){
        done(null, {
            id: user._id,
        });
        // done(null, user);
    });

    passport.deserializeUser(function(obj, done){
        User.findById(obj.id, function(err,user){
            done(err, user);    
        });
        // done(null, obj);
    });

    if(config.GOOGLE_CLIENT_ID !== undefined){
        passport.use(new GoogleStrategy({
                clientID: config.GOOGLE_CLIENT_ID,
                clientSecret: config.GOOGLE_CLIENT_SECRET,
                callbackURL: config.GOOGLE_CALLBACK_URL,
            },
            function(accessToken, refreshToken, profile, done) {
                UserController.findOrCreate(profile, (err, user) => {
                    if (err) {
                        logger.error({
                            message: `at Google Login: ${err}`
                        })
                        return done(null, null, { message: "unable to create or find user" })
                    }

                    if (!user) {
                        return done(null, null, { message: "user not created or not found" });
                    }
                    
                    return done(null, user);
                });
            }
        ));
    }

    if(config.FACEBOOK_CLIENT_ID !== undefined){
        passport.use(new FacebookStrategy({
                clientID: config.FACEBOOK_CLIENT_ID,
                clientSecret: config.FACEBOOK_CLIENT_SECRET,
                callbackURL: config.FACEBOOK_CALLBACK_URL,
                profileFields: ["id", "email", "name"]
            },
            function(accessToken, refreshToken, profile, done) {
                UserController.findOrCreate(profile, (err, user) => {
                    if (err) {
                        logger.error({
                            message: `at Facebook Login: ${err}`
                        })
                        return done(null, null, { message: "unable to create or find user" })
                    }

                    if (!user) {
                        return done(null, null, { message: "user not created or not found" });
                    }
                    
                    return done(null, user);
                });
            }
        ));
    }
}