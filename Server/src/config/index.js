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
    
    PROJECT_ID: process.env.PROJECT_ID,
    API_KEY: process.env.APIKEY,
    AUTH_DOMAIN: process.env.AUTHDOMAIN,
    DATABASE_URL: process.env.DATABASEURL,
    STORAGE_BUCKET: process.env.STORAGEBUCKET,
    MESSAGING_SENDER_ID: process.env.MESSAGINGSENDERID,
    APP_ID: process.env.APPID,
    MEASUREMENT_ID: process.env.MEASUREMENTID
}