// Dependencies
const express = require('express');
const routes = express.Router();
const matchController = require('../controllers/matchController')
const { Player } = require('../models/player')

const configEnv = require('../config')

// Middlewares
const SessionMiddleware = require('../middlewares/Session.middleware');

routes.use(SessionMiddleware.isAuth)

routes.get('/', (req, res) => {
    res.status(200).send("Match alive!");
})

routes.post('/start', SessionMiddleware.isAuth, matchController.start)
routes.post('/finish', SessionMiddleware.isAuth, matchController.finish)


module.exports = routes;