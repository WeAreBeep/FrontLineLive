using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Contentful.Core.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContentfulModels = Contentful.Core.Models;

namespace Web.Infrastructure.Contentful.Renderers
{
    /// <summary>
    /// This renderer is a custom renderer which renders Hyperlink with single Text content only.
    /// The difference between <see cref="HyperlinkContentRenderer"/> and this customer renderer is that this renderer
    //  will specify the target to be "_blank" for external link.
    /// </summary>
    public class SimpleHyperlinkRenderer : IContentRenderer
    {
        public int Order { get; set; }

        public string Render(IContent content)
        {
            var link = content as Hyperlink;
            var innerText = (ContentfulModels.Text) link.Content.First();
            var tb = new TagBuilder("a");
            string result = "";
            using (var writer = new StringWriter())
            {
                tb.MergeAttribute("href", link.Data.Uri);
                tb.MergeAttribute("title", link.Data.Title);
                tb.InnerHtml.SetHtmlContent(RenderText(innerText));
                if (CheckIsExternalUri(link.Data.Uri)) {
                    tb.MergeAttribute("target", "_blank");
                }
                tb.WriteTo(writer, HtmlEncoder.Default);
                result = writer.ToString();
            }
            return result;
        }

        public Task<string> RenderAsync(IContent content)
        {
            return Task.FromResult(Render(content));
        }

        public bool SupportsContent(IContent content)
        {
            if (!(content is ContentfulModels.Hyperlink link))
            {
                return false;
            }
            if (link.Content.Count > 1 || !(link.Content.First() is ContentfulModels.Text))
            {
                return false;
            }
            return true;
        }

        private bool CheckIsExternalUri(string uriLike)
        {
            if (String.IsNullOrEmpty(uriLike))
            {
                return false;
            }
            try
            {
                var uri = new Uri(uriLike);
                return !String.IsNullOrEmpty(uri.Host);
            }
            catch
            {
                // uriLike may be an invalid URI or a relative path.
                // So it won't be an external link
                return false;
            }
        }

        /// <summary>
        /// Copy from https://github.com/contentful/contentful.net/blob/213152252f029fc243c14e22d93994647b86dbd3/Contentful.Core/Models/Authoring.cs#L307
        /// <summary>
        public string RenderText(ContentfulModels.Text text)
        {
            var sb = new StringBuilder();

            if (text.Marks != null)
            {
                foreach (var mark in text.Marks)
                {
                    sb.Append($"<{MarkToHtmlTag(mark)}>");
                }
            }

            sb.Append(text.Value);

            if (text.Marks != null)
            {
                foreach (var mark in text.Marks)
                {
                    sb.Append($"</{MarkToHtmlTag(mark)}>");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Copy from https://github.com/contentful/contentful.net/blob/213152252f029fc243c14e22d93994647b86dbd3/Contentful.Core/Models/Authoring.cs#L333
        /// <summary>
        private string MarkToHtmlTag(Mark mark)
        {
            switch (mark.Type)
            {
                case "bold":
                    return "strong";
                case "underline":
                    return "u";
                case "italic":
                    return "em";
                case "code":
                    return "code";
            }

            return "span";
        }
    }
}
