const winston = require('winston');
const {
    createLogger,
    transports
} = require('winston');
const configEnv = require('./index')

const format = winston.format.printf(({ level, message, label }) => {
    return `[${level}] [${new Date().toUTCString()}] ${message}`;
});

let serviceAccount = require("../loaders/serviceAccountKey.json");

const logger = winston.createLogger({
    transports: [
        new winston.transports.Console({
            //format: winston.format.colorize()
        }),
        new winston.transports.File({
            filename: `logs/error${new Date().getTime()}.log`,
            level: 'error',
            maxsize: 2*1024*1024, // 2 MB max size
            maxFiles: 512,        // No more storage then 1 GB
        }),
        new winston.transports.File({
            filename: `logs/info${new Date().getTime()}.log`,
            level: 'info',
            maxsize: 2*1024*1024, // 2 MB max size
            maxFiles: 512,        // No more storage then 1 GB
        }),
    ],   
    format: winston.format.combine(
        winston.format.simple(),
        format
    ), 
})

module.exports = {
    logger,
}