const { firebase, db } = require('../loaders/firebase')
const configEnv = require("../config")

class SchemaMatch {
    
    async findOneById(provider_id) {
        const pathTable = configEnv.PROJECT_ID + '/match/' + provider_id

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
            console.error(error);
        });
        
    }

    async create(match) {
        const pathTable = configEnv.PROJECT_ID + '/match/' + `${match.userId}/${match.startedAt}`

        db.ref(pathTable).set({
            created_at: firebase.database.ServerValue.TIMESTAMP,
            started_at: match.startedAt,
            finished_at: match.finishedAt,
            score: match.score
        });

        return match;
    }
}

module.exports = {
    match: new SchemaMatch()
}