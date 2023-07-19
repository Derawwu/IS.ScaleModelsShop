Feature: 5.1.Update Manufacturer

	As a product owner
	I want to be able to update a Manufacturer
	So I could always have manufacturers up-to-date

@Scenario_5.1.1.
@URS_NA
@Risk_NA
Scenario: 5.1.1. When a user sends a request to update a Manufacturer, then the NoContent status code should be received and the manufacturer should be updated in the database
	Given the manufacturers are setup in the database
	When the user sends the PUT request for Manufacturer with ManufacturerId and the following data
		| Key     | Value                      |
		| Name    | TestManufacturerUpdated    |
		| Website | https://exampleupdated.org |
	Then the "NoContent" status code is received
		And the following Manufacturer is updated in the database
			| Key     | Value                      |
			| Name    | TestManufacturerUpdated    |
			| Website | https://exampleupdated.org |
		And the LastModifiedDate field in the database has value for the updated manufacturer

@Scenario_5.1.2.
@URS_NA
@Risk_NA
Scenario: 5.1.2. When a user sends a request to update a Manufacturer without Website property in the request body, then the NoContent status code should be received and the manufacturer should be updated in the database
	Given the manufacturers are setup in the database
	When the user sends the PUT request for Manufacturer with ManufacturerId and the following data
		| Key     | Value                      |
		| Name    | TestManufacturerUpdated    |
	Then the "No Content" status code is received
		And the following Manufacturer is updated in the database
			| Key     | Value                      |
			| Name    | TestManufacturerUpdated    |
		And the LastModifiedDate field in the database has value for the updated manufacturer

@Scenario_5.1.3.
@URS_NA
@Risk_NA
Scenario: 5.1.3. When a user sends a request to update a Manufacturer without Name property in the request body, then the BadRequest status code should be received and the expected error message is returned
	Given the manufacturers are setup in the database
	When the user sends the PUT request for Manufacturer with ManufacturerId and the following data
		| Key     | Value                      |
		| Website | https://exmapleupdated.org |
	Then the "Bad Request" status code is received
		And the "required" message is returned for the "Name" field

@Scenario_5.1.4.
@URS_NA
@Risk_NA
Scenario: 5.1.4. When a user sends a request to update a Manufacturer with the extra field in the body, then the NoContent status code should be received and the Manufacturer should not have extra field property in it
	Given the manufacturers are setup in the database
	When the user sends the PUT request for Manufacturer with ManufacturerId and with extra field in the request body
		| Key        | Value                      |
		| Name       | TestManufacturerUpdated    |
		| Website    | https://exmapleupdated.org |
	Then the "NoContent" status code is received
		And the following Manufacturer is updated in the database
			| Key     | Value                      |
			| Name    | TestManufacturerUpdated    |
			| Website | https://exmapleupdated.org |
		And the ExtraField field is not present in the Manufacturer response body after update
		And the LastModifiedDate field in the database has value for the updated manufacturer

@Scenario_5.1.5.
@URS_NA
@Risk_NA
Scenario: 5.1.5. When a user sends a request to update a Manufacturer with the empty request body, then the BadRequest status code should be received and the expected error message is returned
	When the user sends the PUT request for Manufacturer with empty request body
	Then the "BadRequest" status code is received
		And the "required" message is returned for the "Name" field

@Scenario_5.1.6.
@URS_NA
@Risk_NA
Scenario: 5.1.6. When a user sends a request to update a Manufacturer without the request body, then the UnsupportedMediaType status code should be received and the expected error message is returned
	When the user sends the PUT request for Manufacturer without request body
	Then the "Unsupported Media Type" status code is received

@Scenario_5.1.7.
@URS_NA
@Risk_NA
Scenario: 5.1.7. When a user sends a request to update a Manufacturer with Website field have more that 40 symbols in it, then the UnprocessableEntity status code is received and the expected error message is returned
	When the user sends the PUT request for Manufacturer filling more than 40 characters into the Website property
	Then the "Unprocessable Entity" status code is received
		And the "TooManyCharacters" message is returned for the "Website" field

@Scenario_5.1.8.
@URS_NA
@Risk_NA
Scenario: 5.1.8. When a user sends a request to update a Manufacturer with Name field have more that 50 symbols in it, then the UnprocessableEntity status code is received and the expected error message is returned
	When the user sends the PUT request for Manufacturer filling more than 50 characters into the Name property
	Then the "Unprocessable Entity" status code is received
		And the "Number of characters not in allowed range" message is returned for the "Name" field

