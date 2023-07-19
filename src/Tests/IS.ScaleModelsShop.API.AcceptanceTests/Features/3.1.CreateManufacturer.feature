Feature: 3.1.Create Manufacturer

	As a product owner
	I want to be able to add new manufacturers
	So appropriate manufacturer could be assigned to a product

@Scenario_3.1.1.
@URS_NA
@Risk_NA
Scenario: 3.1.1. When a user sends request to add a Manufacturer, then the Created status code is received and the expected body is returned
	When the user sends the POST request for Manufacturer with data in the request body
		| Key     | Value            |
		| Name    | TestManufacturer |
		| Website | www.example.org  |
	Then the "Created" status code is received
		And the Manufacturer response body matches the expected data

@Scenario_3.1.2.
@URS_NA
@Risk_NA
Scenario: 3.1.2. When a user sends request to add a Manufacturer without the Website field, then the Created status code is received and the expected body is returned
	When the user sends the POST request for Manufacturer with data in the request body
		| Key  | Value            |
		| Name | TestManufacturer |
	Then the "Created" status code is received
		And the Manufacturer response body matches the expected data

@Scenario_3.1.3.
@URS_NA
@Risk_NA
Scenario Outline: 3.1.3. When a user sends request to add a Manufacturer without Name field, then the status code indicating error is received and the expected error message is returned
	When the user sends the POST request for Manufacturer with "<Value>" as the Name property
	Then the "<StatusCode>" status code is received
		And the "<ErrorMessageType>" message is returned for the "Name" field

	Examples:
	 | Value        | StatusCode           | ErrorMessageType |
	 | null         | Bad Request          | required         |
	 | empty string | Unprocessable Entity | mustNotBeEmpty   |

@Scenario_3.1.4.
@URS_NA
@Risk_NA
Scenario: 3.1.4. When a user sends request to add a Manufacturer with Website field set to invalid string, then the then the UnprocessableEntity status code is received and the expected error message is returned
	When the user sends the POST request for Manufacturer with "<Value>" as the Website property
	Then the "Unprocessable Entity" status code is received
		And the "Invalid URL" message is returned for the "Website" field

	Examples:
	 | Value          |
	 | 1              |
	 | testString     |
	 | www.example    |
	 | http://example |

@Scenario_3.1.5.
@URS_NA
@Risk_NA
Scenario Outline: 3.1.5. When a user sends request to add a Manufacturer with Website field set URL with inappropriate domain name, then the then the UnprocessableEntity status code is received and the expected error message is returned
	When the user sends the POST request for Manufacturer with "<Value>" as the Website property
	Then the "Unprocessable Entity" status code is received
		And the "Inappropriate domain name" message is returned for the "Website" field

	Examples:
	 | Value                |
	 | http://example.su    |
	 | http://example.ru    |
	 | http://shittyrussiansite.ru |
	 | http://shittyrussiansite.su |

@Scenario_3.1.6.
@URS_NA
@Risk_NA
Scenario: 3.1.6. When a user sends request to add a Manufacturer with Website field have more that 40 symbols in it, then the then the UnprocessableEntity status code is received and the expected error message is returned
	When the user sends the POST request for Manufacturer filling more than 40 characters into the Website property
	Then the "Unprocessable Entity" status code is received
		And the "TooManyCharacters" message is returned for the "Website" field

@Scenario_3.1.7.
@URS_NA
@Risk_NA
Scenario: 3.1.7. When a user sends request to add a Manufacturer with Name field have more that 50 symbols in it, then the then the UnprocessableEntity status code is received and the expected error message is returned
	When the user sends the POST request for Manufacturer filling more than 50 characters into the Name property
	Then the "Unprocessable Entity" status code is received
		And the "Number of characters not in allowed range" message is returned for the "Name" field

@Scenario_3.1.8.
@URS_NA
@Risk_NA
Scenario: 3.1.8. When a user sends request to add a Manufacturer with Name field have less that 3 symbols in it, then the then the UnprocessableEntity status code is received and the expected error message is returned
	When the user sends the POST request for Manufacturer filling less than 3 characters into the Name property
	Then the "Unprocessable Entity" status code is received
		And the "Number of characters not in allowed range" message is returned for the "Name" field

@Scenario_3.1.9.
@URS_NA
@Risk_NA
Scenario: 3.1.9. When a user sends request to add a Manufacturer and Manufacturer with the same name already exists in the database, then the then the UnprocessableEntity status code is received and the expected error message is returned
	Given the following Manufacturer already exist
		| Key  | Value            |
		| Name | TestManufacturer |
	When the user sends the POST request for Manufacturer with data in the request body
		| Key  | Value            |
		| Name | TestManufacturer |
	Then the "Unprocessable Entity" status code is received
		And the "Manufacturer already exists" message is returned for the "Name" field

@Scenario_3.1.10.
@URS_NA
@Risk_NA
Scenario: 3.1.10. When a user sends request to add a Manufacturer without request body, then the UnsupportedMediaType status code is received
	When the user sends the POST request for Manufacturer without the request body
	Then the "Unsupported Media Type" status code is received

@Scenario_3.1.11.
@URS_NA
@Risk_NA
Scenario: 3.1.11. When a user sends request to add a Manufacturer with an extra field in the request body, then the Created status code is received and the expected body is returned
	When the user sends the POST request for Manufacturer with extra field in the request body
		| Key        | Value            |
		| Name       | TestManufacturer |
		| Website    | www.example.org  |
	Then the "Created" status code is received
		And the Manufacturer response body matches the expected data
		And the ExtraField field is not present in the Manufacturer response body

@Scenario_3.1.12
@URS_NA
@Risk_NA
Scenario: 3.1.12. When a user sends request to add a Manufacturer with empty request body, then the BadRequest status code is received and the expected error message is returned
	When the user sends the POST request for Manufacturer with empty request body
	Then the "Bad Request" status code is received
		And the "required" message is returned for the "Name" field