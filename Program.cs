using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using BXTecnologia.API.Client;
using BXTecnologia.API.Client.AWS;
using BXTecnologia.API.Profiles;
using BXTecnologia.API.Repositories;
using BXTecnologia.API.Repositories.Interfaces;
using BXTecnologia.API.Services;
using BXTecnologia.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var awsOptions = builder.Configuration.GetSection("AWS").Get<AwsConfig>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Email>(builder.Configuration.GetSection("EMAIL"));
builder.Services.AddSingleton<IAmazonS3>(sp => {
    var credentials = new BasicAWSCredentials(awsOptions.AccessKey, awsOptions.SecretKey, awsOptions.AccountId);
    var config = new AmazonS3Config()
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(awsOptions.Region)
    };
    return new AmazonS3Client(credentials, config);
});
builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>  {
    var credentials = new BasicAWSCredentials(awsOptions.AccessKey, awsOptions.SecretKey, awsOptions.AccountId);
    var config = new AmazonDynamoDBConfig
    {
        RegionEndpoint = RegionEndpoint.GetBySystemName(awsOptions.Region)
    };
    return new AmazonDynamoDBClient(credentials, config);
});
builder.Services.AddSingleton<ICustomerImageService, CustomerImageService>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddAutoMapper(typeof(Profiles));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
