using MassTransit;
using SharedContent.Messages;
using Resume.Utils;

namespace Resume.Consumers
{
    public class ResumeIdConsumer : IConsumer<ResumeId>
    {
        public async Task Consume(ConsumeContext<ResumeId> context)
        {
            try
            {
                var message = context.Message;
                ResumeIdManager.ResumeId = message.UserResumeId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error consuming message: {ex.Message}");
                Task.FromException(ex);
            }
        }
    }
}
