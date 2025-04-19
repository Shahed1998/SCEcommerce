namespace Models.BusinessEntities
{
    public class NotificationViewModel
    {
        public bool? IsCreated { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsEdited { get; set; }
        public bool? showtoastMessage { get; set; }
        public bool? success { get; set; }
        public string? p { get; set; } // represents encrypted params
    }
}
