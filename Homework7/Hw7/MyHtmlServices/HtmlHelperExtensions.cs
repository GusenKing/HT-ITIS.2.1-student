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

        foreach (var modelProperty in modelType.GetProperties())
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
    {
        if (property.PropertyType == typeof(string))
        {
            return CreateInputField(property.Name, "text", model == null ? null : property.GetValue(model) as string);
        }
        if (property.PropertyType == typeof(int))
        {
            return CreateInputField(property.Name, "number", model == null ? null : (property.GetValue(model) as int?).ToString());
        }
        if (property.PropertyType.IsEnum)
        {
            return CreateDropdownFromEnum(property, model);
        }
        return new HtmlString("This property is not supported yet");
    }

    private static IHtmlContent CreateLabel(PropertyInfo property)
    {
        string? propertyDisplayName = null;
        foreach (var attribute in property.GetCustomAttributes())
        {
            if (attribute is DisplayAttribute)
                propertyDisplayName = (attribute as DisplayAttribute)?.Name;
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

    private static IHtmlContent CreateInputField(string name, string type, string? value)
    {
        return new HtmlString($"<input id=\"{name}\" type=\"{type}\" name=\"{name}\" value=\"{value}\"></input>");
    }

    private static IHtmlContent CreateDropdownFromEnum(PropertyInfo property, object? model)
    {
        var hcb = new HtmlContentBuilder();
        hcb.AppendHtml($"<select id=\"{property.Name}\" name=\"{property.Name}\">");
        foreach (var optionName in property.PropertyType.GetEnumNames()) 
        {
            bool isSelected = false;
            if (model != null)
            {
                var value = property.GetValue(model);
                isSelected = property.PropertyType.GetEnumName(value!) == (string)optionName;
            }
            hcb.AppendHtml($"<option value=\"{optionName}\" {(isSelected ? "selected" : string.Empty)}>{optionName}</option>");
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
            if (validationAttribute.IsValid(property.GetValue(model))) continue;
            hcb.Append(validationAttribute.ErrorMessage);
        }

        return hcb.AppendHtml("</span>");
    }
} 
