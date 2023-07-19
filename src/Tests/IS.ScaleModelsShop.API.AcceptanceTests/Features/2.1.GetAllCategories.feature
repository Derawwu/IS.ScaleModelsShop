Feature: 2.1.Get All Categories

	As a product owner
	I want to be able to get list of all categories
	So I could know what categories exist in the system and use them for product management

@Scenario_2.1.1.
@URS_NA
@Risk_NA
Scenario: 2.1.1. When a user sends the request to get all Categories and Categories exist in the database, then the OK status code is received and the expected body is returned
	Given the categories are setup in the database
	When the user sends the GET request for Categories
	Then the "Ok" status code is received
		And all categories are returned

@Scenario_2.1.2
@URS_NA
@Risk_NA
Scenario: 2.1.2. When a user sends the request to get all Categories and no Categories exist in the database, then OK status code is received and no Categories are returned
	When the user sends the GET request for Categories
	Then the "Ok" status code is received
		And no categories are returned