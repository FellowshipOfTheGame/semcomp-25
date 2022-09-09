// Dependencies
const express = require('express');
const routes = express.Router();

const configEnv = require('../config')

// Middlewares
const SessionMiddleware = require('../middlewares/Session.middleware');

routes.get('/', (req, res) => {
    res.status(200).send("Match alive!");
})

routes.get('/start', (req, res) => {
    
})

module.exports = routes;