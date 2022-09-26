/*
 * Admin Controllers : Contains all admin endpoints 
 */

// models
const { StorageType } = require('winston-firebase-transport');
const { admin } = require('../models/admin');
const { MatchHistory } = require('../models/match');
const player = require('../models/player');
const { Score } = require('../models/player');

const url = require('url');

module.exports = {

    async gamesCount(req, res) {
        const gamesForPlayed = await admin.countEachPlayerGames()
        const playersCount = await admin.countPlayer()

        let countGamesList = []

        try {
            for(let i =0; i < gamesForPlayed.length; ++i) {
                // seach name
                const playerInfo = await Score.findOneById(gamesForPlayed[i].playerId)
                countGamesList.push({username: playerInfo.name, top_score: playerInfo.top_score, playerId: gamesForPlayed[i].playerId, count_game: gamesForPlayed[i].gameCount})
            }
        } catch(err) {
            res.status(500)
        }

        res.status(200).json({
            message: "Counted games",
            countPlayers: playersCount,
            countGames: countGamesList
        });
    },

    async getMatchHistory(req, res) {
        const queryObject = url.parse(req.url, true).query;
        console.log(queryObject);
        if(queryObject.id === undefined) {
            res.status(400).end()
        }
        
        const matchInfo = await MatchHistory.findOneById(queryObject.id);
        
        res.status(200).json(matchInfo)
    }
}