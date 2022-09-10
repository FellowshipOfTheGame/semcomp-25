run:
	docker-compose up --build -d
server:
	docker-compose stop semcomp25
	docker-compose build semcomp25
	docker-compose run -d semcomp25
stop:
	docker-compose down
clean:
	docker-compose rm -af