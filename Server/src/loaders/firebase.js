const firebase = require('firebase');
const configEnv = require("../config");
const { logger } = require("../config/logger");

// Import app from SDKs
// const { initializeApp } = require("firebase/app");
const { initializeApp } = require("firebase-admin/app")
const admin = require("firebase-admin");
const { get } = require('../routes/session');

/**
 * Database Singleton (using Firebase)
 * Setup database
 */
class FirebaseClient {

    constructor() {
        this._connect()
    }

    _connect() {
        let serviceAccount = require("./serviceAccountKey.json");

        // Web app's Firebase configuration
        const firebaseConfig = {
            databaseURL: configEnv.ADMIN_DATABASEURL,
            credential: admin.credential.cert(serviceAccount),
        }
    
        // Initialize Firebase
        let app;
        
        try {
            if (!firebase.apps.length)
                app = initializeApp(firebaseConfig);
            else
               app = firebase.app(); // if already initialized, use that one

            logger.info({
                message: `at Firebase: connect successful`
            })  
        } catch(err) {   
            logger.error({   
                message: `at Firebase: ${err}`
            })
        }
    } 

    getFirebaseDatabase() {
        return firebase.database()
    }
}
const fbClient = new FirebaseClient()
const db = fbClient.getFirebaseDatabase();

module.exports = {
    firebase,
    db 
}
