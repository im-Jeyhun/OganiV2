namespace OganiWebApp.Areas.Client.ViewModels
{
    public class SliderViewModel
    {
        public SliderViewModel(string title, string bgImageName, string bgImageNameUrl, int porductCategoryId)
        {
            Title = title;
            BgImageName = bgImageName;
            BgImageNameUrl = bgImageNameUrl;
            PorductCategoryId = porductCategoryId;
        }

        public string Title { get; set; }
        public string BgImageName { get; set; }
        public string BgImageNameUrl { get; set; }
        public int PorductCategoryId { get; set; }
    }
}
