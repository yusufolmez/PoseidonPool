namespace PoseidonPool.Infrastructure.Options
{
    public class IyzipayOptions
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string BaseUrl { get; set; }
        public string CallbackUrl { get; set; }
        public string Currency { get; set; } = "TRY";
        public string ConversationIdPrefix { get; set; } = "PP-";
    }
}


