/*
 *  Envriment vars 
 */
const dotenv = require('dotenv')

dotenv.config()

module.exports = {
    
    SERVER_TRUST_PROXY: process.env.SERVER_TRUST_PROXY == "1" || false,
    SERVER_PORT: process.env.SERVER_PORT || 3000,
    SERVER_HOST: process.env.SERVER_HOST || 'localhost',
    SERVER_PATH_PREFIX: process.env.SERVER_PATH_PREFIX || '',
    
    GOOGLE_CLIENT_ID: process.env.GOOGLE_CLIENT_ID || undefined,
    GOOGLE_CLIENT_SECRET: process.env.GOOGLE_CLIENT_SECRET|| undefined,
    GOOGLE_CALLBACK_URL: process.env.GOOGLE_CALLBACK_URL ||  "http://localhost:3000/session/login/callback",

    PROJECT_ID: process.env.PROJECT_ID,
    API_KEY: process.env.APIKEY,
    AUTH_DOMAIN: process.env.AUTHDOMAIN,
    DATABASE_URL: process.env.DATABASEURL,
    STORAGE_BUCKET: process.env.STORAGEBUCKET,
    MESSAGING_SENDER_ID: process.env.MESSAGINGSENDERID,
    APP_ID: process.env.APPID,
    MEASUREMENT_ID: process.env.MEASUREMENTID,

    REDIS_HOST: process.env.REDIS_HOST || "localhost",
    REDIS_PORT: process.env.REDIS_PORT ||  6379,

    COOKIE_SIGNATURE_KEY: process.env.COOKIE_SIGNATURE_KEY || "ABCDEFGHIJKLU",
    SESSION_SECURE: process.env.SESSION_SECURE == "1" || false, 
    SESSION_HTTP_ONLY: process.env.SESSION_HTTP_ONLY == "1" || false,
    SESSION_SAME_SITE: process.env.SESSION_SAME_SITE || "none",
    SESSION_MAX_AGE: process.env.SESSION_MAX_AGE || 3600000,
    
    REQUEST_SIGNATURE_KEY: process.env.REQUEST_SIGNATURE_KEY || "MINAHSENHASUPERSECRETA",
    RESPONSE_SIGNATURE_KEY: process.env.RESPONSE_SIGNATURE_KEY || "MINAHSENHAHYPERSECRETA",
}