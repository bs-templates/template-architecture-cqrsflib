using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Core.Domain.Exceptions;
using BAYSOFT.Abstractions.Crosscutting.Helpers;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Application.Default.Samples.Notifications;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Resources;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Services;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Interfaces.Services.Default.Samples;
using BAYSOFT.Core.Domain.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModelWrapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Commands.DeleteSample
{
	public class DeleteSampleCommand : ApplicationRequest<Sample, DeleteSampleCommandResponse>
    {
        public DeleteSampleCommand()
        {
            ConfigKeys(x => x.Id);

            // Configures supressed properties & response properties
            //ConfigSuppressedProperties(x => x);
            //ConfigSuppressedResponseProperties(x => x);
        }
	}
	public class DeleteSampleCommandResponse : ApplicationResponse<Sample>
	{
		public DeleteSampleCommandResponse()
		{
		}

		public DeleteSampleCommandResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null) : base(request, data, message, resultCount)
		{
		}

		public DeleteSampleCommandResponse(Tuple<int, int, WrapRequest<Sample>, Dictionary<string, object>, Dictionary<string, object>, string, long?> tuple) : base(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6)
		{
		}
	}

	[InheritStringLocalizer(typeof(Messages), Priority = 0)]
	[InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
	[InheritStringLocalizer(typeof(EntitiesSamples), Priority = 2)]
	public class DeleteSampleCommandHandler : ApplicationRequestHandler<Sample, DeleteSampleCommand, DeleteSampleCommandResponse>
	{
		private ILoggerFactory Logger { get; set; }
		private IMediator Mediator { get; set; }
		private IStringLocalizer Localizer { get; set; }
		private IDefaultDbContextWriter Writer { get; set; }
		public DeleteSampleCommandHandler(
			ILoggerFactory logger,
			IMediator mediator,
			IStringLocalizer<DeleteSampleCommandHandler> localizer,
			IDefaultDbContextWriter writer
		)
		{
			Logger = logger;
			Mediator = mediator;
			Localizer = localizer;
			Writer = writer;
		}
		public override async Task<DeleteSampleCommandResponse> Handle(DeleteSampleCommand request, CancellationToken cancellationToken)
		{
			try
			{
				request.IsValid(Localizer, true);

				var id = request.Project(x => x.Id);

				var data = await Writer
					.Query<Sample>()
					.SingleOrDefaultAsync(x => x.Id == id);

				if (data == null)
				{
					throw new EntityNotFoundException<Sample>(Localizer);
				}

				await Mediator.Send(new DeleteSampleServiceRequest(data));

				await Writer.CommitAsync(cancellationToken);

				await Mediator.Publish(new DeleteSampleNotification(data));

				return new DeleteSampleCommandResponse(request, data, Localizer["Successful operation!"], 1);
			}
			catch (Exception exception)
			{
				Logger.CreateLogger<DeleteSampleCommandHandler>().Log(LogLevel.Error, exception, exception.Message);

				return new DeleteSampleCommandResponse(ExceptionResponseHelper.CreateTuple(Localizer, request, exception));
			}
		}
	}
}
