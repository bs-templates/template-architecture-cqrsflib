using BAYSOFT.Core.Domain.Entities.Default;
using BAYSOFT.Core.Domain.Interfaces.Infrastructures.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using BAYSOFT.Core.Domain.Services.Tests;

namespace BAYSOFT.Core.Domain.Services.Tests.Default
{
    public static class MockDefaultHelper
    {
        internal static Mock<IDefaultDbContext> GetMockedDefaultDbContext()
        {
            var mockedDeafultDbContext = new Mock<IDefaultDbContext>();

            return mockedDeafultDbContext;
        }
        internal static Mock<IDefaultDbContextQuery> GetMockedDefaultDbContextQuery()
        {
            var mockedDeafultDbContextQuery = new Mock<IDefaultDbContextQuery>();

            return mockedDeafultDbContextQuery;
        }
    }
}
