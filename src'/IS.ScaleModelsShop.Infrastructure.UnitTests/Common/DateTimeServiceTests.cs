using FluentAssertions;
using IS.ScaleModelsShop.Infrastructure.Common;

namespace IS.ScaleModelsShop.Infrastructure.UnitTests.Common
{
    [TestFixture]
    public class DateTimeServiceTests
    {
        private DateTimeService dateTimeService;

        [SetUp]
        public void Setup()
        {
            this.dateTimeService = new DateTimeService();
        }

        [Test]
        public void Constructor_WhenInstanceCreated_ShouldCreated()
        {
            this.dateTimeService.Should().NotBeNull();
        }
    }
}