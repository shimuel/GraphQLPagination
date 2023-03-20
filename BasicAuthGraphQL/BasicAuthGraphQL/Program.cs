using System.Reflection;
using GraphQL;
using BasicAuthGraphQL.Security;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using GraphQL.Validation;
using BasicAuthGraphQL;
using BasicAuthGraphQL.PubRepo;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Types;
using BasicAuthGraphQL.Schema.Pubs;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", corbuilder =>
    {
        corbuilder.AllowCredentials();
        corbuilder.WithOrigins("https://localhost:7266/");
    });
});
builder.Services.AddSingleton<AuthorRepo>();
builder.Services.AddSingleton<BookRepo>();

builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("PermissionPolicy", policy => policy.RequireClaim("Permissions", "read", "update"));
    options.AddPolicy(Constants.POLICY_READ, policy =>
        policy.Requirements.Add(new ReadRequirement()));
    options.AddPolicy(Constants.POLICY_UPDATE, policy =>
        policy.Requirements.Add(new WriteRequirement()));
    options.AddPolicy(Constants.POLICY_UI, policy =>
        policy.Requirements.Add(new UIRequirement()));
});

//For Basic Authentication /Authorization
builder.Services.AddSingleton<IAuthorizationHandler, RequirementAuthorizationHandler>();
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
        ("BasicAuthentication", null);


builder.Services
    .AddTransient<IValidationRule, GraphQL.Server.Transports.AspNetCore.AuthorizationValidationRule>();

builder.Services.AddGraphQL(graphqlBuilder => graphqlBuilder
    .AddSchema<PubsSchema>()
    .AddErrorInfoProvider(options => options.ExposeExceptionStackTrace = builder.Environment.IsDevelopment())
    .AddGraphTypes(Assembly.GetExecutingAssembly())
    .AddSystemTextJson()
    .AddAuthorizationRule() //triggers Authorization
    .AddUserContextBuilder(httpContext =>
    {
        return new GraphQLUserContext(httpContext);
    }).ConfigureExecutionOptions(options =>
    {
        var logger = options.RequestServices!.GetRequiredService<ILogger<Program>>();
        options.UnhandledExceptionDelegate = ctx =>
        {
            logger.LogError("{Error} occurred", ctx.OriginalException.Message);
            return Task.CompletedTask;
        };
    })
    .AddErrorInfoProvider<CustomErrorInfoProvider>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebSockets();
app.UseRouting();
app.UseCors();
//For Basic Authentication
app.UseAuthentication();
app.UseAuthorization(); /*Must go between UseRouting and UseEndpoints*/

//app.UseGraphQL<MyMiddleware<ISchema>>("/ui/playground", new GraphQLHttpMiddlewareOptions());
//app.UseGraphQL<ISchema>();
app.UseGraphQL<GraphQLHttpMiddlewareWithLogs<ISchema>>("/graphql", new GraphQLHttpMiddlewareOptions() {});
app.UseGraphQLPlayground("/ui/playground",
    new GraphQL.Server.Ui.Playground.PlaygroundOptions
    {
        GraphQLEndPoint = "/graphql",
        SubscriptionsEndPoint = "/graphql",
        RequestCredentials = GraphQL.Server.Ui.Playground.RequestCredentials.Include,
    });

app.UseHttpsRedirection();

app.MapGet("/login", [Microsoft.AspNetCore.Authorization.Authorize] () => true).WithName("login");

app.UseEndpoints(endpoints =>
{
    // configure the graphql endpoint at "/graphql"
    endpoints.MapGraphQL("/graphql")
        .RequireCors("MyCorsPolicy");
    // configure Playground at "/"
    //endpoints.MapGraphQLPlayground("/ui/playground");
});

//app.MapGet("/ui/playground", [Microsoft.AspNetCore.Authorization.Authorize(Policy = Constants.POLICY_UI)] () =>
//{

//}).WithName("playground");

//app.MapGet("/graphql", [Microsoft.AspNetCore.Authorization.Authorize()] () =>
//    {

//    }).WithName("graphql");

app.Run();