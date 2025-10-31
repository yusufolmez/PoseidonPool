using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PoseidonPool.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PoseidonPool.Application.Features.Commands.Payment.Create.CreatePaymentCommandRequest request)
        {
            var res = await _mediator.Send(request);
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("callback")]
        [HttpGet("callback")]
        public async Task<IActionResult> Callback()
        {
            string token = null;
            string conversationId = null;
            
            if (Request.Query.ContainsKey("conversationId"))
            {
                conversationId = Request.Query["conversationId"].ToString();
            }
            
            if (Request.Query.ContainsKey("token"))
            {
                token = Request.Query["token"].ToString();
            }
            else if (Request.ContentType?.Contains("application/json") == true)
            {
                Request.EnableBuffering();
                Request.Body.Position = 0;
                
                using var reader = new System.IO.StreamReader(Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                Request.Body.Position = 0;
                
                if (!string.IsNullOrWhiteSpace(body))
                {
                    try
                    {
                        var jsonBody = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(body);
                        if (jsonBody.TryGetProperty("token", out var tokenProp))
                            token = tokenProp.GetString();
                        if (jsonBody.TryGetProperty("conversationId", out var convProp))
                            conversationId = convProp.GetString();
                    }
                    catch { }
                }
            }
            else if (Request.HasFormContentType)
            {
                if (Request.Form.ContainsKey("token"))
                    token = Request.Form["token"].ToString();
                if (Request.Form.ContainsKey("conversationId"))
                    conversationId = Request.Form["conversationId"].ToString();
            }
            
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(new 
                { 
                    error = "Token is required",
                    message = "Token must be provided as query parameter (?token=xxx), in JSON body, or in form data.",
                    receivedQuery = Request.QueryString.ToString(),
                    receivedMethod = Request.Method
                });
            }
            
            var request = new PoseidonPool.Application.Features.Commands.Payment.Callback.PaymentCallbackCommandRequest
            {
                Token = token,
                ConversationId = string.IsNullOrWhiteSpace(conversationId) ? null : conversationId
            };
            var res = await _mediator.Send(request);
            return Ok(res);
        }
    }
}


