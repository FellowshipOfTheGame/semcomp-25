// Dependencies
// const Redis = require("ioredis")
const passport = require('passport')
// const { v4: uuidv4, validate: uuidValidate } = require('uuid')

const configEnv = require('../config')
// const { otpClient: redis } = require("../loaders/redis")
const sessionOpts = require('../loaders/session')
const { logger } = require('../config/logger');

// Exporting controller async functions
module.exports = { 
    loginCallback,
    logout,
}

// Controller Functions
async function loginCallback(req, res) { 

    // return res.json({ message: "ok" })
    res.redirect(`${configEnv.SERVER_PATH_PREFIX}/session/login/google-success`)
}

// https://medium.com/swlh/session-management-in-nodejs-using-redis-as-session-store-64186112aa9
async function logout(req, res) { 

    req.session.destroy(function(err) {
        console.log("The session has been destroyed!")
    })

    return res.json({ message: "ok" })
}