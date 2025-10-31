using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PoseidonPool.Application.Abstractions.Payment.Iyzico;
using PoseidonPool.Application.DTOs.Payment;
using PoseidonPool.Infrastructure.Options;
using Iyzipay.Model;
using Iyzipay.Request;
using IyzipaySdkOptions = Iyzipay.Options;

namespace PoseidonPool.Infrastructure.Services.Payment.Iyzico
{
    public class IyzicoService : IIyzicoService
    {
        private readonly IyzipayOptions _options;
        private readonly IyzipaySdkOptions _iyzipayOptions;

        public IyzicoService(IOptions<IyzipayOptions> options)
        {
            _options = options?.Value ?? new IyzipayOptions();
            
            var apiKey = (_options.ApiKey ?? string.Empty).Trim();
            var secretKey = (_options.SecretKey ?? string.Empty).Trim();
            var baseUrl = (_options.BaseUrl ?? "https://sandbox-api.iyzipay.com").Trim();
            
            _iyzipayOptions = new IyzipaySdkOptions
            {
                ApiKey = apiKey,
                SecretKey = secretKey,
                BaseUrl = baseUrl
            };
            
            if (string.IsNullOrWhiteSpace(_iyzipayOptions.ApiKey) || 
                string.IsNullOrWhiteSpace(_iyzipayOptions.SecretKey))
            {
                throw new InvalidOperationException(
                    "Iyzico API Key veya Secret Key yapılandırılmamış. " +
                    "Lütfen appsettings.json'da Payments:Iyzico bölümünü kontrol edin.");
            }
        }

