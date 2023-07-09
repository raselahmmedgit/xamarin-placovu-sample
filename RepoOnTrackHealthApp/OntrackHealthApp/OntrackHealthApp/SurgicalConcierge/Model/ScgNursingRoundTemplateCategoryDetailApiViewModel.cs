namespace OntrackHealthApp.SurgicalConcierge.Model
{
    public class ScgNursingRoundTemplateCategoryDetailApiViewModel
    {
        public long TemplateCategoryDetailId { get; set; }
        public string TemplateCategoryDetailText { get; set; }

        public long? TemplateCategoryId { get; set; }
        public bool IsFloorPhoneField { get; set; }
        public bool IsCustomMessageField { get; set; }
        public bool IsDeleted { get; set; }
        public long? DisplayOrder { get; set; }
    }
}