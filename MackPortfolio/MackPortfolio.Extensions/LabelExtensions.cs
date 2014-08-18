using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MackPortfolio.Extensions
{
    public static class LabelExtensions
    {
        public static MvcHtmlString Label(this HtmlHelper html, string expression, string id = "", bool generatedId = false)
        {
            var metadata = ModelMetadata.FromStringExpression(expression, html.ViewData);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? id.Split('.').Last();
            return LabelHelper(html, metadata, expression, id, generatedId);
        }

        public static MvcHtmlString RequiredLabel(this HtmlHelper html, string expression, string id = "", bool generatedId = false)
        {
            var metadata = ModelMetadata.FromStringExpression(expression, html.ViewData);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? id.Split('.').Last();
            return LabelHelper(html, metadata, expression, labelText);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText = "")
        {
            return LabelHelper(html,
                ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                ExpressionHelper.GetExpressionText(expression), labelText);
        }

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
        public static MvcHtmlString RequiredLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            return LabelHelper(helper, metaData, htmlFieldName, string.Empty);
        }

        internal static MvcHtmlString LabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string id, bool generatedId)
        {
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }
            var sb = new StringBuilder();
            sb.Append(labelText);
            if (metadata.IsRequired)
                sb.Append("*");

            var tag = new TagBuilder("label");
            if (!string.IsNullOrWhiteSpace(id))
            {
                tag.Attributes.Add("id", id);
            }
            else if (generatedId)
            {
                tag.Attributes.Add("id", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName) + "_Label");
            }

            if (!string.IsNullOrEmpty(metadata.Description))
            {

                tag.Attributes.Add("title", metadata.Description);
            }

            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(sb.ToString());

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        internal static MvcHtmlString LabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string labelText)
        {
            if (string.IsNullOrEmpty(labelText))
            {
                labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
            }

            if (string.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            bool isRequired = false;

            if (metadata.ContainerType != null)
            {
                //isRequired = metadata.ContainerType.GetProperty(metadata.PropertyName)
                //                .GetCustomAttributes(typeof(RequiredAttribute), false)
                //                .Length == 1;
                isRequired = metadata.IsRequired;
            }

            TagBuilder tag = new TagBuilder("label");
            tag.Attributes.Add("for", TagBuilder.CreateSanitizedId(
                    html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)
                )
            );

            if (!string.IsNullOrEmpty(metadata.Description))
            {

                tag.Attributes.Add("title", metadata.Description);
            }

            if (isRequired)
            {
                tag.Attributes.Add("class", "label-required");

                var asteriskTag = new TagBuilder("span");
                asteriskTag.Attributes.Add("class", "field-required");
                asteriskTag.SetInnerText(" *");
                tag.InnerHtml = labelText + asteriskTag.ToString(TagRenderMode.Normal);
            }
            else
            {
                tag.SetInnerText(labelText);
            }

            var output = tag.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(output);
        }
    }
}
