# Post-Lab Follow-up: ApplicationContentIndexer Semantic Search

## Private Preview SDK Available

We have a private preview SDK ready for building lexical and semantic search over your application content to make content more discoverable for your users.

**Sign up for access:** https://aka.ms/WindowsAISemanticSearch

We would be really excited to chat with you if you are interested in trying out our private SDK, wanting to receive information about our public preview roadmap, or to discuss requirements and use cases.

## What to Expect: Private SDK Integration

### Overview

Semantic Search and Knowledge Retrieval empowers app developers to structure and store in-app content in a way that allows users to search by meaning or intent – not just keywords – enabling faster, more relevant results.

Using the ApplicationContentIndexer API enables you to index content from within your app so that it can be queried to retrieve relevant matches based on both exact terms and semantic meaning.

### Use Cases

Use ApplicationContentIndexer to:
- **Enhance in-app search experiences** by supporting semantic search. Users can search by meaning—not just exact keyword matches—making it easier to find relevant information.
- **Support Retrieval-Augmented Generation (RAG)** by enabling local knowledge retrieval. At its core, RAG enables a generative AI model to "look things up" before answering. Instead of relying solely on its pre-trained knowledge (which can be outdated or incomplete), the model retrieves relevant documents or data from an external source and uses that information to generate a more accurate, grounded, and context-aware response.

Whether you're building smarter search into your app or enabling AI assistants with domain-specific knowledge, ApplicationContentIndexer provides a flexible foundation to get started quickly.

### Prerequisites

To use the ApplicationContentIndexer APIs, make sure your development environment meets the following requirements:

- **Copilot+ PC**
- **Windows Insider Program** - Beta Channel - Windows 11, version 24H2 or newer (Build 26120.4151)
- **WinAppSDK 1.8 Experimental 2**, version 1.8.250515001-experimental2 (a Public Preview Release)
- **ApplicationContentIndexer API nuget package** (a Private Preview Release)

For more info on environment setup, including systemaiModels capability checks, refer to the guide here: [Get started building an app with Windows AI APIs](https://learn.microsoft.com/windows/ai/)

### Important Considerations

> [!IMPORTANT] 
> This feature is currently in private preview and should not be used in production environments.

#### Privacy and Security
Semantic and lexical indexes are generated on behalf of your app and stored in the app's local app data folder. As part of the private preview release, this feature is intended for indexing non-sensitive application content. For best security practices, do not use this feature to index user data that may contain personal, confidential, or sensitive information.

#### Responsible AI Considerations
The semantic indexing and search capabilities in this preview do not apply any form of content moderation, nor do they attempt to detect or mitigate semantic bias introduced by the underlying models. Developers are responsible for evaluating and managing the potential risks when implementing AI-powered features.

We recommend reviewing the Responsible Generative AI Development on Windows guidelines for best practices when building AI experiences in your app.

### Key Concepts

#### Semantic and Lexical Search
Internally, ApplicationContentIndexer uses a combination of traditional text indexing and modern vector-based search powered by embeddings. These details are abstracted away – developers do not need to manage embedding models, vector storage, or retrieval infrastructure directly.

You can query the index using a plain string. The query may return:
- **Lexical matches** – exact text matches (including text found within images)
- **Semantic matches** – content that is similar in meaning, even if the words are not identical

For example, a query for "kitten" might return a reference to:
- Text entries about cats, even if the word "kitten" isn't explicitly mentioned
- Images that visually contain kittens
- Textual content in images that contain 'cat' or words with enough semantic relevance

#### Supported Content Types
ApplicationContentIndexer supports adding the following types of content:
- **Text** – plain or structured text content
- **Images** – including screenshots, photos, or image files that contain text or recognizable visual elements

#### Index Persistence
When you create an ApplicationContentIndexer, the index is saved to disk. This allows your app to retain indexed data across sessions without needing to rebuild the index on each launch.

#### Static Data Management
When indexing context using the private preview, there is no built-in indication whether the index was newly created or retrieved from the disk after calling `GetOrCreateIndexerAsync`. This may lead to performance issues, as static data that does not change may be re-indexed every time the app launches.

**Recommended Workaround:** Implement a unique index marker system. When the app launches, first search the index for a special marker entry (e.g., `"__INDEX_MARKER__:{indexName}"`) that signals static data has been added. This ensures efficient indexing across app sessions.

#### Data Ingestion and Storage
When indexing content using ApplicationContentIndexer, your app provides either text or image data directly. The API indexes the content but does not store a full copy of it. Instead, it relies on Content IDs that your app provides and manages.

- **Text indexing**: Results return an `AppManagedTextQueryMatch` with the matching ContentId and offset/length information
- **Image indexing**: Results return an `AppManagedImageQueryMatch` with the ContentId and optional bounding box for textual matches

This approach avoids content duplication and ensures that the app remains in control of its data.