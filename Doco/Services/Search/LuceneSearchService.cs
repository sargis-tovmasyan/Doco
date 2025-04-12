using Doco.Core.Interfaces;
using Doco.Core.Models;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

using Version = Lucene.Net.Util.Version;

namespace Doco.Services.Search;

public class LuceneSearchService : ISearchService
{
    private readonly string _indexPath;
    private readonly IMetadataService _metadataService;
    private const Version VERSION = Version.LUCENE_30;

    public LuceneSearchService(string indexPath, IMetadataService metadataService)
    {
        _indexPath = indexPath;
        _metadataService = metadataService;
    }

    public void IndexDocument(string documentPath)
    {
        var dir = FSDirectory.Open(new DirectoryInfo(_indexPath));
        var analyzer = new StandardAnalyzer(VERSION);

        using (var writer = new IndexWriter(dir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
        {
            var metadata = _metadataService.GetMetadata(documentPath);
            var doc = new Document();

            doc.Add(new Field("path", documentPath, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("content", File.ReadAllText(documentPath), Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("fileName", metadata.FileName, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("contentType", metadata.ContentType, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("size", metadata.Size.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("createdAt", metadata.CreatedAt.ToString("o"), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("lastModifiedAt", metadata.LastModifiedAt.ToString("o"), Field.Store.YES, Field.Index.NOT_ANALYZED));

            foreach (var tag in metadata.Tags)
                doc.Add(new Field($"tag_{tag.Key}", tag.Value, Field.Store.YES, Field.Index.NOT_ANALYZED));

            writer.UpdateDocument(new Term("path", documentPath), doc);
            writer.Optimize();
            writer.Commit();
        }
    }

    public void RemoveFromIndex(string documentPath)
    {
        var dir = FSDirectory.Open(new DirectoryInfo(_indexPath));
        var analyzer = new StandardAnalyzer(VERSION);

        using (var writer = new IndexWriter(dir, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
        {
            writer.DeleteDocuments(new Term("path", documentPath));
            writer.Commit();
        }
    }

    public IEnumerable<SearchResult> Search(string query, IDictionary<string, string>? filters = null)
    {
        var results = new List<SearchResult>();
        var dir = FSDirectory.Open(new DirectoryInfo(_indexPath));
        var analyzer = new StandardAnalyzer(VERSION);

        using (var reader = IndexReader.Open(dir, true))
        {
            var searcher = new IndexSearcher(reader);
            var parser = new QueryParser(VERSION, "content", analyzer);
            var mainQuery = parser.Parse(query);

            Query finalQuery = mainQuery;

            if (filters != null)
            {
                var booleanQuery = new BooleanQuery { { mainQuery, Occur.MUST } };

                foreach (var filter in filters)
                {
                    var filterQuery = new TermQuery(new Term($"tag_{filter.Key}", filter.Value));
                    booleanQuery.Add(filterQuery, Occur.MUST);
                }

                finalQuery = booleanQuery;
            }

            var hits = searcher.Search(finalQuery, 10);

            foreach (var hit in hits.ScoreDocs)
            {
                var doc = searcher.Doc(hit.Doc);
                var path = doc.Get("path");
                var metadata = _metadataService.GetMetadata(path);

                results.Add(new SearchResult
                {
                    DocumentPath = path,
                    Score = hit.Score,
                    Metadata = metadata
                });
            }
        }

        return results;
    }
}