// Dependencies
const express = require('express');
const routes = express.Router();
const passport = require('passport');

const config = require('../config')

// Middlewares
const SessionMiddleware = require('../middlewares/Session.middleware');

// Controllers
const SessionController = require('../controllers/sessionController');

// Routes
routes.get('/login', passport.authenticate('google', { scope: ['profile', 'email'], access_type: 'online' }))
routes.get('/login/callback', passport.authenticate('google', { failureRedirect: `${config.SERVER_PATH_PREFIX}/?auth=failed` }), SessionController.loginCallback)

// routes.get('/facebook/login', passport.authenticate('facebook', { scope: ['email', 'public_profile'] }))
// routes.get('/facebook/login/callback', passport.authenticate('facebook', { failureRedirect: `${config.SERVER_PATH_PREFIX}/?auth=failed` }), SessionController.loginCallback)


routes.post('/login/get-session', SessionController.getSession)

routes.get('/validate', SessionMiddleware.isAuth, (req, res) => res.json({ message: "ok" }))
routes.post('/logout', SessionController.logout);

// Export routes
module.exports = routes;