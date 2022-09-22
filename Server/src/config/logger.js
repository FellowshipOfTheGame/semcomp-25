const winston = require('winston');
const {
    createLogger,
    transports
} = require('winston');
const { FirebaseTransport, StorageType } = require('winston-firebase-transport');
const admin = require("firebase-admin");
const configEnv = require('./index')

const format = winston.format.printf(({ level, message, label }) => {
    return `[${level}] [${new Date().toUTCString()}] ${message}`;
});

let serviceAccount = require("../loaders/serviceAccountKey.json");

// Web app's Firebase configuration
const firebaseConfig = {
    databaseURL: configEnv.ADMIN_DATABASEURL,
    credential: admin.credential.cert(serviceAccount),
}

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
        new FirebaseTransport({ 
     
        
			firebaseConfig: firebaseConfig,     
			logger: {
				level: 'info',
                format: winston.format.combine(
                    winston.format.json(),
                    format 
                ),   
                options: {
                    useUnifiedTopology: true
                },
			},     
			applicationName: configEnv.PROJECT_ID,
			collectionName: 'logs',
			storageType: StorageType.Realtime,
		}), 
    ],   
    format: winston.format.combine(
        winston.format.simple(),
        format
    ), 
})

const raceLogger = winston.createLogger({
//     transports: [
//         new winston.transports.Console({
//             format: winston.format.colorize()
//         }),
//         new winston.transports.File({
//             filename: `logs/race${new Date().getTime()}.log`,
//             level: 'info',
//             maxsize: 2*1024*1024, // 2 MB max size
//             maxFiles: 512,        // No more storage then 1 GB
//         }),
//         new FirebaseTransport({
// 			firebaseConfig: {
//                 apiKey: configEnv.API_KEY,
//                 authDomain: configEnv.AUTH_DOMAIN,
//                 databaseURL: configEnv.DATABASE_URL,
//                 projectId: configEnv.PROJECT_ID,
//                 storageBucket: configEnv.STORAGE_BUCKET,
//                 messagingSenderId: configEnv.MESSAGING_SENDER_ID,
//                 appId: configEnv.APP_ID,
//                 measurementId: configEnv.MEASUREMENT_ID
//             },    
// 			logger: {
// 				level: 'error',
// 			},
// 			applicationName: 'test2',
// 			collectionName: 'race',
// 			storageType: 'realtime',
// 		}),

//     ],
//     format: winston.format.combine(
//         // winston.format.colorize(),
//         winston.format.simple(),
//         format
//     ),
});

module.exports = {
    logger,
    raceLogger,
}