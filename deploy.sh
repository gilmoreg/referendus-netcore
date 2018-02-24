#!/bin/bash
set -e # Abort script at first error
set -u # Disallow unset variables

wget -qO- https://toolbelt.heroku.com/install-ubuntu.sh | sh
heroku plugins:install heroku-container-registry
docker login -u=$DOCKER_USER -p=$DOCKER_PASS registry.heroku.com