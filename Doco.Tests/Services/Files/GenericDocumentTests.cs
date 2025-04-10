using Doco.Core;
using Doco.Core.Models;
using Doco.Services.Files;

namespace Doco.Tests.Services.Files.Pdf
{
    public class GenericDocumentTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            var metadata = new DocumentMetadata
            {
                FileName = "example.pdf",
                Type = DocumentType.Pdf,
                ContentType = "application/pdf",
                Size = 1024,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };
            var filePath = "test.pdf";

            // Act
            var document = new GenericDocument(metadata, filePath);

            // Assert
            Assert.Equal(metadata, document.Metadata);
        }

        [Fact]
        public void GetContentStream_ShouldReturnFileStream()
        {
            // Arrange
            var metadata = new DocumentMetadata
            {
                FileName = "example.pdf",
                Type = DocumentType.Pdf,
                ContentType = "application/pdf",
                Size = 1024,
                CreatedAt = DateTime.UtcNow,
                LastModifiedAt = DateTime.UtcNow
            };
            var filePath = "test.pdf";

            // Create a temporary file for testing
            File.WriteAllText(filePath, "Test content");

            try
            {
                var document = new GenericDocument(metadata, filePath);

                // Act
                using var stream = document.GetContentStream();

                // Assert
                Assert.NotNull(stream);
                Assert.True(stream.CanRead);
            }
            finally
            {
                // Clean up the temporary file
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
