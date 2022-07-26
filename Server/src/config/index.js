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
    MEASUREMENT_ID: process.env.MEASUREMENTID
}