using System.Net;

namespace Ordering.Application.Models;

public record JsonApiResponse
{
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;

    public IEnumerable<string> ErrorMessages { get; init; }
}

