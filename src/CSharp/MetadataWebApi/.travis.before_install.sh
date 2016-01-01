#!/usr/bin/env bash
if [ "$TRAVIS_OS_NAME" = "linux" ]; then
  sudo apt-get install ca-certificates
  sudo update-ca-certificates
  mozroots --import --sync
fi
