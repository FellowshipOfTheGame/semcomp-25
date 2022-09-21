module.exports = {
  apps : [{
    script: "./index.js",
    instances : "max",
    env_production: {
      NODE_ENV: "production"
   },
  }]
};


