const firebase = require('firebase');
const configEnv = require("../config");
const { logger } = require("../config/logger");

// Import app from SDKs
const { initializeApp } = require("firebase/app");

/**
 * Database Singleton (using Firebase)
 * Setup database
 */
class FirebaseClient {

    constructor() {
        this._connect()
    }

    _connect() {
        // Web app's Firebase configuration
        const firebaseConfig = {
            apiKey: configEnv.API_KEY,
            authDomain: configEnv.AUTH_DOMAIN,
            databaseURL: configEnv.DATABASE_URL,
            projectId: configEnv.PROJECT_ID,
            storageBucket: configEnv.STORAGE_BUCKET,
            messagingSenderId: configEnv.MESSAGING_SENDER_ID,
            appId: configEnv.APP_ID,
            measurementId: configEnv.MEASUREMENT_ID
        }
    
        // Initialize Firebase
        let app;
        
        if (!firebase.apps.length) {
            app = initializeApp(firebaseConfig);
         }else {
            app = firebase.app(); // if already initialized, use that one
         }

        // .(config.MONGO_CONNECT_URL,{
        //     useNewUrlParser: true,
        //     useUnifiedTopology: true,
        //     useCreateIndex: true,
        // })
        // .then(() => {
        //     logger.info({
        //         message: `at Firebase: ${config.MONGO_CONNECT_URL} connect successful`
        //     })
        // })
        // .catch(err => {
        //     logger.error({
        //         message: `at Firebase: ${err}`
        //     })
        // })
      
    }
}

module.exports = new FirebaseClient()

