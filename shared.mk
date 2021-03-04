SELF_DIR := $(dir $(lastword $(MAKEFILE_LIST)))

CONTAINER ?=

PROJECT_NAME ?=
PORT ?=

DOCKER_COMPOSE_CMD := docker-compose -f $(SELF_DIR)docker-compose.dev.yml

ifeq (,${NO_DOCKER})
DOCKER_RUN := $(DOCKER_COMPOSE_CMD) run -w /src/$(PROJECT_NAME) --rm ${CONTAINER}
else
DOCKER_RUN :=
endif

.PHONY: vendor
vendor:
	@echo "Installing dependency..."
	$(DOCKER_RUN) dotnet restore
	$(DOCKER_RUN) dotnet list $(PROJECT_NAME).csproj package

.PHONY: dev
dev: vendor
	$(DOCKER_RUN) dotnet watch run --urls "http://*:$(PORT)"

.PHONY: dist
dist: vendor
	$(DOCKER_RUN) dotnet publish -c Release -o dist
