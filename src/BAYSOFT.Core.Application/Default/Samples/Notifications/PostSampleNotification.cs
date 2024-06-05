using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Notifications
{
    public class PostSampleNotification : INotification
	{
		public Sample Payload { get; set; }
		public DateTime CreatedAt { get; set; }
		public PostSampleNotification(Sample payload)
		{
			Payload = payload;
			CreatedAt = DateTime.UtcNow;
		}
	}
	public class PostSampleNotificationHandler : INotificationHandler<PostSampleNotification>
	{
		private ILoggerFactory Logger { get; set; }
		private IMediator Mediator { get; set; }
		public PostSampleNotificationHandler(
			ILoggerFactory logger,
			IMediator mediator)
		{
			Logger = logger;
			Mediator = mediator;
		}
		public Task Handle(PostSampleNotification notification, CancellationToken cancellationToken)
		{
			Logger.CreateLogger<PostSampleNotificationHandler>().Log(LogLevel.Information, $"Sample posted! - Event Created At: {notification.CreatedAt:yyyy-MM-dd HH:mm:ss} Payload: {JsonConvert.SerializeObject(notification.Payload)}");

			//var message = new Message<PostSampleNotification>(notification);

			//Mediator.Send(new RabbitMQServiceRequest("BAYSOFT_EVENTS", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))));

			return Task.CompletedTask;
		}
	}
}
