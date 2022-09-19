var redis   = require("redis");
var session = require('express-session');
var redisStore = require('connect-redis')(session);
var client  = redis.createClient();

const config = require("../config")

 client.connect().catch(console.error)

//Configure redis client
const redisClient = new redisStore({
    host: config.REDIS_HOST,
    port: config.REDIS_PORT,
    client: client,
});


// Module Exports
module.exports = {
    redisClient,
    session,
}