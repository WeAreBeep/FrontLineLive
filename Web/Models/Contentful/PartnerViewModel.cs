using ContentfulModels = Contentful.Core.Models;

namespace Web.Models.Contentful
{
    public class PartnerViewModel
    {
        public ContentfulModels.SystemProperties Sys { get; set; }

        public string WebsiteUrl { get; set; }

        public ContentfulModels.Asset Logo { get; set; }
    }
}
