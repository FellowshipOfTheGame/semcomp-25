// Dependencies
const express = require('express');
const routes = express.Router();

// Middlewares
const SessionMiddleware = require('../middlewares/Session.middleware');

// Controllers
const PlayerController = require('../controllers/playerController')

// Routes 
routes.get('/status', SessionMiddleware.isAuth, PlayerController.getInfoWithSession)
routes.get('/rank', SessionMiddleware.isAuth, PlayerController.getRanking);

// Export routes
module.exports = routes;