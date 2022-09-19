FROM node:18-alpine

# Create app directory
WORKDIR /app

# Install app dependencies
# A wildcard is used to ensure both package.json AND package-lock.json are copied
COPY package*.json ./

RUN npm install

RUN npm install -g pm2

# Bundle app source
COPY . .

EXPOSE 3000

ENV TZ America/Sao_Paulo

CMD [ "pm2", "start", "ecosystem.config.js", "--attach", "--env=production" ]
