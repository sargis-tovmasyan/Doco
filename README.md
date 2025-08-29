Design Goals

📦 Document Type	Start with PDF, support others later<br>
🗃 Storage	Local-only first, cloud-pluggable later<br>
⚙️ Platform	.NET 9 (latest & greatest)<br>

## 🧠 Features:

✔️ Versioning<br>
✔️ Metadata tagging<br>
✔️ Full-text search<br>
✔️ Encryption<br>

<pre>
USER/API
   |
   v
ISearchService.Search("invoice", filters)
   |
   +--> IndexStore.Search("invoice") --> [DocA, DocB]
   |
   +--> MetadataStore.Filter([DocA, DocB], filters)
   |
   +--> DocumentStore.LoadMetadata/Preview     (optional)
   |
   v
=> Final ranked, filtered results
</pre>

