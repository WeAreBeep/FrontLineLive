using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Linq;
using System.Linq.Expressions;
using Web.Snippets.System.ComponentModel.DataAnnotations;

namespace Web.Snippets.Microsoft.AspNetCore.Mvc.Rendering
{
	public static class HtmlExtensions
	{
		public static IHtmlContent LabelsFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
		{
			IHtmlContent labelP = html.LabelPFor(expression);
			IHtmlContent descriptionP = html.DescriptionPFor(expression);
			return LabelsFor(html, expression, labelP, descriptionP);
		}

		public static IHtmlContent LabelsFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, string descriptionText)
		{
			IHtmlContent labelP = html.LabelPFor(expression, labelText);
			IHtmlContent descriptionP = html.DescriptionPFor(expression, descriptionText);
			return LabelsFor(html, expression, labelP, descriptionP);
		}

		public static IHtmlContent LabelsFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IHtmlContent labelP, IHtmlContent descriptionP)
		{
			TagBuilder tagBuilder = new TagBuilder("div");
			tagBuilder.InnerHtml.AppendHtml(labelP);
			tagBuilder.InnerHtml.AppendHtml(descriptionP);
			return tagBuilder;
		}

		public static IHtmlContent NumberPickerFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
		{
			IHtmlContent respVal = new HtmlString("");
			if(expression.Body is MemberExpression memEx)
			{
				NumericalRangeAttribute range = memEx.Member.GetCustomAttributes(typeof(NumericalRangeAttribute), false)
					.Cast<NumericalRangeAttribute>().Single();
				string id = html.IdFor(expression); 
				string name = html.NameFor(expression);
				respVal = new HtmlString($@"<number-input v-model='{name}' id='{id}' name='{name}' :step='{range.Step}' :min='{range.Min}' :max='{range.Max}' class='number_picker' inline controls></number-input>");
			}
			return respVal;
		}

		public static IHtmlContent DescriptionFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
		{
			if(html == null) throw new ArgumentNullException(nameof(html));
			if(expression == null) throw new ArgumentNullException(nameof(expression));

			ModelExpression modelExpression = getModelExpression(html, expression);
			return new HtmlString(modelExpression.Metadata.Description);
		}

		public static IHtmlContent DescriptionPFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
		{
			return DescriptionPFor(html, expression, html.DescriptionFor(expression));
		}

		public static IHtmlContent DescriptionPFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string descriptionText)
		{
			return DescriptionPFor(html, expression, new HtmlString(descriptionText));
		}

		public static IHtmlContent DescriptionPFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IHtmlContent innerHtml)
		{
			TagBuilder tagBuilder = new TagBuilder("p");
			tagBuilder.AddCssClass("description");
			tagBuilder.InnerHtml.AppendHtml(innerHtml);
			return tagBuilder;
		}

		public static IHtmlContent LabelPFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
		{
			return LabelPFor(html, expression, html.LabelFor(expression));
		}

		public static IHtmlContent LabelPFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText)
		{
			return LabelPFor(html, expression, html.LabelFor(expression, labelText));
		}

		public static IHtmlContent LabelPFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IHtmlContent innerHtml)
		{
			TagBuilder tagBuilder = new TagBuilder("p");
			tagBuilder.InnerHtml.AppendHtml(innerHtml);
			ModelExpression modelExpression = getModelExpression(html, expression);
			if(modelExpression.Metadata.IsRequired)
			{
				tagBuilder.AddCssClass("required");
			}
			return tagBuilder;
		}

		static ModelExpression getModelExpression<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
		{
			ModelExpressionProvider modelExpressionProvider = (ModelExpressionProvider)html.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));
			ModelExpression modelExpression = modelExpressionProvider.CreateModelExpression(html.ViewData, expression);
			if(modelExpression == null)
				throw new InvalidOperationException($"Failed to get model explorer for {modelExpressionProvider.GetExpressionText(expression)}");
			else
				return modelExpression;
		}
	}
}