using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2.DataModel;
using BXTecnologia.API.Utils;

namespace BXTecnologia.API.Models.Customer;

[DynamoDBTable("customers")]
public class Customer
{
    [DynamoDBHashKey("id")]
    [Required]
    [JsonConverter(typeof(GuidToStringConverter))]
    public Guid Id { get; set; } = Guid.NewGuid();
    [DynamoDBProperty("fullname")]
    [Required]
    public string FullName { get; set; }
    [DynamoDBProperty("email")]
    [Required]
    public string Email { get; set; }
    [DynamoDBProperty("dateofbirth")]
    [Required]
    public DateTime DateOfBirth { get; set; }
    [DynamoDBProperty("level")]
    [Required]
    public string Level { get; set; }
    [DynamoDBProperty("role")]
    [Required]
    public string Role { get; set; }
    [DynamoDBProperty("updated_at")]
    [Required]
    public DateTime UpdatedAt { get; set; }
}