const { firebase, db } = require('../loaders/firebase')
const configEnv = require("../config");
const { use } = require('passport');

class SchemaPlayer {

    async findAll() {
        const pathTable = configEnv.PROJECT_ID + '/player/'
        const PlayerTable = db.ref(pathTable)

        return PlayerTable.get().then((users) => {
            if(users.exists()) {
                return users;
            } else {
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
                return snapshot.val();
            } else {
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
            is_banned: false
        });

        return Player;
    }
}

class SchemaScore {

    async getRaking() {
        const pathTable = configEnv.PROJECT_ID + '/score/'
        const ScoreTable = db.ref(pathTable)
        let scoreList = [];

        const query = await ScoreTable.orderByChild('top_score').limitToLast(10)
        
        await query.once('value', snapshot => {            
            snapshot.forEach(childSnapshot => {
                const scoreValue = {
                    name: childSnapshot.val().name,
                    score: childSnapshot.val().top_score
                }
                scoreList.push(scoreValue)
            })
        })
    
        return scoreList;
    }

    async getPlayerRaking(userId) {
        const pathTable = configEnv.PROJECT_ID + '/score/'
        const ScoreTable = db.ref(pathTable)
        let personalRank = null;

        if(!userId) {
            return null
        }

        const query = await ScoreTable.orderByChild('top_score')
        
        await query.once('value', snapshot => {       
            let index = 0;

            snapshot.forEach(childSnapshot => {
                if(childSnapshot.key == userId) {
                    personalRank = {
                        name : childSnapshot.val().name,
                        position: snapshot.numChildren() - index,
                        score: childSnapshot.val().top_score
                    }
                }   
                index += 1                 
            })
        })
    
        return personalRank;
    }

    async findOneById(provider_id) {
        const playerTable = configEnv.PROJECT_ID + '/score/' + provider_id

        // find one user
        return db.ref(playerTable).get().then((snapshot) => {
            if (snapshot.exists()) {
                return snapshot.val();
            } else {
                return null;
            }
        }).catch((error) => {
            console.error(error);
        }); 
    }

    createOrUpdate(Player) {
        const pathTable = configEnv.PROJECT_ID + '/score/' + Player.provider_id

        db.ref(pathTable).set({
            name: Player.name,
            top_score: Player.top_score,
            match_id: Player.match_id || 0,
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