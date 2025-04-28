using System.Net;
using System.Text.Json;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using AutoMapper;
using BXTecnologia.API.Client.AWS;
using BXTecnologia.API.Models.Customer;
using BXTecnologia.API.Models.Customer.DTO;
using BXTecnologia.API.Repositories.Interfaces;
using BXTecnologia.API.Utils;
using Microsoft.Extensions.Options;

namespace BXTecnologia.API.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;
    private readonly AwsConfig _awsConfig;
    private readonly IMapper _mapper;
    
    public CustomerRepository(
        IAmazonDynamoDB dynamoDb, 
        IMapper mapper, 
        IOptions<AwsConfig> awsConfig)
    {
        _awsConfig = awsConfig.Value;
        _dynamoDb = dynamoDb;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(CreateCustomerDTO customerDTO)
    {
        var customer = _mapper.Map<Customer>(customerDTO);
        customer.UpdatedAt = DateTime.UtcNow;
        
        var options = new JsonSerializerOptions
        {
            Converters = { new GuidToStringConverter() }
        };
        
        var customerAsJson = JsonSerializer.Serialize(customer, options);
        var itemAsDocument = Document.FromJson(customerAsJson);
        var itemAsAttributes = itemAsDocument.ToAttributeMap();

        var createItemRequest = new PutItemRequest()
        {
            TableName = _awsConfig.DynamoDB,
            Item = itemAsAttributes
        };

        try
        {
            var response = await _dynamoDb.PutItemAsync(createItemRequest);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error creating item: {ex.Message}", ex);
        }

    }

    public async Task<ReadCustomerDTO?> GetAsync(Guid id)
    {
        var request = new GetItemRequest
        {
            TableName = _awsConfig.DynamoDB,
            Key = new Dictionary<string, AttributeValue>
        {
            { "Id", new AttributeValue { S = id.ToString() } }
        }
        };

        var response = await _dynamoDb.GetItemAsync(request);
        if (response.Item == null)
        {
            return null;
        }

        var itemAsDocument = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<ReadCustomerDTO>(itemAsDocument.ToJson());
    }

    public async Task<IEnumerable<ReadCustomerDTO>> GetAllAsync()
    {
        var scanRequest = new ScanRequest
        {
            TableName = _awsConfig.DynamoDB
        };
        var response = await _dynamoDb.ScanAsync(scanRequest);
        return response.Items
            .Select(x =>
            {
                var attributeMap = Document.FromAttributeMap(x);
                var json = attributeMap.ToJson();
                return JsonSerializer.Deserialize<ReadCustomerDTO>(json);
            })!;
    }
    public async Task<bool> UpdateAsync(UpdateCustomerDTO customerDTO, DateTime requestStarted)
    {
        var customer = _mapper.Map<Customer>(customerDTO);
        customer.UpdatedAt = DateTime.UtcNow;
        var customerAsJson = JsonSerializer.Serialize(customer);
        var itemAsDocument = Document.FromJson(customerAsJson);
        var itemAsAttributes = itemAsDocument.ToAttributeMap();

        var updateItemRequest = new PutItemRequest
        {
            TableName = _awsConfig.DynamoDB,
            Item = itemAsAttributes,
            ConditionExpression = "UpdatedAt < :requestStarted",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":requestStarted", new AttributeValue { S = requestStarted.ToString("O") } }
            }
        };

        var response = await _dynamoDb.PutItemAsync(updateItemRequest);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleteItemRequest = new DeleteItemRequest
        {
            TableName = _awsConfig.DynamoDB,
            Key = new Dictionary<string, AttributeValue>
            {
                { "Id", new AttributeValue { S = id.ToString() } },
            }
        };

        var response = await _dynamoDb.DeleteItemAsync(deleteItemRequest);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    private async Task<string?> LastUpdatedDateForCustomer(string id)
    {
        var request = new GetItemRequest
        {
            TableName = _awsConfig.DynamoDB,
            Key = new Dictionary<string, AttributeValue>
            {
                { "Id", new AttributeValue { S = id } }
            },
            AttributesToGet = new List<string> { nameof(ReadCustomerDTO.UpdatedAt) }
        };

        var response = await _dynamoDb.GetItemAsync(request);
        if (response.Item.Count == 0)
        {
            return null;
        }

        return response.Item[nameof(ReadCustomerDTO.UpdatedAt)].S;
    }
}