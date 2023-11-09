using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Hw8.Parsers;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public IActionResult Calculate([FromServices] ICalculator calculator,
        string val1,
        string operation,
        string val2)
    {
        if (!double.TryParse(val1, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedVal1))
            return new ObjectResult(Messages.InvalidNumberMessage);
        if (!Enum.TryParse(operation, out Operation parsedOperation))
            return new ObjectResult(Messages.InvalidOperationMessage);
        if (!double.TryParse(val2, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedVal2))
            return new ObjectResult(Messages.InvalidNumberMessage);

        return parsedOperation switch
        {
            Operation.Plus => new ObjectResult(calculator.Plus(parsedVal1, parsedVal2)),
            Operation.Minus => new ObjectResult(calculator.Minus(parsedVal1, parsedVal2)),
            Operation.Multiply => new ObjectResult(calculator.Multiply(parsedVal1, parsedVal2)),
            Operation.Divide => parsedVal2 == 0.0
                ? new ObjectResult(Messages.DivisionByZeroMessage)
                : new ObjectResult(calculator.Divide(parsedVal1, parsedVal2)),
            _ => new ObjectResult(Messages.InvalidOperationMessage)
        };
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}