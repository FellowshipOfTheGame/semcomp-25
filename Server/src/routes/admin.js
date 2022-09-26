// Dependencies
const express = require('express');
const routes = express.Router();
const { admin } = require('../models/admin')

const configEnv = require('../config')

// Middlewares
const SessionMiddleware = require('../middlewares/Session.middleware');
const adminController = require('../controllers/adminController');
const matchController = require('../controllers/matchController')

// routes.use(SessionMiddleware.isAuth)

routes.delete('/delete/logs', async (req, res) => {
    admin.deleteTable('logs/')
    res.status(200).send("Log deleted!");
})

routes.get('/match/gamecount', adminController.gamesCount)
routes.get('/match/history', adminController.getMatchHistory)

module.exports = routes;