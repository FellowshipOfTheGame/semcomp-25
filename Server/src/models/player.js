const { db } = require('../loaders/firebase')
const configEnv = require("../config")

class SchemaPlayer {

    describe() {
        console.log('this function works...')
    }
    
    async findOne(provider_id, provider) {
        db.ref(provider + '/' + provider_id).get().then((snapshot) => {
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
       db.ref(Player.provider + '/' + Player.provider_id).set({
            first_name: Player.first_name,
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