const configEnv = require("../config");
const { logger } = require("../config/logger");

const firebase = require('firebase');
const { initializeApp } = require("firebase-admin/app")
const admin = require("firebase-admin");

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
            databaseURL: configEnv.DATABASE_URL,
            credential: admin.credential.cert(serviceAccount),
        }
    
        // Initialize Firebase
        let app;
        
        try {
            if (!admin.apps.length)
                app = initializeApp(firebaseConfig);
            else
               app = admin.app(); // if already initialized, use that one

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
        return admin.database()
    }
}
const fbClient = new FirebaseClient()
const db = fbClient.getFirebaseDatabase();

module.exports = {
    firebase,
    db 
}
