using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Shouldly;
using Wax.Api;
using Wax.Messages.Commands.Customers;
using Xunit;

namespace Wax.E2ETests
{
    public class GlobalExceptionTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public GlobalExceptionTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ShouldReturn400StatusCodeWhenInvalidCommand()
        {
            var response = await PostAsync("customers", new CreateCustomerCommand { });

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();

            var problemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(content);

            problemDetails.Title.ShouldBe("One or more validation errors occurred.");
            problemDetails.Errors.Count.ShouldBeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public async Task ShouldReturn404StatusCodeWhenEntityNotFound()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/customers/{Guid.NewGuid()}");

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            var content = await response.Content.ReadAsStringAsync();

            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(content);

            problemDetails.Title.ShouldBe("The specified resource was not found.");
            problemDetails.Detail.ShouldStartWith("Entity not found");
        }

        [Fact]
        public async Task ShouldReturn409StatusCodeWhenBusinessError()
        {
            var message = new CreateCustomerCommand
            {
                Name = "Microsoft",
                Address = "Microsoft Corporation One Microsoft Way Redmond, WA 98052-6399 USA",
                Contact = "(800) 426-9400"
            };

            var response = await PostAsync("customers", message);

            response.EnsureSuccessStatusCode();

            response = await PostAsync("customers", message);

            response.StatusCode.ShouldBe(HttpStatusCode.Conflict);

            var content = await response.Content.ReadAsStringAsync();

            var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(content);

            problemDetails.Title.ShouldBe("A business error occur.");
            problemDetails.Detail.ShouldBe("Customer with this name already exists.");
        }

        private async Task<HttpResponseMessage> PostAsync<T>(string url, T body) where T : class
        {
            var client = _factory.CreateClient();

            var response = await client.PostAsync(url,
                new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));

            return response;
        }
    }
}