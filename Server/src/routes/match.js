// Dependencies
const express = require('express');
const routes = express.Router();
const controller = require('../controllers/matchController')
const { Player } = require('../models/player')

const configEnv = require('../config')

// Middlewares
const SessionMiddleware = require('../middlewares/Session.middleware');
const redis = require('../loaders/redis');

routes.get('/', SessionMiddleware.isAuth, (req, res) => {
    res.status(200).send("Match alive!");
})

routes.post('/start', SessionMiddleware.isAuth, controller.start)
routes.post('/finish', SessionMiddleware.isAuth, controller.finish)

module.exports = routes;