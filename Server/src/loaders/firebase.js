const firebase = require('firebase');
const configEnv = require("../config");
const { logger } = require("../config/logger");
const path = require('path');
const { Firestore } = require('@google-cloud/firestore');

/**
 * Database Singleton (using Firebase)
 * Setup database
 */
class FirestoreClient {

    constructor() {
        this._connect()
    }

    _connect() {
        this.firestore = new Firestore({
            projectId: configEnv.PROJECT_ID,
            keyFilename: path.join(__dirname, './semcomp25-pkey.json')
        });
        
    }
}

module.exports = new FirestoreClient()