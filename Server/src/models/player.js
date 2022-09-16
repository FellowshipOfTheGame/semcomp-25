const { firebase, db } = require('../loaders/firebase')
const configEnv = require("../config")

class SchemaPlayer {

    async findAll() {
        const pathTable = configEnv.PROJECT_ID + '/player/'
        const PlayerTable = db.ref(pathTable)

        return PlayerTable.get().then((users) => {
            if(users.exists()) {
                return users;
            } else {
                console.log("No data available");
                return null;
            }
            
         }).catch((error) => {
             console.error(error);
         });
    }

    async findOneById(provider_id) {
        const playerTable = configEnv.PROJECT_ID + '/player/' + provider_id

        // find one user
        return db.ref(playerTable).get().then((snapshot) => {
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

    async create(Player) {
        const pathTable = configEnv.PROJECT_ID + '/player/' + Player.provider_id

        db.ref(pathTable).set({
            created_at: firebase.database.ServerValue.TIMESTAMP,
            first_name: Player.first_name,
            surname_name: Player.surname_name,
            email: Player.email,
            provider: Player.provider, 
            provider_id: Player.provider_id,
            games_count: 0,
            is_banned: false
        });

        return Player;
    }
}

class SchemaScore {

    async findAll() {
        const pathTable = configEnv.PROJECT_ID + '/score/'
        const ScoreTable = db.ref(pathTable)
        let topUserScoreListRef = ScoreTable.orderByChild('top_score')

        return topUserScoreListRef.get().then((scores) => {
            if(scores.exists()) {
                return scores;
            } else {
                console.log("No data available");
                return null;
            }
            
         }).catch((error) => {
             console.error(error);
         });
    }

    async findOneById(provider_id) {
        const playerTable = configEnv.PROJECT_ID + '/score/' + provider_id

        // find one user
        return db.ref(playerTable).get().then((snapshot) => {
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

    createOrUpdate(Player) {
        const pathTable = configEnv.PROJECT_ID + '/score/' + Player.provider_id

        db.ref(pathTable).set({
            top_score: Player.top_score,
            top_score_date: firebase.database.ServerValue.TIMESTAMP,
        }).catch((error) => {
            console.error(error);
        });

        return Player
    }
}

module.exports = {
    Player: new SchemaPlayer(),
    Score: new SchemaScore()
}