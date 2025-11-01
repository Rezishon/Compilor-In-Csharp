internal class Program
{
    private static void Main(string[] args)
    {
        string filePath = "/home/rezishon/Projects/Compilor-In-Csharp/sample.bsh";
        const string MyAllowAllOriginsPolicy = "_myAllowAllOrigins";

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                name: MyAllowAllOriginsPolicy,
                policy =>
                {
                    // The simplest and most permissive setting:
                    // This is the "for all" part you requested.
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                }
            );
        });

        var app = builder.Build();
        app.UseCors(MyAllowAllOriginsPolicy);

        app.MapGet("/", () => "Hello World!");

        app.MapPost(
            "/uploadfile",
            (FileUploadRequest request) =>
            {
                if (string.IsNullOrEmpty(request.Base64Data))
                {
                    return Results.BadRequest("Base64 data is required.");
                }

                try
                {
                    byte[] fileBytes = Convert.FromBase64String(request.Base64Data);

                    var fileName = request.FileName;
                    File.WriteAllBytes(
                        "/home/rezishon/Projects/Compilor-In-Csharp/sample.bsh",
                        fileBytes
                    );

                    return Results.Ok(
                        new { Message = $"فایل شما با سایز {fileBytes.Length}بایت ذخیره شد" }
                    );
                }
                catch (FormatException)
                {
                    return Results.BadRequest("The provided string is not a valid Base64 format.");
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        $"An unexpected error occurred during file processing.\n{ex}",
                        statusCode: StatusCodes.Status500InternalServerError
                    );
                }
            }
        );

        app.Run();
    }
}

public class FileUploadRequest
{
    public required string Base64Data { get; set; }
    public string FileName { get; set; } = "sample.bsh";
}
