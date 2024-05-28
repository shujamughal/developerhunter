using ApplyForJob.Utils;
using MassTransit;
using SharedContent.Messages;

namespace ApplyForJob.Consumers
{
    public class CompanyJWTTokenApplyForJobConsumer : IConsumer<CompanyJWTokenApplyForJob>
    {
        public async Task Consume(ConsumeContext<CompanyJWTokenApplyForJob> context)
        {
            try
            {
                var message = context.Message;
                CompanyTokenManager.CompanyTokenString = message.CompanyTokenString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error consuming message: {ex.Message}");
                Task.FromException(ex);
            }
        }
    }
}
