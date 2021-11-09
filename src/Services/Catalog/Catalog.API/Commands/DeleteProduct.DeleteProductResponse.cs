namespace Catalog.API.Commands
{
    public record DeleteProductResponse : CatalogApiResponse
    {
        public bool Deleted;
    }
}
