using Doco.Services.Metadata;

namespace Doco.Tests.Services.Metadata
{
    public class MetaDatabaseTests
    {
        [Fact]
        public void GetMetadata_ShouldReturnEmptyDictionary_WhenNoMetadataExists()
        {
            // Arrange
            using var db = new MetaDatabase(":memory:");

            // Act
            var metadata = db.GetMetadata("nonexistent-path");

            // Assert
            Assert.NotNull(metadata);
            Assert.Empty(metadata);
        }

        [Fact]
        public void SetMetadata_ShouldStoreAndRetrieveMetadata()
        {
            // Arrange
            using var db = new MetaDatabase(":memory:");
            var documentPath = "test-document";
            var metadata = new Dictionary<string, string> { { "Author", "John Doe" } };

            // Act
            db.SetMetadata(documentPath, metadata);
            var retrievedMetadata = db.GetMetadata(documentPath);

            // Assert
            Assert.NotNull(retrievedMetadata);
            Assert.Single(retrievedMetadata);
            Assert.Equal("John Doe", retrievedMetadata["Author"]);
        }

        [Fact]
        public void SetMetadata_ShouldUpdateExistingMetadata()
        {
            // Arrange
            using var db = new MetaDatabase(":memory:");
            var documentPath = "test-document";
            var initialMetadata = new Dictionary<string, string> { { "Author", "John Doe" } };
            var updatedMetadata = new Dictionary<string, string> { { "Author", "Jane Doe" }, { "Title", "Test Title" } };

            // Act
            db.SetMetadata(documentPath, initialMetadata);
            db.SetMetadata(documentPath, updatedMetadata);
            var retrievedMetadata = db.GetMetadata(documentPath);

            // Assert
            Assert.NotNull(retrievedMetadata);
            Assert.Equal(2, retrievedMetadata.Count);
            Assert.Equal("Jane Doe", retrievedMetadata["Author"]);
            Assert.Equal("Test Title", retrievedMetadata["Title"]);
        }

        [Fact]
        public void RemoveMetadata_ShouldDeleteSpecifiedKeys()
        {
            // Arrange
            using var db = new MetaDatabase(":memory:");
            var documentPath = "test-document";
            var metadata = new Dictionary<string, string>
            {
                { "Author", "John Doe" },
                { "Title", "Test Document" }
            };
            db.SetMetadata(documentPath, metadata);

            // Act
            db.RemoveMetadata(documentPath, TODO);
            var retrievedMetadata = db.GetMetadata(documentPath);

            // Assert
            Assert.Single(retrievedMetadata);
            Assert.False(retrievedMetadata.ContainsKey("Author"));
            Assert.True(retrievedMetadata.ContainsKey("Title"));
        }

        [Fact]
        public void RemoveMetadata_ShouldDoNothing_WhenKeyDoesNotExist()
        {
            // Arrange
            using var db = new MetaDatabase(":memory:");
            var documentPath = "test-document";
            var metadata = new Dictionary<string, string> { { "Author", "John Doe" } };
            db.SetMetadata(documentPath, metadata);

            // Act
            db.RemoveMetadata(documentPath, TODO);
            var retrievedMetadata = db.GetMetadata(documentPath);

            // Assert
            Assert.Single(retrievedMetadata);
            Assert.True(retrievedMetadata.ContainsKey("Author"));
        }
    }
}
