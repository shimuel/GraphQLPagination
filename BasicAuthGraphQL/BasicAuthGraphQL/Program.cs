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
builder.Services.AddSingleton< ISubscriptionService>(new SubscriptionService());

builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("PermissionPolicy", policy => policy.RequireClaim("Permissions", "read", "update"));
    options.AddPolicy(Constants.POLICY_READ, policy =>
        policy.Requirements.Add(new ReadRequirement()));
    options.AddPolicy(Constants.POLICY_UPDATE, policy =>
        policy.Requirements.Add(new WriteRequirement()));
    options.AddPolicy(Constants.POLICY_UI, policy =>
        policy.Requirements.Add(new UIRequirement()));
    options.AddPolicy(Constants.POLICY_SUBSCRIBE, policy =>
        policy.Requirements.Add(new SubscribeRequirement()));
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
    .AddAuthorizationRule() //triggers Authorization -- You may also use .AllowAnonymous() and/or [AllowAnonymous] to allow specific fields to be returned to unauthenticated users within an graph that has an authorization requirement defined.
                            // Note that authorization rules are ignored for input types and fields of input types 
    //.AddWebSocketAuthentication<MyAuthService>()
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

// when the GraphQL endpoint is used in server-to-server communications — but if you wish to ignore these exceptions, 
// app.UseIgnoreDisconnections();

//For Basic Authentication
app.UseAuthentication();
app.UseAuthorization(); /*Must go between UseRouting and UseEndpoints and for GraphQL AddAuthorizationRule + AuthorizationValidationRule + RequirementAuthorizationHandler */

//GraphQL specific
//Multi-schema configuration
//app.UseGraphQL<DogSchema>("/dogs/graphql");
//app.UseGraphQL<CatSchema>("/cats/graphql");
//Be sure to derive from GraphQLHttpMiddleware<TSchema> rather than GraphQLHttpMiddleware as shown above for multi-schema compatibility.
app.UseGraphQL<GraphQLHttpMiddlewareWithLogs<ISchema>>("/graphql", new GraphQLHttpMiddlewareOptions()
{
    //Lots of schema specific settings - GraphQLHttpMiddlewareOptions + GraphQLWebSocketOptions @https://github.com/graphql-dotnet/server 
});
app.UseGraphQLPlayground("/ui/playground",
    new GraphQL.Server.Ui.Playground.PlaygroundOptions
    {
        GraphQLEndPoint = "/graphql",
        SubscriptionsEndPoint = "/graphql",
        RequestCredentials = GraphQL.Server.Ui.Playground.RequestCredentials.Include,
    });

app.UseHttpsRedirection();

// to setup requirement claim(s) based access
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