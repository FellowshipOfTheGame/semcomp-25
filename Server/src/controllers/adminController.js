/*
 * Admin Controllers : Contains all admin endpoints 
 */
const { admin } = require('../models/admin');
const player = require('../models/player');
const { Score } = require('../models/player');
module.exports = {

    async gamesCount(req, res) {
        const gamesForPlayed = await admin.countEachPlayerGames()
        let countGamesList = []

        try {
            for(let i =0; i < gamesForPlayed.length; ++i) {
                // seach name
                const playerInfo = await Score.findOneById(gamesForPlayed[i].playerId)
                countGamesList.push({username: playerInfo.name, top_score: playerInfo.top_score, count_game: gamesForPlayed[i].gameCount})
            }
        } catch(err) {
            res.status(500)
        }

        res.status(200).json({
            message: "Counted games",
            countGame: countGamesList
        });
    }
}