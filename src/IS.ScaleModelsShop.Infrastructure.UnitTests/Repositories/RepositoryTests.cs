using FluentAssertions;
using IS.ScaleModelsShop.Application.Repositories;
using IS.ScaleModelsShop.Domain.Contracts;
using IS.ScaleModelsShop.Infrastructure.Context;
using IS.ScaleModelsShop.Infrastructure.Repositories;
using IS.ScaleModelsShop.Infrastructure.UnitTests.FakeEntities;
using IS.ScaleModelsShop.Infrastructure.UnitTests.Utilities;
using Moq;

namespace IS.ScaleModelsShop.Infrastructure.UnitTests.Repositories
{
    public class RepositoryTests
    {
        private IRepository<FakeEntity> _fakeEntityRepository;
        private Guid _fakeEntityId;
        private Mock<IDateTime> _mockDateTime;
        private FakeEntity _fakeEntity;
        private TestAppDbContext _testDbContext;

        [SetUp]
        public void Setup()
        {
            _fakeEntityId = Guid.NewGuid();
            _fakeEntity = new FakeEntity
            {
                Id = _fakeEntityId,
                Name = "TestName",
                Description = "TestDescription"
            };

            _testDbContext = new TestAppDbContext(Utilities.Utilities.TestDbContextOptions<AppDbContext>());
            _testDbContext.FakeEntities.Add(_fakeEntity);
            _testDbContext.SaveChanges();

            _mockDateTime = new Mock<IDateTime>();
            _mockDateTime.Setup(x => x.UtcNow).Returns(DateTime.Now);

            _fakeEntityRepository = new Repository<FakeEntity>(_testDbContext, _mockDateTime.Object);
        }

        [Test]
        public void Constructor_WhenCalled_InstanceShouldBeCreated()
        {
            _fakeEntityRepository.Should().NotBeNull();
            _fakeEntityRepository.Should().BeOfType<Repository<FakeEntity>>();
        }

        [Test]
        public void Constructor_WhenAppDbContextIsNull_ShouldThrownException()
        {
            Func<IRepository<FakeEntity>> result = () => new Repository<FakeEntity>(null, _mockDateTime.Object);

            result.Should().Throw<ArgumentNullException>();
        }
    }
}