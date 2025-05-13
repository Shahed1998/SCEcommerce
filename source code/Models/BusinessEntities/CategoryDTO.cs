namespace Models.BusinessEntities
{
  
    public class CategoryDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? DisplayOrder { get; set; }

        public int Total { get; set; }

        public ICollection<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
