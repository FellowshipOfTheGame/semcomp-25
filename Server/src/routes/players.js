// Dependencies
const express = require('express');
const routes = express.Router();

const configEnv = require('../config')

// Middlewares
const SessionMiddleware = require('../middlewares/Session.middleware');

// Controllers
// const SessionController = require('../controllers/sessionController');

// Routes
routes.get('/login', passport.authenticate('google', { scope: ['profile', 'email'], access_type: 'online' }))

// routes.get('/facebook/login', passport.authenticate('facebook', { scope: ['email', 'public_profile'] }))

// routes.get('/validate', SessionMiddleware.isAuth, (req, res) => res.json({ message: "ok" }))

// Export routes
module.exports = routes;