using MassTransit;
using SharedContent.Messages;
using Resume.Utils;

namespace Resume.Consumers
{
    public class CompanyJWTTokenResumeConsumer : IConsumer<CompanyJWTokenResume>
    {
        public async Task Consume(ConsumeContext<CompanyJWTokenResume> context)
        {
            try
            {
                Console.WriteLine("Company token consumed in resume service");
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
