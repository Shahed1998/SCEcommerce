namespace Models.BusinessEntities
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public long? Price { get; set; }

        public int? DiscountPercentage { get; set; }

        public string? ImageUrl { get; set; }

        public int? CategoryId { get; set; }

        public CategoryDTO? Category { get; set; }

    }
}
