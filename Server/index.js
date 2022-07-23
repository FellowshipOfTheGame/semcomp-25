const express = require('express')
const morgan = require('morgan')
const cors  = require('cors')
const configEnv = require('./src/config')
const logger = require('./src/config/logger')

// Singletons & Libraries Loaders
require('./src/loaders/firebase')

const app = express()

app.use(express.json());
app.use(express.urlencoded({ extended: false }))
app.use(morgan('dev'))
app.use(cors());

//Creating APIs

//GET request to display our todo list
app.get("/", (req, res) => {
    //Code to fecth data from the database will go here
});
  
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
        console.log({
            message: `Starting HTTP server on port ${configEnv.SERVER_PORT}.`
        }) 
    })
}