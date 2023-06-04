const { firebase, db } = require('../loaders/firebase')
const configEnv = require("../config")

class SchemaMatch {
    
    async findOneById(userId) {
        const pathTable = configEnv.PROJECT_ID + '/match/' + `${userId}`

        // find one user
        return db.ref(pathTable).get().then((snapshot) => {
            if (snapshot.exists()) {
                console.log(snapshot.val());
                return snapshot.val();
            } else {
                console.log("No data available");
                return null;
            }
        }).catch((error) => {
            logger.error({
                message: `Could not find match `
            });
        });
    }

    async create(match) {
        const pathTable = configEnv.PROJECT_ID + '/match/' + `${match.userId}/${match.startedAt}`

        db.ref(pathTable).set({
            created_at: firebase.database.ServerValue.TIMESTAMP,
            started_at: match.startedAt,
            finished_at: match.finishedAt,
            score: match.score,
        });

        return match;
    }
}

var schemaMatchHistory = {
     score: function (value) {
       return !isNaN(value) && parseInt(value) == value && value < Number.MAX_SAFE_INTEGER 
     },
}

class SchemaMatchHistory {

    
    async findOneById(userId) {
        const pathTable = configEnv.PROJECT_ID + '/match-history/' + `${userId}`

        // find one user
        return db.ref(pathTable).get().then((snapshot) => {
            if (snapshot.exists()) {
                console.log(snapshot.val());
                return snapshot.val();
            } else {
                console.log("No data available");
                return null;
            }
        }).catch((error) => {
            logger.error({
                message: `Could not find match `
            });
        });
    }

    async createOrUpdate(match) {
        const pathTable = configEnv.PROJECT_ID + '/match-history/' + `${match.userId}`
        
        db.ref(pathTable).set({
            created_at: firebase.database.ServerValue.TIMESTAMP,
            match_id: match.matchId,
            score_history: match.scoreHistory,
            time_history: match.timeHistory,
            rem_time_history: match.remTimeHistory,
            paused_history: match.pausedHistory
        });

        return match;
    }
}

module.exports = {
    match: new SchemaMatch(),
    MatchHistory: new SchemaMatchHistory(),
    schemaMatchHistory,
}