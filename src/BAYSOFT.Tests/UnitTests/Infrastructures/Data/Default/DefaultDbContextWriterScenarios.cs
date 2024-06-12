using BAYSOFT.Core.Domain.Default.Aggregates.Samples.Entities;
using BAYSOFT.Tests.Helpers.Data.Default;

namespace BAYSOFT.Tests.UnitTests.Infrastructures.Data.Default
{
	[TestClass]
	public class DefaultDbContextWriterScenarios
	{
		[TestMethod]
		public void DefaultDbContextReader_Add_Should_Not_Throw_Exception()
		{
			using (var context = DefaultDbContextExtensions.GetInMemoryDefaultDbContext().SetupSamples())
			{
				var defaultDbContextReader = context.GetDbContextReader();
				var defaultDbContextWriter = context.GetDbContextWriter();

				var entity = new Sample { Description = "new sample" };

				defaultDbContextWriter.Add(entity);
				context.SaveChanges();

				Assert.IsTrue(defaultDbContextReader.Query<Sample>().Any(x => x.Description == entity.Description));
				Assert.IsTrue(entity.Id != 0);
			}
		}
	}
}
