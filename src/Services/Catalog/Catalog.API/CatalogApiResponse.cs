using System.Net;

namespace Catalog.API
{
    public record CatalogApiResponse
    {
        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;

        public string ErrorMessage { get; init; }
    }
}
