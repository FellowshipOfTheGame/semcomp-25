const { firebase, db } = require('../loaders/firebase')
const configEnv = require("../config")

class SchemaPlayer {

    describe() {
        console.log('this function works...')
    }
    
    async findOne(provider_id, provider) {
        
        const pathTable = configEnv.PROJECT_ID + provider + '/' + provider_id

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
       db.ref(Player.provider + '/' + Player.provider_id).push({
            created_at: firebase.ServerValue.TIMESTAMP,
            first_name: Player.first_name,
            surname_name: Player.surname_name,
            email: Player.email,
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