using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UserDb>(opt => opt.UseInMemoryDatabase("TechTest"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSingleton<Crypto>();
var app = builder.Build();


//endpoint 4
app.MapGet("/Users", async (UserDb db) =>
    await db.Users.ToListAsync());

//endpoint 1
app.MapPost("/Users", async (User user, UserDb db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/user/{user.Id}", user);
});

//endpoint 2
app.MapPost("/Encrypt", (EncryptRequest encrypt, Crypto crypto ) =>
{
    //logica encriptado
    string encryptedText = crypto.encrypt(encrypt.Text);
    return new { encrypted = encryptedText };
});

//endpoint 3
app.MapPost("/Decrypt", (DecryptRequest decrypt, Crypto crypto ) =>
{
    //logica desencriptado
    string decryptedText = crypto.decrypt(decrypt.Encrypted);
    return new {text = decryptedText};
});

app.Run();