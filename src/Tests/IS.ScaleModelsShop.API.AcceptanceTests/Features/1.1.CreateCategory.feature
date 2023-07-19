Feature: 1.1.Create Category

	As a product owner
	I want to be able to add new categories
	So appropriate category could be assigned to a product

@Scenario_1.1.1.
@URS_NA
@Risk_NA
Scenario: 1.1.1. When a user sends a request to add a category, then the Created status code is received and the expected body is returned
	When the user sends the POST request for Category with data in the request body
	| Key  | Value        |
	| Name | TestCategory |
	Then the "Created" status code is received
		And the Category response body matches the expected data

@Scenario_1.1.2.
@URS_NA
@Risk_NA
Scenario Outline: 1.1.2. When a user sends a request to add a category without a name, then the status code indicating error is received and the expected error message is returned
	When the user sends the POST request for Category with "<Value>" as the Name property
	Then the "<StatusCode>" status code is received
		And the "<ErrorMessageType>" message is returned for the "Name" field

	Examples:
	| Value        | StatusCode           | ErrorMessageType |
	| null         | Bad Request          | required         |
	| empty string | Unprocessable Entity | mustNotBeEmpty   |

@Scenario_1.1.3.
@URS_NA
@Risk_NA
Scenario: 1.1.3. When a user sends a request to add a category with a name exceeded max length, then the UprocessableEntity status code is received and the expected error message is returned
	When the user sends the POST request for a Category with the Name property value of more than 50 characters
	Then the "Unprocessable Entity" status code is received
		And the "TooManyCharacters" message is returned for the "Name" field

@Scenario_1.1.4.
@URS_NA
@Risk_NA
Scenario: 1.1.4. When a user sends a request to add a category and a Category with the same name already exists in the database, then the UprocessableEntity status code is received and the expected error message is returned
	Given the following Category already exist
		| Key  | Value        |
		| Name | TestCategory |
	When the user sends the POST request for Category with data in the request body
		| Key  | Value        |
		| Name | TestCategory |
	Then the "Unprocessable Entity" status code is received
		And the "Category already exists" message is returned for the "Name" field

@Scenario_1.1.5.
@URS_NA
@Risk_NA
Scenario: 1.1.5. When a user sends a request to add a category with the extra field, then the Created status code is received and the expected body is returned
When the user sends the POST request for Category with extra filed in the request body
		| Key        | Value        |
		| Name       | TestCategory |
	Then the "Created" status code is received
		And the Category response body matches the expected data
		And the ExtraField field is not present in the Category response body

@Scenario_1.1.6.
@URS_NA
@Risk_NA
Scenario: 1.1.6. When a user sends a request to add a Category without request body, then the UnsupportedMediaType status code is received
	When the user sends the POST request for Category without request body
	Then the "Unsupported Media Type" status code is received