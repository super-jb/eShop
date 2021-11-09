namespace Catalog.API.Commands
{
    public record DeleteProductResponse : JsonApiResponse
    {
        public bool Deleted;
    }
}
