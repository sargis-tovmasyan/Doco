using Doco.Core;
using Doco.Core.Exceptions;
using Doco.Core.Models;
using Doco.Services.Files;
using Doco.Services.Files.Pdf;

namespace Doco.Tests.Services.Files.Pdf
{
    public class PdfDocumentServiceTests
    {
        [Fact]
        public void LoadDocument_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            // Arrange
            var service = new PdfDocumentService();
            var nonExistentFilePath = "nonexistent.pdf";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => service.LoadDocument(nonExistentFilePath));
        }

        [Fact]
        public void LoadDocument_ShouldThrowUnsupportedDocumentTypeException_WhenFileIsNotPdf()
        {
            // Arrange
            var service = new PdfDocumentService();
            var invalidFilePath = "document.txt";

            // Create a temporary file for testing
            File.WriteAllText(invalidFilePath, "Test content");

            try
            {
                // Act & Assert
                var exception = Assert.Throws<UnsupportedDocumentTypeException>(() => service.LoadDocument(invalidFilePath));
                Assert.Equal("Only PDF is supported.", exception.Message);
            }
            finally
            {
                // Clean up the temporary file
                if (File.Exists(invalidFilePath))
                {
                    File.Delete(invalidFilePath);
                }
            }
        }

        [Fact]
        public void LoadDocument_ShouldReturnGenericDocument_WhenFileIsValidPdf()
        {
            // Arrange
            var service = new PdfDocumentService();
            var validFilePath = "valid.pdf";

            // Create a temporary PDF file for testing
            File.WriteAllText(validFilePath, "Test content");

            try
            {
                // Act
                var document = service.LoadDocument(validFilePath);

                // Assert
                Assert.NotNull(document);
                Assert.Equal("valid.pdf", document.Metadata.FileName);
                Assert.Equal(DocumentType.Pdf, document.Metadata.Type);
                Assert.Equal(new FileInfo(validFilePath).Length, document.Metadata.Size);
            }
            finally
            {
                // Clean up the temporary file
                if (File.Exists(validFilePath))
                {
                    File.Delete(validFilePath);
                }
            }
        }

        [Fact]
        public void SaveDocument_ShouldCopyContentToDestinationPath()
        {
            // Arrange
            var service = new PdfDocumentService();
            var sourceFilePath = "source.pdf";
            var destinationFilePath = "destination.pdf";

            // Create a temporary source file for testing
            File.WriteAllText(sourceFilePath, "Test content");

            var metadata = new DocumentMetadata
            {
                FileName = "source.pdf",
                Type = DocumentType.Pdf,
                Size = new FileInfo(sourceFilePath).Length,
                CreatedAt = DateTime.UtcNow
            };
            var document = new GenericDocument(metadata, sourceFilePath);

            try
            {
                // Act
                service.SaveDocument(document, destinationFilePath);

                // Assert
                Assert.True(File.Exists(destinationFilePath));
                Assert.Equal(File.ReadAllText(sourceFilePath), File.ReadAllText(destinationFilePath));
            }
            finally
            {
                // Clean up the temporary files
                if (File.Exists(sourceFilePath))
                {
                    File.Delete(sourceFilePath);
                }
                if (File.Exists(destinationFilePath))
                {
                    File.Delete(destinationFilePath);
                }
            }
        }

        [Fact]
        public void DeleteDocument_ShouldDeleteFile_WhenFileExists()
        {
            // Arrange
            var service = new PdfDocumentService();
            var filePath = "fileToDelete.pdf";

            // Create a temporary file for testing
            File.WriteAllText(filePath, "Test content");

            // Act
            service.DeleteDocument(filePath);

            // Assert
            Assert.False(File.Exists(filePath));
        }

        [Fact]
        public void DeleteDocument_ShouldDoNothing_WhenFileDoesNotExist()
        {
            // Arrange
            var service = new PdfDocumentService();
            var nonExistentFilePath = "nonexistent.pdf";

            // Act
            service.DeleteDocument(nonExistentFilePath);

            // Assert
            Assert.False(File.Exists(nonExistentFilePath)); // Ensure no exception is thrown
        }
    }
}
