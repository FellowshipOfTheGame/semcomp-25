const { db } = require('../loaders/firebase')
const configEnv = require("../config")

class SchemaPlayer {

    describe() {
        console.log('this function works...')
    }
    
    async find(googleId) {
        db.ref(configEnv.PROJECT_ID + '/players/' + googleId).set({
            username: 'julio35',
            email: 'julio@gmail.com',
            provider_id : googleId
          });
    }

    async create() {
        db.ref('logs/info').set('something')
    }

    async update() {
        db.ref('logs/info').set('something')
    }
}

module.exports = {
    Player: new SchemaPlayer()
}