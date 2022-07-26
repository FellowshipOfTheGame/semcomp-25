const express = require('express')
const cors  = require('cors')
const morgan = require('morgan')
const { logger } = require('./src/config/logger')
const passport = require('passport');

// Singletons & Libraries Loaders
require('./src/loaders/firebase')
require('./loaders/passport')(passport)

// Enviroments Variables
const configEnv = require('./src/config')

// Routes
const playerRoutes = require('./src/routes/players')

const app = express()

app.use(express.json())
app.use(express.urlencoded({ extended: false }))
app.use(morgan('dev'))
app.use(cors())

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
    // Test application

    // app.set('view-engine', 'ejs')

    // https://www.youtube.com/watch?v=-RCnNyD0L-s
    
    //GET request to display our todo list

    app.listen(configEnv.SERVER_PORT, (error) => {
        if (error) throw error
        logger.info({
            message: `Starting HTTP server on port ${configEnv.SERVER_PORT}.`
        }) 
    })
};