#!/usr/bin/env bash
if [ "$TRAVIS_OS_NAME" = "linux" ]; then
  sudo cp ./src/CSharp/MetadataWebApi/EntrustRootCertificationAuthority-G2.crt /usr/local/share/ca-certificates/EntrustRootCertificationAuthority-G2.crt
  sudo update-ca-certificates
  mozroots --import --sync
fi
