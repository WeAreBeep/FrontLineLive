DOCKER_COMPOSE_CMD := docker-compose -f docker-compose.dev.yml

.PHONY: dev
dev:
	make -C $(MODULE) dev

.PHONY: docker-start-db
docker-start-db:
	$(DOCKER_COMPOSE_CMD) up -d db

.PHONY: docker-stop-db
docker-stop-db:
	$(DOCKER_COMPOSE_CMD) stop db
