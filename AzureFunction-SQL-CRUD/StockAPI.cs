using AzureFunction_SQL_CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AzureFunction_SQL_CRUD
{
	public class StockAPI
	{
		private readonly ILogger<StockAPI> _logger;

		public StockAPI(ILogger<StockAPI> logger)
		{
			_logger = logger;
		}

		[Function("GetAllStock")]
		public async Task<IActionResult> GetAllStock([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
		{
			string? connstring = Environment.GetEnvironmentVariable("connectionstring");
			if (connstring == null)
			{
				return new BadRequestObjectResult("Connectionstring is empty");
			}
			var stocks = new List<Stock>();
			using (SqlConnection conn = new SqlConnection(connstring))
			{
				SqlCommand cmd = new SqlCommand("Select * from dbo.Stocks order by StockId", conn);
				await conn.OpenAsync();
				using (var reader = await cmd.ExecuteReaderAsync())
				{
					while (reader.Read())
					{
						var stock = new Stock
						{
							StockId = reader.GetInt32(reader.GetOrdinal("StockId")),
							StockName = reader.GetString(reader.GetOrdinal("StockName")),
							Price = reader.GetDecimal(reader.GetOrdinal("Price"))
						};
						stocks.Add(stock);
					}

				}
				return new OkObjectResult(stocks);

			}
		}

		[Function("Create-Stock")]
		public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
		{
			_logger.LogInformation("C# HTTP trigger function processed a request.");

			var body = await req.ReadFromJsonAsync<CreateStock>();
			if (body == null)
			{
				return new BadRequestObjectResult("Invalid Payload");
			}
			try
			{
				await AddStock(body);

				return new OkObjectResult(body);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message, ex);
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}
		}

		private async Task AddStock(CreateStock stock)
		{
			string? connectionstring = Environment.GetEnvironmentVariable("connectionstring");
			if (string.IsNullOrEmpty(connectionstring))
			{
				throw new Exception("Connectionstring is empty");
			}


			using (SqlConnection conn = new SqlConnection(connectionstring))
			{
				string querystring = "INSERT INTO dbo.Stocks (StockName,Price) VALUES (@StockName,@Price)";
				SqlCommand cmd = new SqlCommand(querystring, conn);
				cmd.Parameters.AddWithValue("@StockName", stock.StockName);
				cmd.Parameters.AddWithValue("@Price", stock.Price);
				await conn.OpenAsync();
				await cmd.ExecuteNonQueryAsync();
			}

		}
	}
}
