# Copyright (c) Experian. All rights reserved.

gem "minitest"
require "minitest/autorun"
require "electronic_updates"

class ElectronicUpdatesTest < Minitest::Test

  def test_getUserName_returns_the_user_name_that_was_set_with_setCredentials()
    @userName = "MyUserName"
    @password = "MyPassword"

    ElectronicUpdates::setCredentials(@userName, @password)
    @actual = ElectronicUpdates::getUserName()

    assert_equal @userName, @actual
  end

end
