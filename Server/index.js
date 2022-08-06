const express = require('express')
const cors  = require('cors')
const morgan = require('morgan')

const { logger } = require('./src/config/logger')

const passport = require('passport');

const bodyParser = require('body-parser')
const cookieParser = require('cookie-parser');
const cookieSession = require('cookie-session');

// Singletons & Libraries Loaders
require('./src/loaders/firebase')
require('./src/loaders/passport')(passport)
 
// Enviroments Variables
const configEnv = require('./src/config')

// Routes
// const playerRoutes = require('./src/routes/players');
const sessionRoutes = require('./src/routes/session');

const app = express()

app.use(cors())

app.use(bodyParser.urlencoded({ extended: false }))
app.use(bodyParser.json())

// For an actual app you should configure this with an experation time, better keys, proxy and secure

// app.use(express.json())
app.use(express.urlencoded({ extended: false }))
app.use(morgan('dev'))

app.use(cookieSession({
    name: 'gameSession',
    keys: ['key1', 'key2'],
    maxAge:60*60*24
}))
 
app.use(passport.initialize());
app.use(passport.session());

// Routes Configurations
  
app.get(`${configEnv.SERVER_PATH_PREFIX}/ping`, (req, res) => res.json({ message: "pong :)" }))
// app.use(`${configEnv.SERVER_PATH_PREFIX}/player/`, playerRoutes)
app.use(`${configEnv.SERVER_PATH_PREFIX}/session/`, sessionRoutes)

//POST request to create a new task in todo list
app.post("/create", (req, res) => {
    //Code to add a new data to the database will go here
});

//POST request to delete a task in todo list
app.post("/delete", (req, res) => {
    //Code to delete a data from the database will go here
});

main().catch(err => console.log(err));

async function main() {

    app.listen(configEnv.SERVER_PORT, (error) => {
        if (error) throw error
        logger.info({
            message: `Starting HTTP server on port ${configEnv.SERVER_PORT}.`
        }) 
    })
};