using MassTransit;
using SharedContent.Messages;
using jobPosting.Utils;

namespace jobPosting.Consumers
{
    public class CompanyJWTTokenJobPostingConsumer : IConsumer<CompanyJWTokenJobPosting>
    {
        public async Task Consume(ConsumeContext<CompanyJWTokenJobPosting> context)
        {
            try
            {
                var message = context.Message;
                CompanyTokenManager.CompanyTokenString = message.CompanyTokenString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error consuming message: {ex.Message}");
                await Task.FromException(ex);
            }
        }
    }
}
