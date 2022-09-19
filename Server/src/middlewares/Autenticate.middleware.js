const { logger } = require('../config/logger');

module.exports = {
    isAuth (req, res, next) {
        console.log(req.userAgent)
        if (req.userAgent == "Algo") {
            return 
        } else {

        }
       
    }
}