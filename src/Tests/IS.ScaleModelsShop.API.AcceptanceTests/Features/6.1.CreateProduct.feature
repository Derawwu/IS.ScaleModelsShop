Feature: 6.1.CreateProduct

	As a product owner
	I want to be able to create a new product
	So that I can extend range of available products

@Scenario_6.1.1.
@URS_NA
@Risk_NA
Scenario: 6.1.1. When a user sends request to create a Product, then the Created status code should be received and the created Product should be returned
	Given the following Category already exist
		| Name | Value                                |
		| Id   | 00000000-0000-0000-0000-000000000001 |
		| Name | TestCategory                         |
		And the following Manufacturer already exist
			| Name    | Value                                |
			| Id      | 00000000-0000-0000-0000-000000000002 |
			| Name    | TestManufacturer                     |
			| Website | http://example.org                   |
	When the user sends the POST request for Product with data in the request body
		| Name           | Value                                |
		| Name           | TestProduct                          |
		| Description    | TestDescription                      |
		| Price          | 1                                    |
		| ManufacturerId | 00000000-0000-0000-0000-000000000002 |
		| CategoryId     | 00000000-0000-0000-0000-000000000001 |
	Then the "Created" status code is received
		And the following Product is updated in the database
			| Name           | Value                                |
			| Name           | TestProduct                          |
			| Description    | TestDescription                      |
			| Price          | 1                                    |
			| ManufacturerId | 00000000-0000-0000-0000-000000000002 |
			| CategoryId     | 00000000-0000-0000-0000-000000000001 |

@Scenario_6.1.2.
@URS_NA
@Risk_NA
Scenario: 6.1.2. When a user sends request to create a Product with null as the Name property, then the BadRequest status code should be received and the expected error message should be received
	Given the following Category already exist
		| Name | Value                                |
		| Id   | 00000000-0000-0000-0000-000000000001 |
		| Name | TestCategory                         |
		And the following Manufacturer already exist
			| Name    | Value                                |
			| Id      | 00000000-0000-0000-0000-000000000002 |
			| Name    | TestManufacturer                     |
			| Website | http://example.org                   |
	When the user sends the POST request for Product with data in the request body
		| Name           | Value                                |
		| Description    | TestDescription                      |
		| Price          | 1                                    |
		| ManufacturerId | 00000000-0000-0000-0000-000000000002 |
		| CategoryId     | 00000000-0000-0000-0000-000000000001 |
	Then the "Bad Request" status code is received
		And the "required" message is returned for the "Name" field