using System;
using ContentfulModels = Contentful.Core.Models;

namespace Web.Models.Contentful
{
    public class BasePageViewModel
    {
        public ContentfulModels.SystemProperties Sys { get; set; }

        public ContentfulModels.Document Content { get; set; }
    }
}
