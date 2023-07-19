Feature: 4.1.Get All Manufacturers

	As a product owner
	I want to be able to get list of all categories
	So I could know what categories exist in the system and use them for product management

@Scenario_4.1.1.
@URS_NA
@Risk_NA
Scenario: 4.1.1. When a user sends the request to get all Manufacturers and Manufacturers exist in the database, then the OK status code is received and the expected body is returned
	Given the manufacturers are setup in the database
	When the user sends the GET request for Manufacturers
	Then the "Ok" status code is received
		And all manufacturers are returned

@Scenario_4.1.2
@URS_NA
@Risk_NA
Scenario: 4.1.2. When a user sends the request to get all Categories and no Categories exist in the database, then OK status code is received and no Categories are returned
	When the user sends the GET request for Manufacturers
	Then the "Ok" status code is received
		And no manufacturers are returned
