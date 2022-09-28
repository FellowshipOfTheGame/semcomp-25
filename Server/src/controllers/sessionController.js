// Dependencies
const { v4: uuidv4} = require('uuid')
const validate = require('uuid-validate');

const configEnv = require('../config')
const { otpClient: redis } = require("../loaders/redis")
const sessionOpts = require('../loaders/session')
const { logger } = require('../config/logger');

function uuidValidateV4(uuid) {
  return uuidValidate(uuid) && uuidVersion(uuid) === 4;
}

// Exporting controller async functions
module.exports = { 
    loginCallback,
    logout,
    getSession
}

// Controller Functions
async function loginCallback(req, res) { 
    const otpCode = uuidv4()

    redis.set(`otp-${otpCode}`, req.sessionID, "EX", 3*60, (err) => { // Expire in 3 minutes        
        if(err){
            logger.error({
                message: `at Session.loginCallback(): Error in set opt to session ${req.sessionID}`
            })
            return res.redirect(`${configEnv.SERVER_PATH_PREFIX}/?auth=failed`)
        }
        res.redirect(`${configEnv.SERVER_PATH_PREFIX}/codigo-login?code=${otpCode}`)
    }) 
}

async function getSession(req, res) { 
    const otpCode = req.body?.code

    if(!validate(otpCode)) 
        return res.status(400).json({ message: "invalid field @code" })

    redis.multi()
    .get(`otp-${otpCode}`)
    .del(`otp-${otpCode}`)
    .exec((err, results) => { 

        if(err){
            logger.error({
                message: `at Session.getSession: Error in get opt session ${otpCode}`
            })

            return res.status(500).json({ message: "internal server error" })
        }
        
        const sessionID = results[0]
        
        if(sessionID === null) {
            logger.warn({
                message: `[${req.ip}] - at Session.getSession(): Failed to retrieve session from OTP code`
            })
            return res.status(400).json({ message: "otp code expired" })
        }
        
        req.sessionID = sessionID
        req.sessionStore.get(sessionID, function (err, session) {
            if(err || session === undefined)
                return res.status(400).json({ message: "original session expired" })

            req.sessionStore.createSession(req, session);
            return res.json({ message: "ok" })
        })
    })
}

// https://medium.com/swlh/session-management-in-nodejs-using-redis-as-session-store-64186112aa9
async function logout(req, res) { 

    req.session.destroy(function(err) {
        console.log("The session has been destroyed!")
    })

    return res.json({ message: "ok" })
}