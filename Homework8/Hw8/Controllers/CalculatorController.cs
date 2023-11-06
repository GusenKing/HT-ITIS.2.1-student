using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Hw8.Parsers;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] ICalculator calculator,
        string val1,
        string operation,
        string val2)
    {
        var parsedValues = ParserForCalculator.ParseParams(val1, operation, val2);
        ActionResult<double> result;
        switch (parsedValues.Item2)
        {
            case Operation.Plus:
                result =  calculator.Plus(parsedValues.Item1, parsedValues.Item3);
                break;
            case Operation.Minus:
                result =  calculator.Minus(parsedValues.Item1, parsedValues.Item3);
                break;
            case Operation.Multiply:
                result =  calculator.Multiply(parsedValues.Item1, parsedValues.Item3);
                break;
            case Operation.Divide:
            {
                if (parsedValues.Item3 == 0.0)
                    throw new ArgumentException(Messages.DivisionByZeroMessage);
                result = calculator.Divide(parsedValues.Item1, parsedValues.Item3);
                break;
            }
            default:
                result = double.NaN;
                break;
        }

        return result;
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}