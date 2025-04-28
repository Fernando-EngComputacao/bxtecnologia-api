namespace BXTecnologia.API.Client.AWS;

public class AwsConfig
{
    public string Region { get; set; }
    
    public string ServiceURL { get; set; }
    public string AccountId { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    
    public string BucketNameS3 { get; set; }
    public string DynamoDB { get; set; }
}
