using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Web.Infrastructure
{
    public static class Helper
    {
        // Altering body CSS class won't work in minified CSS with class renaming
        // FIXME: Use dynamic layout to handle different style of body.
        private const string workerableStylesClass = "workable_styles";
        private const string invertedClass = "inverted";
        public static string GetBodyClass(ViewDataDictionary viewData)
        {
            var bodyClasses = new List<string>{ workerableStylesClass };
            if(viewData.TryGetValue(Settings.LayoutFrontEndModeEditKey, out object isFrontEnd) && Convert.ToBoolean(isFrontEnd))
            {
                bodyClasses.Remove(workerableStylesClass);
            }
            if (viewData.TryGetValue(Settings.LayoutInvertedKey, out var isInverted) && Convert.ToBoolean(isInverted))
            {
                bodyClasses.Add(invertedClass);
            }
            return String.Join(" ", bodyClasses);
        }
    }
}