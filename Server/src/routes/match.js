// Dependencies
const express = require('express');
const routes = express.Router();
const controller = require('../controllers/matchController')
const { Player } = require('../models/player')

const configEnv = require('../config')

// Middlewares
const SessionMiddleware = require('../middlewares/Session.middleware');

routes.use(SessionMiddleware.isAuth)

routes.get('/', (req, res) => {
    res.status(200).send("Match alive!");
})

routes.post('/start', controller.start)
routes.post('/finish', controller.finish)
routes.post('/savepoint', controller.savepoint)

module.exports = routes;