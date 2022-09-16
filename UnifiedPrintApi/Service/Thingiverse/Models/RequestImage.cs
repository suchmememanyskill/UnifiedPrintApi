namespace UnifiedPrintApi.Service.Thingiverse.Models
{
    public class RequestImage
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Uri Url { get; set; }
        public List<Size> Sizes { get; set; }
    }
}
