"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.GoogleTokenStrategy = void 0;
/**
 * Module dependencies.
 */
const google_auth_library_1 = require("google-auth-library");
const passport_strategy_1 = require("passport-strategy");
/**
 * `Strategy` constructor.
 *
 * The Google authentication strategy authenticates requests by verifying the
 * signature and fields of the token.
 *
 * Applications must supply a `verify` callback which accepts the `idToken`
 * coming from the user to be authenticated, and then calls the `done` callback
 * supplying a `parsedToken` (with all its information in visible form) and the
 * `googleId`.
 *
 * Options:
 * - `clientID` your Google application's client id (or several as Array)
 *
 * Examples:
 *
 * passport.use(new GoogleTokenStrategy({
 *     clientID: '12345.abcdefghijkl.apps.googleusercontent.com'// Specify the CLIENT_ID of the app that accesses the backend
 *    // Or, if multiple clients access the backend:
 *    //[CLIENT_ID_1, CLIENT_ID_2, CLIENT_ID_3]
 *   },
 *   function(parsedToken, googleId, done) {
 *     User.findOrCreate(..., function (err, user) {
 *       done(err, user);
 *     });
 *   }
 * ));
 *
 * @param {Object} options
 * @param {Function} verify
 * @api public
 */
class GoogleTokenStrategy extends passport_strategy_1.Strategy {
    constructor(options, verify) {
        super();
        if (typeof options === 'function') {
            verify = options;
            options = {};
        }
        if (!verify) {
            throw new Error('GoogleVerifyTokenStrategy requires a verify function');
        }
        this.passReqToCallback = options.passReqToCallback;
        this.clientID = options.clientID;
        this.name = 'google-verify-token';
        this.googleAuthClient = new google_auth_library_1.OAuth2Client(this.clientID);
        this.verify = verify;
        this.audience = options.audience ? options.audience : options.clientID;
    }
    /**
     * Authenticate request by verifying the token
     *
     * @param {Object} req
     * @api protected
     */
    authenticate(req, options) {
        options = options || {};
        const idToken = this.paramFromRequest(req, 'id_token') ||
            this.paramFromRequest(req, 'access_token') ||
            this.getBearerToken(req.headers);
        console.log("id_token: ", idToken)

        if (!idToken) {
            return this.fail({ message: 'no ID token provided' }, 401);
        }
        this.verifyGoogleToken(idToken, this.clientID, (err, parsedToken, info) => {
            console.log("ParsedToken: ", parsedToken)
            console.log(info)
            if (err) {
                return this.fail({ message: err.message }, 401);
            }
            if (!parsedToken) {
                return this.fail(info);
            }
            const verified = (error, user, infoOnUser) => {
                if (error) {
                    return this.error(error);
                }
                if (!user) {
                    return this.fail(infoOnUser);
                }
                this.success(user, infoOnUser);
            };
            if (this.passReqToCallback) {
                this.verify(req, parsedToken, parsedToken.sub, verified);
            }
            else {
                this.verify(parsedToken, parsedToken.sub, verified);
            }
        });
    }
    /**
     * Verify signature and token fields
     *
     * @param {String} idToken
     * @param {String} clientID
     * @param {Function} done
     * @api protected
     */
    verifyGoogleToken(idToken, clientID, done) {
        this.googleAuthClient.verifyIdToken({
            requiredAudience: this.audience,
            idToken,
        }, (err, loginTicket) => {
            console.log("--------------")
            console.log(err)
            console.log(loginTicket)
            if (err) {
                done(null, false, { message: err.message });
            }
            else if (loginTicket) {
                const payload = loginTicket.getPayload();
                done(null, payload);
            }
            else {
                done(null, false, { message: 'No login ticket retuned' });
            }
        });
    }
    /**
     * Gets the id token value from req using name for lookup in req.body, req.query,
     * and req.params.
     * @param {express.Request} req
     * @param {string} name  the key to use to lookup id token in req.
     * @api protected
     */
    paramFromRequest(req, name) {
        const body = req.body || {};
        const query = req.query || {};
        const params = req.params || {};
        const headers = req.headers || {};
        if (body[name]) {
            return body[name];
        }
        if (query[name]) {
            return query[name];
        }
        if (headers[name]) {
            return headers[name];
        }
        return params[name] || '';
    }
    getBearerToken(headers) {
        if (headers && headers.authorization) {
            const parts = headers.authorization.split(' ');
            return parts.length === 2 && parts[0] === 'Bearer' ? parts[1] : undefined;
        }
    }
}
exports.GoogleTokenStrategy = GoogleTokenStrategy;
