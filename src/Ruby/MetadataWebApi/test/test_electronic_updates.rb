# Copyright (c) Experian. All rights reserved.

gem "minitest"
require "minitest/autorun"
require File.join(File.dirname(__FILE__), '../lib', 'electronic_updates')

class ElectronicUpdatesTest < Minitest::Test

  def test_getToken_returns_the_token_that_was_set_with_setToken()
    @token = "MyToken"

    ElectronicUpdates::setToken(@token)
    @actual = ElectronicUpdates::getToken()

    assert_equal @token, @actual
  end

end
