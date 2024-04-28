# Azure Function SQL CRUD

This project demonstrates how to create a simple Azure Function to perform CRUD (Create, Read, Update, Delete) operations on a SQL database using HTTP triggers.

## Overview

The project consists of an Azure Function called `StockAPI` which provides an HTTP endpoint to create a new stock entry in the database. The function receives a JSON payload containing the details of the stock to be added, validates it, and then inserts the data into the SQL database.

## Functionality

The `StockAPI` function has the following features:

- **Create-Stock Endpoint**: An HTTP POST endpoint (`Create-Stock`) is exposed to create a new stock entry.
- **Input Validation**: The function validates the incoming payload to ensure it contains the necessary data.
- **Database Interaction**: The function interacts with a SQL database to insert the new stock entry.

## Prerequisites

Before running this Azure Function, ensure the following:

- Azure subscription
- Azure Function App created
- SQL Database provisioned
- Connection string to the SQL Database available in the Azure Function environment variables

## Setup

1. Clone this repository to your local machine.
2. Configure the Azure Function App settings with the required connection string to the SQL Database.
3. Deploy the Azure Function to your Azure Function App.

## Usage

To use the `Create-Stock` endpoint:

1. Send an HTTP POST request to the endpoint URL with a JSON payload containing the details of the stock to be added.
2. The function will validate the payload and insert the data into the SQL Database.
3. Upon successful insertion, the function will return a 200 OK response with the details of the newly created stock entry.

Example payload:
```json
{
  "StockName": "Example Stock",
  "Price": 10.5
}
