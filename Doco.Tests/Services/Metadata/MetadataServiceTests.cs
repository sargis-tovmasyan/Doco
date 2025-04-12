using Doco.Core.Interfaces;
using Doco.Services.Metadata;
using Moq;

namespace Doco.Tests.Services.Metadata
{
    public class MetadataServiceTests
    {
        private readonly Mock<IMetaDatabase> _mockDatabase;
        private readonly MetadataService _metadataService;

        public MetadataServiceTests()
        {
            _mockDatabase = new Mock<IMetaDatabase>();
            _metadataService = new MetadataService(_mockDatabase.Object);
        }

        [Fact]
        public void GetMetadataAsync_ShouldReturnMetadata_FromDatabase()
        {
            // Arrange
            var documentPath = "test-document";
            var expectedMetadata = new Dictionary<string, string> { { "Author", "John Doe" } };
            _mockDatabase.Setup(db => db.GetMetadata(documentPath)).Returns(expectedMetadata);

            // Act
            var result = _metadataService.GetMetadata(documentPath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMetadata, result);
            _mockDatabase.Verify(db => db.GetMetadata(documentPath), Times.Once);
        }

        [Fact]
        public void SetMetadataAsync_ShouldCallDatabaseSetMetadata()
        {
            // Arrange
            var documentPath = "test-document";
            var metadata = new Dictionary<string, string> { { "Author", "John Doe" } };

            // Act
            _metadataService.SetMetadata(documentPath, metadata);

            // Assert
            _mockDatabase.Verify(db => db.SetMetadata(documentPath, metadata), Times.Once);
        }

        [Fact]
        public void RemoveMetadataAsync_ShouldCallDatabaseRemoveMetadata()
        {
            // Arrange
            var documentPath = "test-document";
            var keysToRemove = new[] { "Author" };

            // Act
            _metadataService.RemoveMetadata(documentPath, keysToRemove);

            // Assert
            _mockDatabase.Verify(db => db.RemoveMetadata(documentPath, keysToRemove), Times.Once);
        }
    }
}
