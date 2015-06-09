# Copyright (c) Experian. All rights reserved.

lib = File.expand_path("../lib/", __FILE__)
$:.unshift lib unless $:.include?(lib)
require "electronic_updates/version"

Gem::Specification.new do |spec|
  spec.name = "electronic_updates"
  spec.version = ElectronicUpdates::VERSION

  spec.license = "Apache-2.0"
  spec.licenses = ["Apache-2.0"]

  spec.summary = "QAS Electronic Updates"
  spec.description = <<-EOF
    Downloads data files from the QAS Electronic Updates Metadata REST API.
  EOF

  spec.author = "Experian Data Quality"
  spec.authors = ["Experian Data Quality"]
  spec.email = "support@qas.com"
  spec.homepage = "https://rubygems.org/gems/electronic_updates"

  spec.required_rubygems_version = Gem::Requirement.new(">= 0") if spec.respond_to? :required_rubygems_version=
  spec.date = "2015-05-19"
  spec.require_paths = ["lib"]

  spec.files = [
    "Rakefile",
    "lib/electronic_updates.rb",
    "lib/electronic_updates/electronicupdates.rb",
    "lib/electronic_updates/version.rb"
  ]

  spec.test_files = [
    "test/test_electronic_updates.rb"
  ]

  spec.platform = Gem::Platform::RUBY

  spec.add_runtime_dependency "rest-client",
    ["~> 1.8"]

  spec.add_development_dependency "minitest",
    ["~> 5.6"]

end

