using FindFun.Server.Shared;
using FindFun.Server.Shared.File;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace FindFund.Server.IntegrationTest;

public class FileUploadIntegrationTests(WebAplicationCustomFactory factory) : IClassFixture<WebAplicationCustomFactory>
{
    private readonly WebAplicationCustomFactory _factory = factory;

    [Fact]
    public async Task DeleteFileAsync_ShouldFail_WhenFileDontExist()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var fileUpLoad = scope.ServiceProvider.GetRequiredService<FileUpLoad>();

        var results = await WebApplicationTestData.UpLoadFile(fileUpLoad);

        results.Should().ContainSingle();
        var uploaded = results.First();
        uploaded.IsValid.Should().BeTrue();
        uploaded.Data.Should().NotBeNullOrWhiteSpace();

        var relativePath = FileValidation.GetRelativePathFromUrl(uploaded.Data!);

        await FileValidation.DeleteUploadedFilesAsync(results, fileUpLoad, CancellationToken.None);
        var deleteAgain = await fileUpLoad.DeleteFileAsync(relativePath, CancellationToken.None);
        deleteAgain.IsValid.Should().BeFalse();
        deleteAgain.Data.Should().BeFalse();

    }
    [Fact]
    public async Task DeleteFileAsync_DeletesUploadedFile()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var fileUpLoad = scope.ServiceProvider.GetRequiredService<FileUpLoad>();

        var results = await WebApplicationTestData.UpLoadFile(fileUpLoad);

        results.Should().ContainSingle();
        var uploaded = results.First();
        uploaded.IsValid.Should().BeTrue();
        uploaded.Data.Should().NotBeNullOrWhiteSpace();

        var relativePath = FileValidation.GetRelativePathFromUrl(uploaded.Data!);
        var deleteResult = await fileUpLoad.DeleteFileAsync(relativePath, CancellationToken.None);

        deleteResult.IsValid.Should().BeTrue();
        deleteResult.Data.Should().BeTrue();

        var deleteAgain = await fileUpLoad.DeleteFileAsync(relativePath, CancellationToken.None);
        deleteAgain.IsValid.Should().BeFalse();
        deleteAgain.Data.Should().BeFalse();
    }

    [Fact]
    public async Task FilesUpLoader_UploadsFile_ReturnsUrl()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var fileUpLoad = scope.ServiceProvider.GetRequiredService<FileUpLoad>();
        List<Result<string>> results = await WebApplicationTestData.UpLoadFile(fileUpLoad);

        // Assert
        results.Should().NotBeNull().And.HaveCount(1);
        results.First().IsValid.Should().BeTrue();
        results.First().Data.Should().NotBeNullOrWhiteSpace();
        Uri.IsWellFormedUriString(results.First().Data, UriKind.Absolute).Should().BeTrue();
    }

}
