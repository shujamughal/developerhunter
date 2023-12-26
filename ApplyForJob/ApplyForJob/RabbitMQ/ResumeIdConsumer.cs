//using System;
//using System.Threading.Tasks;
//using ApplyForJob.Models;
//using MassTransit;

//namespace ApplyForJob.RabbitMQ
//{
//    public class ResumeIdConsumer : IConsumer<ResumeIdMessage>
//    {
//        public async Task Consume(ConsumeContext<ResumeIdMessage> context)
//        {
//            try
//            {
//                var resumeId = context.Message.ResumeId;
//                Console.WriteLine($"Received ResumeId: {resumeId}");

//                Console.WriteLine($"Processing completed for ResumeId: {resumeId}");
//                await context.ConsumeCompleted;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error processing message: {ex.Message}");
//                throw;
//            }
//        }
//    }

//}
