const { firebase, db } = require('../loaders/firebase')
const configEnv = require("../config")

class SchemaPlayer {

    describe() {
        console.log('this function works...')
    }
    
    async findOneById(provider_id) {
        const tableName = '/player/'
        const pathTable = configEnv.PROJECT_ID + tableName + provider_id

        // find one user
        db.ref(pathTable).get().then((snapshot) => {
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
        console.log(Player)

        db.ref(pathTable).set({
            created_at: firebase.database.ServerValue.TIMESTAMP,
            first_name: Player.first_name,
            surname_name: Player.surname_name,
            email: Player.email,
            provider: Player.provider, 
            provider_id: Player.provider_id,
            games_count: 0,
            top_score: 0,
            top_score_date: firebase.database.ServerValue.TIMESTAMP,
            is_banned: false
        });
        
        return Player;
    }

        async update() {
        db.ref('logs/info').set('something')
        }
}

module.exports = {
    Player: new SchemaPlayer()
}