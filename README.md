# IS Scale Model Shop

## Solution Strucuture
* `IS.ScaleModelsShop.API` - An ASP.NET Web API application.
* `IS.ScaleModelsShop.API.Contracts` - Contracts between the IS Scale Model Shop API and IS Scale Model Shop Application.
* `IS.ScaleModelsShop.API.UnitTests` - Contains unit tests for `ScaleModelsShop.API`.

* `ScaleModelsShop.Infrastructure` - Defines functionality to provide object saving and retrieving behavior.
* `ScaleModelsShop.Infrastructure.UnitTests` - Contains unit tests for `ScaleModelsShop.Infrastructure`.

* `ScaleModelsShop.Application` - Represents a set of rules, principles and dependencies for the behavior of objects in the domain.
* `ScaleModelsShop.Application.UnitTests` - Contains unit tests for `ScaleModelsShop.Application`.

* `ScaleModelsShop.Domain` - Contains base objects for the application behavior.

### Detailed System Design

The IS.ScaleModelsShop is designed to manage data for a Study within the new IRT platform.

The projects use a target framework of .NET 7.
IS.ScaleModelsShop architecture design is based on the Clean Architecture pattern.

#### Application Project

Contains handlers that process commands and queries for following the CQRS principle and the mediator pattern:
* CreateCategoryCommandHandler - a handler for the operation of creating a category
* CreateManufacturerCommandHandler - a handler for the operation of creating a manufacturer
* CreateProductCommandHandler - a handler for the operation of creating a product
* DeleteProduct - a handler for the operation of deleting product
* EditManufacturerCommandHandler - a handler for the operation of editing a manufacturer
* EditProductCommandHandler - a handler for the operation of editing a product
* GetAllCategoriesQueryHandler - the query handler for receiving all categories
* GetAllManufacturersQueryHandler - the query handler for receiving all manufacurers
* GetAllProductsQueryHandler - the query handler for receiving all products
* GetProductByNameQueryHandler - the query handler for receiving a product by name
* GetProductsByCategoryName - the query for receiving all products by assigned category id
* GetProductsByManufacturerName - the query for receiving all products by assigned manufacturer id

#### API Project

Contains controllers with RESTful endpoints for managing the data for a Product.

#### Infrastructure Project

The Infrastructure project is responsible for implementing all the IO operations that are required for the software.
Contains repositories to work with MS SQL Database entities.

#### Domain Project

The Domain project is the center part of the architecture. It holds all application domain objects.
Contains ORM data models and other domain models and services that are injected and used in other levels of application.

## 4.1 Application Interfaces

Not applicable. The API does not have an UI as such within its solution.

## 4.2 User Interface Design

Not applicable. The API is unaware of the UI.

## 4.3 Data Conversions

The Mapper is responsible for mapping between the client view models into the logical business objects.

## 4.4 Application Program Interfaces

The IS.ScaleModelsShop has the following endpoints:

### Products

Allows interaction with the Products capabilities.

#### Get All Products

**GET /api/products**

Returns the content with all products match pagination.

*Validation:*

- Optional parameters are:
  - "$pageSize" must be greater than 0
  - "$pageCount" must be must be greater than 0

  *Pagination:*

  - if "pageSize" is defined, but "pageCount" not defined or equals 0,
    the first top items will return.

  - if "pageCount" is defined, but "pageSize" not defined,
    the first element from the database will be returned.

#### Get Product by Name

**GET /api/products/{productName}**

Returns the content of the requested product with a name that matches the requested {productName}.

*Validation:*

- Required field is:
  - "{id}" must be a guid

#### Get Product by Category Id

**GET /api/get-products-by-category/{categoryId}**

Returns the content of the requested product with a linked category that matches the requested {categoryId}.

*Validation:*

- Required field is:
  - "{categoryId}" must be a guid

#### Get Product by Manufacturer Id

**GET /api/get-products-by-manufacturer/{manufacturerId}**

Returns the content of the requested product with a linked manufacturer that matches the requested {manufacturerId}.

*Validation:*

- Required field is:
  - "{manufacturerId}" must be a guid

#### Create Product

**POST /api/products**

Adds a new product to the database.

*Validation:*

- Required fields are:
  - "Name"
  - "Description"
  - "Price"
  - "CategoryId"
  - "ManufacturerId"

- Length:
  - maximum 50:
    - "Name"
  - maximum 500:
   - "Description"

- Unique name:
  - "Name"

- Greater then zero:
 - "Price"

#### Update Study

**PUT /api/products/{id}**

Updates a product in the database.
If the "CategoryId" property is updated, the record in "ProductCategory" table will be updated to match provided "CategoryId"

*Validation:*

- Required fields are:
  - "Name"
  - "Description"
  - "Price"
  - "CategoryId"
  - "ManufacturerId"

- Length:
  - maximum 50:
    - "Name"
  - maximum 500:
   - "Description"

- Unique name:
  - "Name"

- Greater then zero:
 - "Price"

#### Delete Product

**DELETE /api/products/{id}**

Delete a product from the database.

*Validation:*

- Required field is:
  - "{id}" must be a guid

### Manufacturers

Allows interaction with the Manufacturers capabilities.

#### Get All Manufacturers

**GET /api/manufacturers/all**

Returns the content with all manufacturers.

#### Create Manufacturer

**POST /api/manufacturers**

Adds a new manufacturer to the database.

*Validation:*

- Required fields are:
  - "Name"

- Optional fields are:
  - "WebSite"

- Length:
 - Between 3 and 50 characters:
   - "Name"
 - Less than 20 characters:
   - "Website"

- Unique name:
  - "Name"

  - Required format:
  - Must match url format(e.g. www.example.com, https://microsoft.com):
    - "WebSite"
  - Unappropriate domain names could not be used( e.g. ".ru", ".su"):
    - "WebSite"

This returns the metadata of the added item.

#### Update Manufacturer

**PUT /api/manufacturer/{manufacturerId}**

Updates a manufacturer in the database.

*Validation:*

- Required fields are:
  - "Name"

- Optional fields are:
  - "WebSite"

- Length:
 - Between 3 and 50 characters:
   - "Name"
 - Less than 20 characters:
   - "Website"

- Unique name:
  - "Name"

  - Required format:
  - Must match url format(e.g. www.example.com, https://microsoft.com):
    - "WebSite"
  - Unappropriate domain names could not be used( e.g. ".ru", ".su"):
    - "WebSite"

### Categories

Allows interaction with the Categories capabilities.

#### Get All Categories

**GET /api/categories/all**

Returns the content with all manufacturers.

*Validation:*

- Required fields are:
  - "Name"

#### Create Category

**POST /api/categories**

Adds a new category to the database.

*Validation:*

- Required fields are:
  - "Name"

- Length:
 - Less than 50 characters:
   - "Name"

- Unique name:
  - "Name"

This returns the metadata of the added item.

## 4.5 Use-Cases

The repository can be found here:
https://github.com/Derawwu/IS.ScaleModelsShop