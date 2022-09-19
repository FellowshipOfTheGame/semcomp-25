const express = require('express')
const cors  = require('cors')
const morgan = require('morgan')
const fs = require('fs');
const path = require('path')

const { logger } = require('./src/config/logger')

const passport = require('passport');

const bodyParser = require('body-parser')
// const cookieParser = require('cookie-parser');
// const cookieSession = require('cookie-session');
const session = require('./src/loaders/session')

// Singletons & Libraries Loaders
require('./src/loaders/firebase')
require('./src/loaders/passport')(passport)
 
// Enviroments Variables
const configEnv = require('./src/config')

// Routes
const playerRoutes = require('./src/routes/players');
const sessionRoutes = require('./src/routes/session');
const matchRoutes = require('./src/routes/match')
const viewsRoutes = require('./src/routes/views')

const app = express()
// app.set('trust proxy', true)
// app.use(bodyParser.urlencoded({ extended: false }))
// app.use(bodyParser.json())
app.use(express.json())
app.use(express.urlencoded({ extended: false }))
app.use(morgan('dev'))
// app.use(session.cookieLoader())
app.use(session.sessionLoader())

// app.use(cookieSession({
//     name: 'gameSession',
//     keys: ['key1', 'key2'],
//     maxAge: 24 * 60 * 60 * 1000 // 24 hours
// }))
app.use(passport.initialize());
app.use(passport.session());

// Enable cors to all origins (because we are an API after all :P)
app.use(cors({
    credentials: true,
    origin: /^https:\/\/[a-zA-Z0-9]*\.ssl\.hwcdn\.net$/,
    optionsSuccessStatus: 200, // some legacy browsers (IE11, various SmartTVs) choke on 204
    "methods": "GET,HEAD,PUT,PATCH,POST,DELETE",
    "preflightContinue": false,
    exposedHeaders: ["set-cookie"],
}))

// Security and Log Configurations
// TODO (https://expressjs.com/pt-br/advanced/best-practice-security.html)
app.disable('x-powered-by');

app.use((req, res, next) => {
    let ip = req.headers['x-forwarded-for'] || (req.socket.remoteAddress);
    ip = ip.indexOf('.') >= 0 ? ip.substring(ip.lastIndexOf(':') + 1) : ip;

    req.ip = ip;

    logger.info({
        message: `${req.method} ${req.path} - User-Agent: ${req.get('User-Agent')} - ${req.ip} `
    });
    next();
})

// Routes Configurations
  
app.get(`${configEnv.SERVER_PATH_PREFIX}/ping`, (req, res) => res.json({ message: "pong :)" }))
app.use(`${configEnv.SERVER_PATH_PREFIX}/player/`, playerRoutes)
app.use(`${configEnv.SERVER_PATH_PREFIX}/session/`, sessionRoutes)
app.use(`${configEnv.SERVER_PATH_PREFIX}/match/`, matchRoutes)
app.use(`${configEnv.SERVER_PATH_PREFIX}/api/`, viewsRoutes)
app.use(configEnv.SERVER_PATH_PREFIX, express.static(path.join(__dirname, 'src/public')));

main().catch(err => console.log(err));

async function main() {

    if(configEnv.ENABLE_HTTPS) { 

        var httpsCredentials = {
            key:  configEnv.CERTIFICATE_KEY_PATH && fs.readFileSync(configEnv.CERTIFICATE_KEY_PATH),
            cert: configEnv.CERTIFICATE_CERT_PATH && fs.readFileSync(configEnv.CERTIFICATE_CERT_PATH),
            ca:   configEnv.CERTIFICATE_CA_PATH && fs.readFileSync(configEnv.CERTIFICATE_CA_PATH)
        }
    
        https.createServer(httpsCredentials, app).listen(configEnv.SERVER_PORT , (error) => {
            if (error) throw error
            logger.info({
                message: `Starting HTTPS server on port ${configEnv.SERVER_PORT}.`
            }) 
        })
    } else {
        app.listen(configEnv.SERVER_PORT, (error) => {
            if (error) throw error
            logger.info({
                message: `Starting HTTP server on port ${configEnv.SERVER_PORT}.`
            }) 
        })
    }
};