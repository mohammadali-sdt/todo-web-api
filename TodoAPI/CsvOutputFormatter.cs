using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace TodoAPI;

public class CsvOutputFormatter : TextOutputFormatter
{


    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (typeof(UserDto).IsAssignableFrom(type) || typeof(IEnumerable<UserDto>).IsAssignableFrom(type))
        {
            return base.CanWriteType(type);
        }

        return false;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selecteEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        if (context.Object is IEnumerable<UserDto> dtos)
        {
            foreach (var user in dtos)
            {
                FormatCsv(buffer, user);
            }
        }
        else
        {
            if(context.Object != null)
                FormatCsv(buffer, (UserDto)context.Object);
        }

        await response.WriteAsync(buffer.ToString());
    }

    private static void FormatCsv(StringBuilder buffer, UserDto user)
    {
        buffer.AppendLine($"{user.Id}, \"{user.Name}\", \"{user.Username}\", \"{user.Email}\", \"{user.Age}\"");
    }
    
    
}