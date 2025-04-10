using Doco.Core;
using Doco.Core.Models;

namespace Doco.Tests.Services.Metadata
{
    public class DocumentMetadataTests
    {
        [Fact]
        public void DefaultValues_ShouldBeSetCorrectly()
        {
            // Arrange & Act
            var metadata = new DocumentMetadata();

            // Assert
            Assert.Equal(string.Empty, metadata.FileName);
            Assert.Equal(DocumentType.Unknown, metadata.Type);
            Assert.Equal("application/octet-stream", metadata.ContentType);
            Assert.Equal(0, metadata.Size);
            Assert.Equal(default, metadata.CreatedAt);
            Assert.Equal(default, metadata.LastModifiedAt);
        }

        [Fact]
        public void Properties_ShouldBeSettableAndRetrievable()
        {
            // Arrange
            var metadata = new DocumentMetadata();
            var createdAt = DateTime.UtcNow;
            var lastModifiedAt = DateTime.UtcNow.AddHours(1);

            // Act
            metadata.FileName = "example.pdf";
            metadata.Type = DocumentType.Pdf;
            metadata.ContentType = "application/pdf";
            metadata.Size = 1024;
            metadata.CreatedAt = createdAt;
            metadata.LastModifiedAt = lastModifiedAt;

            // Assert
            Assert.Equal("example.pdf", metadata.FileName);
            Assert.Equal(DocumentType.Pdf, metadata.Type);
            Assert.Equal("application/pdf", metadata.ContentType);
            Assert.Equal(1024, metadata.Size);
            Assert.Equal(createdAt, metadata.CreatedAt);
            Assert.Equal(lastModifiedAt, metadata.LastModifiedAt);
        }
    }
}