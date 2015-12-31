#!/usr/bin/env bash
if [ "$TRAVIS_OS_NAME" = "linux" ]; then
  mozroots --import --ask-remove
  sudo cp ./updates.qas.com.crt /usr/local/share/ca-certificates/updates.qas.com.crt
  sudo update-ca-certificates
fi
