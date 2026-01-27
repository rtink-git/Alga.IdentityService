#!/bin/bash
set -e

REMOTE_PUB_DIR="./bin/Release/publish"
REMOTE_HOST="root@83.229.87.134"
REMOTE_DIR="/root/docker/auth-rt-ink"

dotnet publish -c Release -r linux-x64 --self-contained false -o "$REMOTE_PUB_DIR"

# 🔹 Создаём директорию на сервере
ssh $REMOTE_HOST "mkdir -p $REMOTE_DIR"

# 🔹 Чистим содержимое
ssh $REMOTE_HOST "rm -rf $REMOTE_DIR/*"

# 🔹 Копируем
scp -r "$REMOTE_PUB_DIR/"* "$REMOTE_HOST:$REMOTE_DIR/"

rm -rf "$REMOTE_PUB_DIR"

# 🔹 Docker
ssh $REMOTE_HOST "cd $REMOTE_DIR && docker-compose down && docker-compose up -d --build"



# #!/bin/bash

# REMOTE_PUB_DIR="./bin/Release/publish"
# REMOTE_HOST="root@83.229.87.134"
# REMOTE_DIR="/root/docker/auth-rt-ink/"

# dotnet publish -c Release -r linux-x64 --self-contained false -o $REMOTE_PUB_DIR
# ssh $REMOTE_HOST "rm -rf $REMOTE_DIR*"
# scp -r $REMOTE_PUB_DIR/* $REMOTE_HOST:$REMOTE_DIR
# rm -rf $REMOTE_PUB_DIR

# ssh $REMOTE_HOST "cd $REMOTE_DIR && docker-compose down && docker-compose up -d --build"