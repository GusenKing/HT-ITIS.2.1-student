using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var modelType = helper.ViewData.ModelMetadata.ModelType;
        var model = helper.ViewData.Model;
        var resultingHtmlContent = new HtmlContentBuilder();

        foreach (var modelProperty in modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            resultingHtmlContent.AppendHtml("<div>");
            resultingHtmlContent.AppendHtml(CreateLabel(modelProperty));
            resultingHtmlContent.AppendHtml(CreateInputForProperty(modelProperty, model));
            resultingHtmlContent.AppendHtml(CreateValidationField(modelProperty, model));
            resultingHtmlContent.AppendHtml("</div>");
        }

        return resultingHtmlContent;
    }
    
    private static IHtmlContent CreateInputForProperty(PropertyInfo property, object? model)
        => property.PropertyType.IsEnum
            ? CreateDropdownFromEnum(property, model)
            : CreateInputField(property, model == null ? null : property.GetValue(model)?.ToString());

    private static IHtmlContent CreateLabel(PropertyInfo property)
    {
        string? propertyDisplayName = null;
        foreach (var attribute in property.GetCustomAttributes())
        {
            if (attribute is DisplayAttribute displayAttribute)
                propertyDisplayName = displayAttribute.Name;
        }

        if (propertyDisplayName == null)
        {
            propertyDisplayName = FromCamelCaseToDisplayName(property.Name);
        }
        return new HtmlString($"<label for={property.Name}>{propertyDisplayName}</label>");
    }

    private static string FromCamelCaseToDisplayName(string propertyName)
    {
        return Regex.Replace(propertyName, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
    }

    private static IHtmlContent CreateInputField(PropertyInfo property, string? value)
    {
        var type = property.PropertyType == typeof(string) ? "text" : "number"; 
        return new HtmlString($"<input id=\"{property.Name}\" type=\"{type}\" name=\"{property.Name}\" value=\"{value}\"></input>");
    }

    private static IHtmlContent CreateDropdownFromEnum(PropertyInfo property, object? model)
    {
        var hcb = new HtmlContentBuilder();
        hcb.AppendHtml($"<select id=\"{property.Name}\" name=\"{property.Name}\">");
        
        string? selectedName = null;
        if (model != null)
        {
            var modelValue = property.GetValue(model);
            selectedName = property.PropertyType.GetEnumName(modelValue!);
        }

        foreach (var optionName in property.PropertyType.GetEnumNames()) 
        {
            hcb.AppendHtml($"<option value=\"{optionName}\" {(selectedName == optionName ? "selected" : string.Empty)}>{optionName}</option>");
        }
        
        hcb.AppendHtml("</select>");
        return hcb;
    }
    
    private static IHtmlContent CreateValidationField(PropertyInfo property, object? model)
    {
        var hcb = new HtmlContentBuilder();
        hcb.AppendHtml($"<span id=\"{property.Name}\">");
        
        if (model == null) 
            return hcb.AppendHtml("</span>");
        
        var attributes = property.GetCustomAttributes<ValidationAttribute>();
        foreach (var validationAttribute in attributes)
        {
            if (!validationAttribute.IsValid(property.GetValue(model)))
                hcb.Append(validationAttribute.ErrorMessage);
        }

        return hcb.AppendHtml("</span>");
    }
} 
