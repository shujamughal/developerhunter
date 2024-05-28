using MassTransit;
using SharedContent.Messages;
using Resume.Utils;

namespace jobPosting.Consumers
{
    public class JWTokenResumeConsumer : IConsumer<JWTokenResume>
    {
        public async Task Consume(ConsumeContext<JWTokenResume> context)
        {
            try
            {
                var message = context.Message;
                TokenManager.TokenString = message.TokenString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error consuming message: {ex.Message}");
                // It's better to return a faulted task instead of just creating one and not using it
                await Task.FromException(ex);
            }
        }
    }
}
