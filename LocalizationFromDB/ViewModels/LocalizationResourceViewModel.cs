namespace LocalizationFromDB.ViewModels
{
    public class LocalizationResourceViewModel
    {
        public string ResourceKey { get; set; }
        public Dictionary<string,string> LocalizedValues { get; set; } = new Dictionary<string, string>();
    }
}
