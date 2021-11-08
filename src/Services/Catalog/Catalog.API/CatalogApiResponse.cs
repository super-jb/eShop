using System.Collections.Generic;
using System.Net;

namespace Catalog.API
{
    public record CatalogApiResponse
    {
        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;

        public IEnumerable<string> ErrorMessages { get; init; }
    }
}
