using BAYSOFT.Abstractions.Core.Application;
using BAYSOFT.Abstractions.Crosscutting.Helpers;
using BAYSOFT.Abstractions.Crosscutting.InheritStringLocalization;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Resources;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using BAYSOFT.Core.Domain.Default.Resources;
using BAYSOFT.Core.Domain.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using ModelWrapper;
using ModelWrapper.Extensions.FullSearch;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Core.Application.Default.Samples.Queries.GetSamplesByFilter
{
	public class GetSamplesByFilterQuery : ApplicationRequest<Sample, GetSamplesByFilterQueryResponse>
	{
		public GetSamplesByFilterQuery()
		{
			ConfigKeys(x => x.Id);

			// ConfigSuppressedProperties(x => x.Id);
			// ConfigSuppressedResponseProperties(x => x.Id);

			//Validator.RuleFor(x => x.prop).NotEmpty().WithMessage("{0} is required!");
		}
	}
	public class GetSamplesByFilterQueryResponse : ApplicationResponse<Sample>
	{
		public GetSamplesByFilterQueryResponse(Tuple<int, int, WrapRequest<Sample>, Dictionary<string, object>, Dictionary<string, object>, string, long?> tuple) : base(tuple)
		{
		}

		public GetSamplesByFilterQueryResponse(WrapRequest<Sample> request, object data, string message = "Successful operation!", long? resultCount = null)
			: base(request, data, message, resultCount)
		{
		}
	}

	[InheritStringLocalizer(typeof(Messages), Priority = 0)]
	[InheritStringLocalizer(typeof(EntitiesDefault), Priority = 1)]
	[InheritStringLocalizer(typeof(EntitiesSamples), Priority = 2)]
	public class GetSamplesByFilterQueryHandler : ApplicationRequestHandler<Sample, GetSamplesByFilterQuery, GetSamplesByFilterQueryResponse>
	{
		private ILoggerFactory Logger { get; set; }
		private IMediator Mediator { get; set; }
		private IStringLocalizer Localizer { get; set; }
		private IDefaultDbContextReader Reader { get; set; }
		public GetSamplesByFilterQueryHandler(
			ILoggerFactory logger,
			IMediator mediator,
			IStringLocalizer<GetSamplesByFilterQueryHandler> localizer,
			IDefaultDbContextReader reader
		)
		{
			Logger = logger;
			Mediator = mediator;
			Localizer = localizer;
			Reader = reader;
		}
		public override async Task<GetSamplesByFilterQueryResponse> Handle(GetSamplesByFilterQuery request, CancellationToken cancellationToken)
		{
			try
			{
				long resultCount = 0;

				var data = await Reader
					.Query<Sample>()
					.AsNoTracking()
					.FullSearch(request, out resultCount)
					.ToListAsync(cancellationToken);

				return new GetSamplesByFilterQueryResponse(request, data, Localizer["Successful operation!"], resultCount);
			}
			catch (Exception exception)
			{
				Logger.CreateLogger<GetSamplesByFilterQueryHandler>().Log(LogLevel.Error, exception, exception.Message);

				return new GetSamplesByFilterQueryResponse(ExceptionResponseHelper.CreateTuple(Localizer, request, exception));
			}
		}
	}
}
