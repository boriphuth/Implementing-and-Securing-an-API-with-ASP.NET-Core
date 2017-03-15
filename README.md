# Implementing and Securing an API with ASP.NET Core
Code along with "[Implementing and Securing an API with ASP.NET Core](https://app.pluralsight.com/library/courses/aspdotnetcore-implementing-securing-api/table-of-contents)" by Shawn Wildermuth: https://app.pluralsight.com/library/courses/aspdotnetcore-implementing-securing-api/table-of-contents.

## Description
Building an API with ASP.NET Core is an obvious choice for solutions that require cross-platform hosting, micro-service architecture, or just broad scale. This course will show you how to do just that.

## Http
### Verbs
- **GET**: Retrieve a resource
- **POST**: Add a new resource
- **PUT**: Update an existing resource
- **PATCH**: Update an existing resourcie with set of changes
- **DELETE**: Remove the existing resource
- ...

## REST
Resource Based Architecture, resource are representations or real world entities.
- E.g.: people, invoice, payments, etc..
  - relationschrips are typically nested
  - hierarchies or webs, not relational models
- URIs are paths to resources
  - Query strings fro non data elements (e.g. formatting, sorting, paging)
- **REST** REpresentational State Transer
  - Seperation of Client <-> Server
  - Server Requests are stateless
  - Caheable Requests are encouraged (e.g. getting common piece of data, or image from server)
  - Uniform interface (e.g. using URI's & as easy to read as possible, using the same pattern everywhere)
  - **Problems**:
    - To become qualified REST tends to be difficult
    - Community is split about the REST Dogma
    - Being pragmatic is important
- **API's on the Web**
  - Verbs included in Api (HTTP/RPC) vs REST approach
  - **Pragmatic REST**:
    - URI Endpoints
    - Resource URIs
    - HTTP Verbs
    - Stateless Server
    - Content Negotiation
    - But **not** link relations (e.g. hypermedia), and no verbs included in API endpoints...
