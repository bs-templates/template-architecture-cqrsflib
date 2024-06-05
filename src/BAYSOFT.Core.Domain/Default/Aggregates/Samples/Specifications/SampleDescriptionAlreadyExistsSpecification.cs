using BAYSOFT.Abstractions.Core.Domain.Entities.Specifications;
using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Entities;
using BAYSOFT.Core.Domain.Default.Interfaces.Infrastructures.Data;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BAYSOFT.Core.Domain.Default.Aggregates.Samples.Specifications
{
	public class SampleDescriptionAlreadyExistsSpecification : DomainSpecification<Sample>
    {
        private IDefaultDbContextReader Reader { get; set; }
        public SampleDescriptionAlreadyExistsSpecification(IDefaultDbContextReader reader)
        {
            Reader = reader;
        }

        public override Expression<Func<Sample, bool>> ToExpression()
		{
			return sample => CheckRule(sample);
		}

		private bool CheckRule(Sample sample)
		{
			return Reader.Query<Sample>().Any(x => x.Description == sample.Description && x.Id != sample.Id);
		}
	}
}
