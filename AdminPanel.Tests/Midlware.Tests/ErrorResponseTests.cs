using AdminPanel.Web.Middleware;

namespace AdminPanel.Tests.Midlware.Tests;

public class ErrorResponseTests
{
    [Fact]
    public void ErrorResponse_StatusCode_ShouldBeSetAndGetCorrectly()
    {
        // Arrange
        var errorResponse = new ErrorResponse();
        int expectedStatusCode = 404;

        // Act
        errorResponse.StatusCode = expectedStatusCode;
        int actualStatusCode = errorResponse.StatusCode;

        // Assert
        Assert.Equal(expectedStatusCode, actualStatusCode);
    }

    [Fact]
    public void ErrorResponse_Message_ShouldBeSetAndGetCorrectly()
    {
        // Arrange
        var errorResponse = new ErrorResponse();
        string expectedMessage = "Resource not found";

        // Act
        errorResponse.Message = expectedMessage;
        string actualMessage = errorResponse.Message;

        // Assert
        Assert.Equal(expectedMessage, actualMessage);
    }

    [Fact]
    public void ErrorResponse_ToString_ShouldReturnValidJson()
    {
        // Arrange
        var errorResponse = new ErrorResponse
        {
            StatusCode = 500,
            Message = "Internal server error",
        };
        string expectedJson = /*lang=json,strict*/ "{\"StatusCode\":500,\"Message\":\"Internal server error\"}";

        // Act
        string actualJson = errorResponse.ToString();

        // Assert
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public void ErrorResponse_ToString_ShouldHandleNullMessage()
    {
        // Arrange
        var errorResponse = new ErrorResponse
        {
            StatusCode = 400,
            Message = null,
        };
        string expectedJson = /*lang=json,strict*/ "{\"StatusCode\":400,\"Message\":null}";

        // Act
        string actualJson = errorResponse.ToString();

        // Assert
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public void ErrorResponse_ToString_ShouldHandleEmptyMessage()
    {
        // Arrange
        var errorResponse = new ErrorResponse
        {
            StatusCode = 403,
            Message = string.Empty,
        };
        string expectedJson = /*lang=json,strict*/ "{\"StatusCode\":403,\"Message\":\"\"}";

        // Act
        string actualJson = errorResponse.ToString();

        // Assert
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public void ErrorResponse_ToString_ShouldHandleZeroStatusCode()
    {
        // Arrange
        var errorResponse = new ErrorResponse
        {
            StatusCode = 0,
            Message = "No status",
        };
        string expectedJson = /*lang=json,strict*/ "{\"StatusCode\":0,\"Message\":\"No status\"}";

        // Act
        string actualJson = errorResponse.ToString();

        // Assert
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public void ErrorResponse_ToString_ShouldHandleNegativeStatusCode()
    {
        // Arrange
        var errorResponse = new ErrorResponse
        {
            StatusCode = -1,
            Message = "Invalid status",
        };
        string expectedJson = /*lang=json,strict*/ "{\"StatusCode\":-1,\"Message\":\"Invalid status\"}";

        // Act
        string actualJson = errorResponse.ToString();

        // Assert
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public void ErrorResponse_ToString_ShouldHandleLargeStatusCode()
    {
        // Arrange
        var errorResponse = new ErrorResponse
        {
            StatusCode = 99999,
            Message = "Custom status",
        };
        string expectedJson = /*lang=json,strict*/ "{\"StatusCode\":99999,\"Message\":\"Custom status\"}";

        // Act
        string actualJson = errorResponse.ToString();

        // Assert
        Assert.Equal(expectedJson, actualJson);
    }
}