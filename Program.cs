using InmindLab3LibraryDB.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using InmindLab3LibraryDB.Models;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

// Add controllers with OData support.
builder.Services.AddControllers().AddOData(opt =>
    opt.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100)
        .AddRouteComponents("odata", GetEdmModel()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure PostgreSQL using connection string from appsettings.json
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LibraryContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    // Expose entity sets for OData endpoints.
    builder.EntitySet<Book>("Books");
    builder.EntitySet<Author>("Authors");
    builder.EntitySet<Borrowers>("Borrowers");
    builder.EntitySet<Loan>("Loans");
    return builder.GetEdmModel();
}