        public async Task<CreatePaymentResponseDTO> CreateAsync(CreatePaymentRequestDTO request)
        {
            try
            {
                var convId = string.IsNullOrWhiteSpace(request.ConversationId)
                    ? $"{_options.ConversationIdPrefix}{Guid.NewGuid()}"
                    : request.ConversationId;

                var baseCallbackUrl = !string.IsNullOrWhiteSpace(request.CallbackUrl)
                    ? request.CallbackUrl
                    : (!string.IsNullOrWhiteSpace(_options.CallbackUrl)
                        ? _options.CallbackUrl
                        : "https://localhost:7261/api/payment/callback");
                
                var callbackUrl = baseCallbackUrl.Contains("?") 
                    ? $"{baseCallbackUrl}&conversationId={Uri.EscapeDataString(convId)}"
                    : $"{baseCallbackUrl}?conversationId={Uri.EscapeDataString(convId)}";
                var buyerName = !string.IsNullOrWhiteSpace(request.Buyer?.Name) ? request.Buyer.Name : "Test";
                var buyerSurname = !string.IsNullOrWhiteSpace(request.Buyer?.Surname) ? request.Buyer.Surname : "User";
                var buyerEmail = !string.IsNullOrWhiteSpace(request.Buyer?.Email) ? request.Buyer.Email : "test@example.com";
                var buyerGsm = !string.IsNullOrWhiteSpace(request.Buyer?.GsmNumber) ? request.Buyer.GsmNumber : "5550000000";
                var buyerCity = !string.IsNullOrWhiteSpace(request.Buyer?.City) ? request.Buyer.City : "Istanbul";
                var buyerCountry = !string.IsNullOrWhiteSpace(request.Buyer?.Country) ? request.Buyer.Country : "Turkey";
                var buyerAddress = !string.IsNullOrWhiteSpace(request.Buyer?.Address) ? request.Buyer.Address : "Test Address";
                var buyerZipCode = !string.IsNullOrWhiteSpace(request.Buyer?.ZipCode) ? request.Buyer.ZipCode : "34000";

                var buyer = new Iyzipay.Model.Buyer
                {
                    Id = !string.IsNullOrWhiteSpace(request.Buyer?.Id) ? request.Buyer.Id : Guid.NewGuid().ToString(),
                    Name = buyerName,
                    Surname = buyerSurname,
                    GsmNumber = buyerGsm,
                    Email = buyerEmail,
                    IdentityNumber = !string.IsNullOrWhiteSpace(request.Buyer?.IdentityNumber) ? request.Buyer.IdentityNumber : "11111111111",
                    LastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    RegistrationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    RegistrationAddress = buyerAddress,
                    Ip = !string.IsNullOrWhiteSpace(request.Buyer?.Ip) ? request.Buyer.Ip : "127.0.0.1",
                    City = buyerCity,
                    Country = buyerCountry,
                    ZipCode = buyerZipCode
                };

                var address = new Iyzipay.Model.Address
                {
                    ContactName = $"{buyerName} {buyerSurname}",
                    City = buyerCity,
                    Country = buyerCountry,
                    Description = buyerAddress,
                    ZipCode = buyerZipCode
                };

                var basketItems = new List<BasketItem>();
                if (request.Items == null || !request.Items.Any())
                {
                    basketItems.Add(new BasketItem
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Product",
                        Category1 = "General",
                        Category2 = "General",
                        ItemType = BasketItemType.PHYSICAL.ToString(),
                        Price = request.Price.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                    });
                }
                else
                {
                    foreach (var item in request.Items)
                    {
                        var itemId = !string.IsNullOrWhiteSpace(item.Id) ? item.Id : Guid.NewGuid().ToString();
                        var itemName = !string.IsNullOrWhiteSpace(item.Name) ? item.Name : "Product";
                        var itemCategory1 = !string.IsNullOrWhiteSpace(item.Category1) ? item.Category1 : "General";
                        var itemCategory2 = !string.IsNullOrWhiteSpace(item.Category2) ? item.Category2 : "General";

                        basketItems.Add(new BasketItem
                        {
                            Id = itemId,
                            Name = itemName,
                            Category1 = itemCategory1,
                            Category2 = itemCategory2,
                            ItemType = BasketItemType.PHYSICAL.ToString(),
                            Price = item.Price.ToString("F2", System.Globalization.CultureInfo.InvariantCulture)
                        });
                    }
                }

                if (request.Price <= 0)
                {
                    throw new ArgumentException("Price must be greater than 0", nameof(request.Price));
                }

                if (request.PaidPrice <= 0)
                {
                    throw new ArgumentException("PaidPrice must be greater than 0", nameof(request.PaidPrice));
                }

                var basketTotal = basketItems.Sum(bi => decimal.Parse(bi.Price, System.Globalization.CultureInfo.InvariantCulture));
                if (Math.Abs(basketTotal - request.Price) > 0.01m)
                {
                    var difference = request.Price - basketTotal;
                    if (basketItems.Any())
                    {
                        var lastItem = basketItems.Last();
                        var lastItemPrice = decimal.Parse(lastItem.Price, System.Globalization.CultureInfo.InvariantCulture);
                        lastItem.Price = (lastItemPrice + difference).ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }

                var checkoutFormInitializeRequest = new CreateCheckoutFormInitializeRequest
                {
                    Locale = Locale.TR.ToString(),
                    ConversationId = convId,
                    Price = request.Price.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                    PaidPrice = request.PaidPrice.ToString("F2", System.Globalization.CultureInfo.InvariantCulture),
                    Currency = !string.IsNullOrWhiteSpace(request.Currency) ? request.Currency : Currency.TRY.ToString(),
                    BasketId = Guid.NewGuid().ToString(),
                    PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                    CallbackUrl = callbackUrl,
                    Buyer = buyer,
                    ShippingAddress = address,
                    BillingAddress = address,
                    BasketItems = basketItems
                };

                var checkoutFormInitialize = await Task.Run(() => 
                    CheckoutFormInitialize.Create(checkoutFormInitializeRequest, _iyzipayOptions));

                var response = new CreatePaymentResponseDTO
                {
                    Status = checkoutFormInitialize.Status == "success" ? "success" : "failure",
                    RedirectUrl = checkoutFormInitialize.PaymentPageUrl,
                    CheckoutFormContent = checkoutFormInitialize.CheckoutFormContent,
                    ConversationId = checkoutFormInitialize.ConversationId,
                    ErrorMessage = checkoutFormInitialize.ErrorMessage
                };

                if (checkoutFormInitialize.Status != "success")
                {
                    var errorDetails = $"Status: {checkoutFormInitialize.Status}, " +
                                     $"Error Code: {checkoutFormInitialize.ErrorCode}, " +
                                     $"Error Message: {checkoutFormInitialize.ErrorMessage}, " +
                                     $"Error Group: {checkoutFormInitialize.ErrorGroup}";
                    
                    response.ErrorMessage = string.IsNullOrWhiteSpace(response.ErrorMessage) 
                        ? errorDetails 
                        : $"{response.ErrorMessage} | {errorDetails}";
                }

                if (string.IsNullOrWhiteSpace(_iyzipayOptions.ApiKey) || string.IsNullOrWhiteSpace(_iyzipayOptions.SecretKey))
                {
                    response.ErrorMessage = $"API Key veya Secret Key yapılandırılmamış. ApiKey boş: {string.IsNullOrWhiteSpace(_iyzipayOptions.ApiKey)}, SecretKey boş: {string.IsNullOrWhiteSpace(_iyzipayOptions.SecretKey)}, BaseUrl: {_iyzipayOptions.BaseUrl}";
                    response.Status = "failure";
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                return new CreatePaymentResponseDTO
                {
                    Status = "failure",
                    RedirectUrl = null,
                    CheckoutFormContent = null,
                    ConversationId = string.IsNullOrWhiteSpace(request.ConversationId)
                        ? $"{_options.ConversationIdPrefix}{Guid.NewGuid()}"
                        : request.ConversationId,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<CallbackResponseDTO> HandleCallbackAsync(CallbackRequestDTO request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request?.Token))
                {
                    return new CallbackResponseDTO
                    {
                        Status = "failure",
                        PaymentId = null,
                        ErrorMessage = "Token is required"
                    };
                }

                var retrieveRequest = new RetrieveCheckoutFormRequest
                {
                    Token = request.Token
                };

                var checkoutForm = await Task.Run(() => 
                    CheckoutForm.Retrieve(retrieveRequest, _iyzipayOptions));

                var conversationIdFromIyzico = checkoutForm?.ConversationId;
                var finalConversationId = !string.IsNullOrWhiteSpace(conversationIdFromIyzico) 
                    ? conversationIdFromIyzico 
                    : request.ConversationId;

                if (checkoutForm?.Status == "success" && checkoutForm?.PaymentStatus == "SUCCESS")
                {
                    return new CallbackResponseDTO
                    {
                        Status = "success",
                        PaymentId = checkoutForm.PaymentId,
                        ConversationId = finalConversationId,
                        ErrorMessage = null
                    };
                }
                else
                {
                    return new CallbackResponseDTO
                    {
                        Status = "failure",
                        PaymentId = checkoutForm?.PaymentId,
                        ConversationId = checkoutForm?.ConversationId,
                        ErrorMessage = checkoutForm?.ErrorMessage ?? "Payment failed"
                    };
                }
            }
            catch (Exception ex)
            {
                return new CallbackResponseDTO
                {
                    Status = "failure",
                    PaymentId = null,
                    ErrorMessage = ex.Message
                };
            }
        }

        public Task<RefundResponseDTO> RefundAsync(RefundRequestDTO request)
        {
            return Task.FromResult(new RefundResponseDTO { Status = "success" });
        }

        public Task<CancelResponseDTO> CancelAsync(CancelRequestDTO request)
        {
            return Task.FromResult(new CancelResponseDTO { Status = "success" });
        }

        public Task<IyziInstallmentResponseDTO> RetrieveInstallmentsAsync(string binNumber, decimal price, string currency)
        {
            var resp = new IyziInstallmentResponseDTO
            {
                BinNumber = binNumber,
                Price = price,
                Currency = currency,
                Options = new System.Collections.Generic.List<IyziInstallmentOptionDTO>
                {
                    new IyziInstallmentOptionDTO { InstallmentNumber = 1, TotalPrice = price, InstallmentPrice = price },
                    new IyziInstallmentOptionDTO { InstallmentNumber = 2, TotalPrice = price, InstallmentPrice = Math.Round(price/2,2) }
                }
            };
            return Task.FromResult(resp);
        }

        public Task<IyziPaymentDetailResponseDTO> RetrievePaymentDetailAsync(string paymentId)
        {
            return Task.FromResult(new IyziPaymentDetailResponseDTO { PaymentId = paymentId, Status = "success" });
        }

        public Task<bool> VerifyCallbackAsync(CallbackRequestDTO request)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(request?.Token));
        }
    }
}


