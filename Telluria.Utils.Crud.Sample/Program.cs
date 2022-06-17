using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Telluria.Utils.Crud.Sample;
using GQLDI = GraphQL.MicrosoftDI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<DbContext, AppDbContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCommandHandler, ProductCommandHandler>();

// Add schema and register GraphQL
builder.Services.AddSingleton<ISchema, GraphQLMainSchema>(services =>
  new GraphQLMainSchema(new GQLDI.SelfActivatingServiceProvider(services)));

GQLDI.GraphQLBuilderExtensions.AddGraphQL(builder.Services)
  .AddServer(true)
  .ConfigureExecution(options =>
    options.EnableMetrics = true)
  .AddSystemTextJson()
  .AddDataLoader()
  .AddGraphTypes(typeof(GraphQLMainSchema).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();

  app.UseGraphQLGraphiQL();
  app.UseGraphQLPlayground();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseGraphQL<ISchema>();

app.Run();
