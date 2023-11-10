using Inveon.Services.Email.Models.Dto;

namespace Inveon.Services.Email.Models;

public class CheckoutMailModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public double OrderTotal { get; set; }
    public IEnumerable<CartDetailsDto> CartDetails { get; set; }
}