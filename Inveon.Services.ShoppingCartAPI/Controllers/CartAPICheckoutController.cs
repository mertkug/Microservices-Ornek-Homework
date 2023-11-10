using System.Globalization;
using Inveon.Services.ShoppingCartAPI.Messages;
using Inveon.Services.ShoppingCartAPI.Models.Dto;
using Inveon.Services.ShoppingCartAPI.RabbitMQ;
using Inveon.Services.ShoppingCartAPI.Repository;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inveon.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cartc")]
    public class CartAPICheckOutController : ControllerBase
    {

        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        // private readonly IMessageBus _messageBus;
        protected ResponseDto _response;
        private readonly IRabbitMQCartMessageSender _rabbitMQCartMessageSender;
        // IMessageBus messageBus,
        public CartAPICheckOutController(ICartRepository cartRepository,
            ICouponRepository couponRepository, IRabbitMQCartMessageSender rabbitMQCartMessageSender)
        {
            _cartRepository = cartRepository;
            _couponRepository = couponRepository;
            _rabbitMQCartMessageSender = rabbitMQCartMessageSender;
            //_messageBus = messageBus;
            this._response = new ResponseDto();
        }

        [HttpPost]
        [Authorize]
        public async Task<object> Checkout([FromBody] CheckoutHeaderDto checkoutHeader)
        {
            var discount = 0.0;
            try
            {
                CartDto cartDto = await _cartRepository.GetCartByUserId(checkoutHeader.UserId);
                if (cartDto == null)
                {
                    return BadRequest();
                }

                if (!string.IsNullOrEmpty(checkoutHeader.CouponCode))
                {
                    CouponDto coupon = await _couponRepository.GetCoupon(checkoutHeader.CouponCode);
                    if (checkoutHeader.DiscountTotal != coupon.DiscountAmount)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessages = new List<string>() { "Coupon Price has changed, please confirm" };
                        _response.DisplayMessage = "Coupon Price has changed, please confirm";
                        return _response;
                    }
                }

                checkoutHeader.CartDetails = cartDto.CartDetails;
                //logic to add message to process order.
                // await _messageBus.PublishMessage(checkoutHeader, "checkoutqueue");

                ////rabbitMQ

                Payment payment = OdemeIslemi(checkoutHeader);
                _rabbitMQCartMessageSender.SendMessage(checkoutHeader, "checkoutqueue");
                await _cartRepository.ClearCart(checkoutHeader.UserId);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        public Payment OdemeIslemi(CheckoutHeaderDto checkoutHeaderDto)
        {

            var cartDto = _cartRepository.GetCartByUserIdNonAsync(checkoutHeaderDto.UserId);

            var options = new Options
            {
                ApiKey = "sandbox-wXcRDfJDJXGow4mDnmD8JshX3atJu3Y4",
                SecretKey = "sandbox-zJ9dLZZrgIiHhtFlHBWESJit7YYfijNk",
                BaseUrl = "https://sandbox-api.iyzipay.com"
            };

            var request = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = new Random().Next(1111, 9999).ToString(),
                PaidPrice = "1.2",
                //request.Price = "15";//checkoutHeaderDto.OrderTotal.ToString();
                //request.PaidPrice = "15";//checkoutHeaderDto.OrderTotal.ToString();
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                BasketId = "B67832"
            };
            request.BasketId = checkoutHeaderDto.CartHeaderId.ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            var paymentCard = new PaymentCard
            {
                CardHolderName = checkoutHeaderDto.CartHeaderId.ToString(),
                CardNumber = checkoutHeaderDto.CardNumber,
                ExpireMonth = checkoutHeaderDto.ExpiryMonth,
                ExpireYear = checkoutHeaderDto.ExpiryYear,
                Cvc = checkoutHeaderDto.CVV,
                RegisterCard = 0,
                CardAlias = "Inveon"
            };
            request.PaymentCard = paymentCard;

            var buyer = new Buyer
            {
                //buyer.Id = cartDto.CartHeader.UserId;
                Id = "BY789",
                Name = checkoutHeaderDto.FirstName,
                Surname = checkoutHeaderDto.LastName,
                GsmNumber = checkoutHeaderDto.Phone,
                Email = checkoutHeaderDto.Email,
                IdentityNumber = "74300864791",
                LastLoginDate = "2015-10-05 12:43:35",
                RegistrationDate = "2013-04-21 15:12:09",
                RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                Ip = "85.34.78.112",
                City = "Istanbul",
                Country = "Turkey",
                ZipCode = "34732"
            };
            request.Buyer = buyer;

            var shippingAddress = new Address
            {
                ContactName = $"{checkoutHeaderDto.FirstName} {checkoutHeaderDto.LastName}",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                ZipCode = "34742"
            };
            request.ShippingAddress = shippingAddress;

            var billingAddress = new Address
            {
                ContactName = $"{checkoutHeaderDto.FirstName} {checkoutHeaderDto.LastName}",
                City = "Istanbul",
                Country = "Turkey",
                Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1",
                ZipCode = "34742"
            };
            request.BillingAddress = billingAddress;

            var basketItems = new List<BasketItem>();
            
            var us = new CultureInfo("en-US");
            var numberFormat = us.NumberFormat;
            var decimalSeparator = numberFormat.NumberDecimalSeparator;
            var basket = checkoutHeaderDto.CartDetails;
            var totalPrice = 0.0M;
            foreach (var cartDetails in basket)
            {
                var product = cartDetails.Product;
                var item = new BasketItem();
                item.Id = $"B11{product.ProductId.ToString()}";
                item.Name = product.Name;
                item.Category1 = product.CategoryName;
                item.ItemType = BasketItemType.PHYSICAL.ToString();
                item.Price = (product.Price * cartDetails.Count).ToString("F2", CultureInfo.InvariantCulture);
                basketItems.Add(item);
                request.BasketItems = basketItems;
                Console.WriteLine(item.Price);
            }
            Console.WriteLine(totalPrice);

            totalPrice = basketItems.Sum(x => Convert.ToDecimal(x.Price, CultureInfo.InvariantCulture));

            
            request.Price = totalPrice.ToString(CultureInfo.InvariantCulture);
            request.PaidPrice = checkoutHeaderDto.OrderTotal.ToString(CultureInfo.InvariantCulture);
            var payment = Payment.Create(request, options);
            Console.WriteLine(request.PaidPrice);
            Console.WriteLine(request.Price);
            Console.WriteLine(payment.ErrorMessage);
            return payment;
        }
    }
}
