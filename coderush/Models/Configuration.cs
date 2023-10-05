namespace CodesDotHRMS.Models
{
    public class Configuration
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool? IsActive { get; set; }
    }
}
