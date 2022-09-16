const { firebase, db } = require('../loaders/firebase')
const configEnv = require("../config")

class SchemaMatch {
    tableName = '/match/'
    
    async findOneById(provider_id) {
        const pathTable = configEnv.PROJECT_ID + this.tableName + provider_id

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

    // async findHighestScoreByUserId(userId) {
    //     const pathTable = configEnv.PROJECT_ID + this.tableName
    //     db.ref(pathTable). get({
    //         user_id: userId
    //     }).then((snapshot) => {
    //         if (snapshot.exists()) {
    //             console.log(Object.values(snapshot.val()));
    //             return snapshot.val();
    //         } else {
    //             console.log("No data available");
    //             return null;
    //         }
    //     }).catch((error) => {
    //         console.error(error);
    //     });
    // }

    async create(match) {
        const pathTable = configEnv.PROJECT_ID + `${this.tableName}${match.userId}_${match.startedAt}`

        db.ref(pathTable).set({
            created_at: firebase.database.ServerValue.TIMESTAMP,
            user_id: match.userId,
            started_at: match.startedAt,
            finished_at: match.finishedAt,
            score: match.score
        });

        return match;
    }

        async update() {
            db.ref('logs/info').set('something')
        }
}

module.exports = {
    match: new SchemaMatch()
}