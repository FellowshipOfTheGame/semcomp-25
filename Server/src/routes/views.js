// Dependencies
const routes = require('express').Router();
const ejs    = require('ejs')
const path   = require('path')

const config = require('../config')

const templates = path.join(__dirname, '../templates')
const publicUrl = `${config.SERVER_PATH_PREFIX}`


// Views Routes
routes.get('/', async (req, res) => { 
    ejs.renderFile(`${templates}/index.html`, 
        {
            title: 'Home',
            main:  'home.html',
            public: publicUrl,
        },
        function(err, html) { 
        return res.send(html)
    })
})

routes.get('/informacoes', async (req, res) => { 
    ejs.renderFile(`${templates}/index.html`, 
        {
            title: 'Políticas & Termos',
            main:  'about.html',
            public: publicUrl,
        },
        function(err, html) { 
        return res.send(html)
    })
})

// routes.get('/codigo-login', async (req, res) => { 
//     ejs.renderFile(`${templates}/index.html`, 
//         {
//             title: 'Código de Login',
//             main:  'access-code.html',
//             public: publicUrl,
//         },
//         function(err, html) { 
//         return res.send(html)
//     })
// })

routes.get('*', async (req, res) => { 
    ejs.renderFile(`${templates}/index.html`, 
        {
            title: '404 Página Não Encontrada',
            main:  '404.html',
            public: publicUrl,
        },
        function(err, html) { 
        return res.send(html)
    })
})

// Export routes
module.exports = routes;