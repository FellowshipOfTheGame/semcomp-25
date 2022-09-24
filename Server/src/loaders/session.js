
const config = require('../config')
const redis = require('../loaders/redis')

const sessionOpts = {
    resave: false,
    name: "gameSession",
    saveUninitialized: false,
    cookie: {
        secure: true, 
        httpOnly: config.SESSION_HTTP_ONLY, 
        sameSite: config.SESSION_SAME_SITE, 
        maxAge: 3600000 * 24 * 7
    },
    secret: config.COOKIE_SIGNATURE_KEY,
    resave: false,
    store: redis.sessionStore,
}

module.exports = {
    sessionOpts,
    // Express Session Middleware Configuration
    // Obs: HTTPS behind proxy need to activate `trust proxy` in express  
    // Find about this method in:
    // https://medium.com/swlh/session-management-in-nodejs-using-redis-as-session-store-64186112aa9  
    sessionLoader: () => redis.session(sessionOpts)
}
