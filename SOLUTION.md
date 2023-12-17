## Gravatar Service

The Gravatar service needed access to an HttpClient to call the Gravatar API.

I decided to change this so it was registered with DI and injected where it was required.
That way it could be a singleton or scoped service and make use of the `IHttpClientFactory`.

This also made it easier to write tests for this Service.

I made it a singleton for now so the simple dictionary cache works but left a comment there
about replacing this with an `IMemoryCache` or Redis or similar.