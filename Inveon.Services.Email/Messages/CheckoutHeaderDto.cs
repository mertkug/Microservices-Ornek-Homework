using Inveon.MessageBus;
using Inveon.Services.Email.Models.Dto;

namespace Inveon.Services.Email.Messages
{
    public class CheckoutHeaderDto : BaseMessage
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }
        public string CouponCode { get; set; }
        public double OrderTotal { get; set; }
        public double DiscountTotal { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime PickupDateTime { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpiryMonth { get; set; }

        public string ExpiryYear { get; set; }
        public int CartTotalItems { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
        
        public override string ToString()
        {
            var cartDetails = string.Join(", ", CartDetails.Select(c => $"[ProductId: {c.ProductId}"));
            return $"CheckoutHeaderDto: [CartHeaderId={CartHeaderId}, UserId={UserId}, CouponCode={CouponCode}, OrderTotal={OrderTotal}, DiscountTotal={DiscountTotal}, FirstName={FirstName}, LastName={LastName}, PickupDateTime={PickupDateTime}, Phone={Phone}, Email={Email}, CardNumber={CardNumber}, CVV={CVV}, ExpiryMonth={ExpiryMonth}, ExpiryYear={ExpiryYear}, CartTotalItems={CartTotalItems}, CartDetails=[{cartDetails}]]";
        }
    }
}
