using E_commerce_Website.DTOs;
using E_commerce_Website.Entites;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce_Website.Services
{
    public class PaymentServices
    {
        private readonly IConfiguration _configration;

        public PaymentServices(IConfiguration configration)
        {
            _configration = configration;
        }
        //public async Task<PaymentIntent> CreateOrUpdatePaymentIntent(Basket basket)
        //{
        //    StripeConfiguration.ApiKey = _configration["StripeSettings:SecretKey"];
        //    var service = new PaymentIntentService();
        //    var intent = new PaymentIntent();
        //    var subtotal = basket.Item.Sum(item => item.Quantity * item.Product.Price);
        //    var deliveryFee = subtotal > 10000 ? 0 : 500;
        //    if (string.IsNullOrEmpty(basket.PaymentIntentId))
        //    {
        //        var options = new PaymentIntentCreateOptions
        //        {
        //            Amount = subtotal + deliveryFee,
        //            Currency = "usd",
        //            PaymentMethodTypes = new List<string> { "card" },
        //        };
        //        intent = await service.CreateAsync(options);

        //    }
        //    else
        //    {
        //        var options = new PaymentIntentUpdateOptions
        //        {
        //            Amount = subtotal + deliveryFee
        //        };
        //        await service.UpdateAsync(basket.PaymentIntentId, options);
        //    }
        //    return intent;
        //}

        public async Task<PaymentIntent> CreateOrUpdatePaymentIntent(Basket basket /*, PaymentAddress address*/)
        {
            StripeConfiguration.ApiKey = _configration["StripeSettings:SecretKey"];

            var service = new PaymentIntentService();

            var intent = new PaymentIntent();

            var subtotal = basket.Item.Sum(item => item.Quantity * item.Product.Price);
            var deliveryFee = subtotal > 10000 ? 0 : 500;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = subtotal + deliveryFee,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" },
                    Description = "e-commerse",
                    Shipping = new ChargeShippingOptions
                    {
                        Name = "Jenny Rosen",
                        Address = new AddressOptions
                        {
                            Line1 = "510 Townsend St",
                            PostalCode = "98140",
                            City = "San Francisco",
                            State = "CA",
                            Country = "US",
                        },
                    },
                };
                intent = await service.CreateAsync(options);
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = subtotal + deliveryFee
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            return intent;
        }
    }
}
