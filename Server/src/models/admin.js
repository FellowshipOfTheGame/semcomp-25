const { firebase, db } = require('../loaders/firebase')
const configEnv = require("../config")

class SchemaAdmin {

    async deleteTable(table) {
        const pathTable = configEnv.PROJECT_ID  + '/logs/error'
        console.log(pathTable)
        const adminRef = db.ref(pathTable)

        await adminRef.limitToFirst(50000).get().then(function(snapshot) {
            snapshot.forEach(function(childSnapshot) {
                //remove each child
                console.log("Remove succeeded.")
                adminRef.child(childSnapshot.key).remove();
            });
            return true;
        }).catch(function(error) {
            console.log("Remove failed: " + error.message)
        });
       
        return true;
    }

    async countEachPlayerGames() {
        const pathTable = configEnv.PROJECT_ID  + '/match/'
        const adminRef = db.ref(pathTable)
        const playerList = []

        await adminRef.get().then(function(snapshot) {
            snapshot.forEach(function(childSnapshot) {
                //count each child
                playerList.push({playerId: childSnapshot.key, gameCount: childSnapshot.numChildren()})
            });
            return playerList;
        }).catch(function(error) {
            console.log("Count failed: " + error.message)
        });
       
        return playerList;
    }
     
}

module.exports = {
    admin: new SchemaAdmin()
}