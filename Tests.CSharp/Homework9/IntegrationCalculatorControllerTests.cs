using System.Globalization;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using Hw9.Dto;
using Hw9.ErrorMessages;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.RunLogic.Attributes;

namespace Tests.CSharp.Homework9;

public class IntegrationCalculatorControllerTests : IClassFixture<WebApplicationFactory<Hw9.Program>>
{
    private readonly HttpClient _client;

    public IntegrationCalculatorControllerTests(WebApplicationFactory<Hw9.Program> fixture)
    {
        _client = fixture.CreateClient();
    }

    [HomeworkTheory(Homeworks.HomeWork9)]
    [InlineData("10", "10")]
    [InlineData("2 + 3", "5")]
    [InlineData("(10 - 3) * 2", "14")]
    [InlineData("3 - 4 / 2", "1")]
    [InlineData("8 * (2 + 2) - 3 * 4", "20")]
    [InlineData("10 - 3 * (-4)", "22")]
    [InlineData("(-(3 * 4)) * (5 + 5)", "-120")]
    [InlineData("(-(-(5 + 5))) * 10", "100")]
    public async Task Calculate_CalculateExpression_Success(string expression, string result)
    {
        var response = await CalculateAsync(expression);
        Assert.True(response!.IsSuccess);
        Assert.Equal(result, response.Result.ToString(CultureInfo.InvariantCulture));
    }

    [HomeworkTheory(Homeworks.HomeWork9)]
    [InlineData(null, MathErrorMessager.EmptyString)]
    [InlineData("", MathErrorMessager.EmptyString)]
    [InlineData("10 + i", $"{MathErrorMessager.UnknownCharacter} i")]
    [InlineData("10 : 2", $"{MathErrorMessager.UnknownCharacter} :")]
    [InlineData("3 - 4 / 2.2.3", $"{MathErrorMessager.NotNumber} 2.2.3")]
    [InlineData("2 - 2.23.1 - 23", $"{MathErrorMessager.NotNumber} 2.23.1")]
    [InlineData("8 - / 2", $"{MathErrorMessager.TwoOperationInRow} - and /")]
    [InlineData("8 + (34 - + 2)", $"{MathErrorMessager.TwoOperationInRow} - and +")]
    [InlineData("4 - 10 * (/10 + 2)", $"{MathErrorMessager.InvalidOperatorAfterParenthesis} (/")]
    [InlineData("10 - 2 * (10 - 1 /)", $"{MathErrorMessager.OperationBeforeParenthesis} /)")]
    [InlineData(")10 + 6) * 3", MathErrorMessager.IncorrectBracketsNumber)]
    [InlineData("* 10 + 2", MathErrorMessager.StartingWithOperation)]
    [InlineData("10 + 2 -", MathErrorMessager.EndingWithOperation)]
    [InlineData("((10 + 2)", MathErrorMessager.IncorrectBracketsNumber)]
    [InlineData("(10 - 2))", MathErrorMessager.IncorrectBracketsNumber)]
    [InlineData("10 / 0", MathErrorMessager.DivisionByZero)]
    [InlineData("10 / (1 - 1)", MathErrorMessager.DivisionByZero)]
    public async Task Calculate_CalculateExpression_Error(string expression, string result)
    {
        var response = await CalculateAsync(expression);
        Assert.False(response!.IsSuccess);
        Assert.Equal(result, response.ErrorMessage);
    }

    private async Task<CalculationMathExpressionResultDto?> CalculateAsync(string expression)
    {
        var response = await _client.PostCalculateExpressionAsync(expression);
        return await response.Content.ReadFromJsonAsync<CalculationMathExpressionResultDto>();
    }
}