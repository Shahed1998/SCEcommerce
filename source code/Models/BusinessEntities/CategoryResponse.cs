namespace Models.BusinessEntities
{
    public class CategoryResponse
    {
        public bool? showtoastMessage { get; set; }
        public bool? success { get; set; }
        public string? p { get; set; } // represents encrypted params
    }
}
