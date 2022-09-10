const { logger } = require('../config/logger');

module.exports = {
    isAuth (req, res, next) {
        if (req.userAgent == "Algo") {
            return next();
        }
        else {        
            logger.warn({
                message: `${req.method} ${req.path} - ${req.ip} - unauthorized request`
            })
            
            return res.status(401).json({ message: "unauthorized" });
        }
    }
}