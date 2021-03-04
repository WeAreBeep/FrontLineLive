using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contentful.Core;

namespace Web.Services
{
    public interface IContentfulService
    {

        Task<T> GetFirstByContentType<T>(string contentTypeId) where T : class;
    }

    public class ContentfulService : IContentfulService
    {
        private readonly IContentfulClient _contentfulClient;

        public ContentfulService(IContentfulClient contentfulClient)
        {
            _contentfulClient = contentfulClient;
            _contentfulClient.ContentTypeResolver = new Configuration.Contentful.ContentfulModuleResolver();
        }

        public async Task<T> GetFirstByContentType<T>(string contentTypeId) where T : class
        {
            var queryBuilder = new Contentful.Core.Search.QueryBuilder<T>().Limit(1);
            var resp = await _contentfulClient.GetEntriesByType(contentTypeId, queryBuilder);
            if (resp.Total == 0)
            {
                return null;
            }
            return new List<T>(resp.Items).First();
        }
    }
}
