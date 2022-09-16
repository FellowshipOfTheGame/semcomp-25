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

<<<<<<< HEAD
routes.post('/start', controller.start)
routes.post('/finish', controller.finish)
routes.post('/savepoint', controller.savepoint)
=======
routes.post('/start', SessionMiddleware.isAuth, matchController.start)
routes.post('/finish', SessionMiddleware.isAuth, matchController.finish)

>>>>>>> 3981019 (Fix finish match)

module.exports = routes;