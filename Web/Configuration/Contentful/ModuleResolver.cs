using System;
using System.Collections.Generic;
using Contentful.Core.Configuration;
using Web.Models.Contentful;

namespace Web.Configuration.Contentful
{
    /// <summary>
    /// Resolves a strong type from a content type id. Instructing the serialization engine how to deserialize items in a collection.
    /// </summary>
    public class ContentfulModuleResolver : IContentTypeResolver
    {
        private Dictionary<string, Type> _types = new Dictionary<string, Type>()
        {
            { "home-page", typeof(HomePageViewModel) },
            { "about-us-page", typeof(AboutUsPageViewModel) },
            { "partners-page", typeof(PartnersPageViewModel) },
            { "partner", typeof(PartnerViewModel) }
        };

        /// <summary>
        /// Method to get a type based on the specified content type id.
        /// </summary>
        /// <param name="contentTypeId">The content type id to resolve to a type.</param>
        /// <returns>The type for the content type id or null if none is found.</returns>
        public Type Resolve(string contentTypeId)
        {
            return _types.TryGetValue(contentTypeId, out var type) ? type : null;
        }
    }
}
