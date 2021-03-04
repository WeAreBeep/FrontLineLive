using System.Collections.Generic;

namespace Web.Models.Contentful
{
    public class PartnersPageViewModel : BasePageViewModel
    {
        public IEnumerable<PartnerViewModel> Partners { get; set; } = new List<PartnerViewModel>();
    }
}
