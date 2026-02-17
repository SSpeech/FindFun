using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace FindFund.Server.IntegrationTest;
public class CreateParkIntegrationTests : IClassFixture<WebAplicationCustomFactory>
{
    private readonly WebAplicationCustomFactory _factory;
    private readonly HttpClient _httpClient;
    public CreateParkIntegrationTests(WebAplicationCustomFactory factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();
    }

    [Theory]
    [MemberData(nameof(validFileData))]
    public async Task CreatePark_ShouldCreatesPark_WhenValidDataProvided_ReturnsId(string formFieldName, string fileName, byte[] fileBytes, string contentType)
    {
        HttpResponseMessage response = await PostAsync(formFieldName, fileName, fileBytes, contentType);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var createdId = JsonSerializer.Deserialize<int>(responseString);
        createdId.Should().BeGreaterThan(0);
    }

    [Theory]
    [MemberData(nameof(InvalidFileData))]
    public async Task CreatePark_ShouldReturnBadRequest_WhenInvalidFileProvided(string formFieldName, string fileName, byte[] fileBytes, string contentType, string expectedErrorMessage)
    {
      var response = await PostAsync(formFieldName, fileName, fileBytes, contentType);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var validationProblemDetails  = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        validationProblemDetails.Should().NotBeNull();
        validationProblemDetails.Status.Should().Be((int)response.StatusCode);
        validationProblemDetails.Title.Should().Be("Bad Request");
        validationProblemDetails!.Errors.Should().ContainKey("file").And.HaveCount(1);
        var fileErrors = validationProblemDetails.Errors["file"];
        fileErrors.Should().HaveCount(1).And.Contain(expectedErrorMessage);
    }
    private async Task<HttpResponseMessage> PostAsync(string formFieldName, string fileName, byte[] fileBytes, string contentType)
    {
        await _factory.AddMunicipality();

        var multipart = CreateBaseMultipart(_factory.MunicipalityName);

        var byteContent = new ByteArrayContent(fileBytes);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        multipart.Add(byteContent, formFieldName, fileName);

        var response = await _httpClient.PostAsync("/parks", multipart);
        return response;
    }


    public static TheoryData<string, string, byte[], string, string> InvalidFileData() => new()
    {
     { "ParkImages", "invalid.txt", new byte[] { 0x00, 0x01, 0x02 }, "text/plain", "file has an invalid file extension." },
     { "ParkImages", "empty.png", new byte[0], "image/png", "file exceeded or is below the permitted size." }
    };
    public static TheoryData<string, string, byte[], string> validFileData() => new()
    {
     { "ParkImages", "invalid.webp", new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }, "image/webp"},
     { "ParkImages", "empty.png", new byte[]{137, 80, 78, 71, 13, 10, 26, 10}, "image/png"}
    };

    private static MultipartFormDataContent CreateBaseMultipart(string locality, string parkName = "Test Park")
    {
        return new MultipartFormDataContent
        {
            { new StringContent(parkName), "Name" },
            { new StringContent("A nice park"), "Description" },
            { new StringContent("Tester"), "Organizer" },
            { new StringContent("false"), "IsFree" },
            { new StringContent("5.00"), "EntranceFee" },
            { new StringContent("Playground:Children area"), "Amenities" },
            { new StringContent("Public"), "ParkType" },
            { new StringContent(string.Empty), "ClosingSchedule" },
            { new StringContent("-3.70379,40.41678"), "Coordinates" },
            { new StringContent("Some formatted address"), "FormattedAddress" },
            { new StringContent("Main Street"), "Street" },
            { new StringContent("1"), "Number" },
            { new StringContent("ABC123"), "AgeRecommendation" },
            { new StringContent(locality), "Locality" },
            { new StringContent("12345"), "PostalCode" }
        };
    }


}