@Scenario_5.1.9.
@URS_NA
@Risk_NA
Scenario: 5.1.9. When a user sends a request to update a Manufacturer with Name field have less that 3 symbols in it, then the UnprocessableEntity status code is received and the expected error message is returned
	When the user sends the PUT request for Manufacturer filling less than 3 characters into the Name property
	Then the "Unprocessable Entity" status code is received
		And the "Number of characters not in allowed range" message is returned for the "Name" field

@Scenario_5.1.10.
@URS_NA
@Risk_NA
Scenario Outline: 5.1.10. When a user sends a request to update a Manufacturer with empty Name field, then the error status code is received and the expected error message is returned
	Given the manufacturers are setup in the database
	When the user sends the PUT request for Manufacturer with "<Value>" as the Name property value
	Then the "<StatusCode>" status code is received
		And the "<ErrorMessageType>" message is returned for the "Name" field

	Examples:
		| Value        | StatusCode           | ErrorMessageType |
		| null         | Bad Request          | required         |
		| empty string | Unprocessable Entity | mustNotBeEmpty   |

@Scenario_5.1.11.
@URS_NA
@Risk_NA
Scenario: 5.1.11. When a user sends a request to update a Manufacturer with empty Website field, then the NoContent status code should be received and the manufacturer should be updated in the database
	Given the manufacturers are setup in the database
	When the user sends the PUT request for Manufacturer with "empty string" as the Website property value
	Then the "NoContent" status code is received
		And the following Manufacturer is updated in the database
			| Key     | Value                   |
			| Name    | TestManufacturerUpdated |
			| Website |                         |

@Scenario_5.1.12.
@URS_NA
@Risk_NA
Scenario: 5.1.12. When a user sends a request to update a Manufacturer with null as Website field, then the NoContent status code should be received and the manufacturer should be updated in the database
	Given the manufacturers are setup in the database
	When the user sends the PUT request for Manufacturer with "null" as the Website property value
	Then the "NoContent" status code is received
		And the following Manufacturer is updated in the database
			| Key     | Value                   |
			| Name    | TestManufacturerUpdated |

@Scenario_5.1.13.
@URS_NA
@Risk_NA
Scenario: 5.1.13. When a user sends a request to update a non-existing Manufacturer, then the NotFound status code should be received and the expected error message is returned
	When the user sends the PUT request for Manufacturer with non-existing ManufacturerId and the following data
		| Key     | Value                      |
		| Name    | TestManufacturerUpdated    |
		| Website | https://exmapleupdated.org |
	Then the "NotFound" status code is received
		And the "NotFound" error message returned for the Manufacturer

@Scenario_5.1.14.
@URS_NA
@Risk_NA
Scenario: 5.1.14. When a user sends a request to update a Manufacturer and a Manufacturer with the same Name already exist, then the UnprocessableEntity status code is received and the expected error message is returned
	Given the following Manufacturer already exist
		| Key  | Value                   |
		| Name | TestManufacturerUpdated |
		And the manufacturers are setup in the database
	When the user sends the PUT request for Manufacturer with ManufacturerId and the following data
		| Key     | Value                      |
		| Name    | TestManufacturerUpdated    |
		| Website | https://exampleupdated.org |
	Then the "Unprocessable Entity" status code is received
		And the "Manufacturer already exists" message is returned for the "Name" field

@Scenario_5.1.15.
@URS_NA
@Risk_NA
Scenario: 5.1.15. When a user sends a request to update a Manufacturer and a Manufacturer with the same WebSite already exist, then the UnprocessableEntity status code is received and the expected error message is returned
	Given the following Manufacturer already exist
		| Key     | Value                      |
		| Name    | Manufacturer               |
		| Website | https://exampleupdated.org |
		And the manufacturers are setup in the database
	When the user sends the PUT request for Manufacturer with ManufacturerId and the following data
		| Key     | Value                      |
		| Name    | TestManufacturerUpdated    |
		| Website | https://exampleupdated.org |
	Then the "NoContent" status code is received
		And the following Manufacturer is updated in the database
			| Key     | Value                      |
			| Name    | TestManufacturerUpdated    |
			| Website | https://exampleupdated.org |
		And the LastModifiedDate field in the database has value for the updated manufacturer