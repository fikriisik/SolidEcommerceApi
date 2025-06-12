namespace SolidEcommerceApi.Models;

public class RevokedToken
{
    public int Id { get; set; }
    public string Jti { get; set; }
    public DateTime ExpiryDate { get; set; }
}