using System;
using Microsoft.CodeAnalysis.Operations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Web.Snippets;
using Web.Snippets.Microsoft.Extensions.Configuration;
using Web.Snippets.System;

namespace Web.Infrastructure
{
    public static class Settings
    {
        public static string NeedsImportUrl = "http://x.api/export/needs/";
        public static string SuppliesImportUrl = "http://x.api/export/supplies/";
		public static string LayoutFrontEndModeEditKey => $"{nameof(LayoutFrontEndModeEditKey)}";
        public static string LayoutInvertedKey => $"{nameof(LayoutInvertedKey)}";
		public static string DataTablesTimestampFormat => "yyyy-MM-dd HH:mm:ss";
		public static string DefaultEmail => "noone@nowhere.com";
		public static string GetTwitterUrl(long? tweetId) => tweetId != null ? $"https://twitter.com/i/web/status/{tweetId}" : "";
		public static string GetUshahidiUrl(long? ushahidiId) => ushahidiId != null ? $"https://frontlinehelp.ushahidi.io/posts/{ushahidiId}/edit" : ""; 

        public static string GetTimestamp(DateTimeOffset? timestamp) => timestamp?.ToString(Settings.DataTablesTimestampFormat) ?? "";


        public class ValMsgs
        {
            public const string Phone = "<b>{0}</b> must be a valid phone number";
            public const string StringLength = "<b>{0}</b> must be no longer than {1} characters long";
            public const string StringLengthWithMinimum = "<b>{0}</b> must be between {2} and {1} characters long";
            public const string NumRange = "<b>{0}</b> must be between {1} and {2}";
            public const string Email = "<b>{0}</b> must be a valid email";
            public const string Date = "<b>{0}</b> must be a date";
            public const string Required = "Please enter <b>{0}</b>";
            public const string Url = "<b>{0}</b> must be a valid URL (complete with protocol e.g. http://google.com)";
        }

        public static class Emails
        {
            public static string SendGridKey => ConfigMan.Values[$"{nameof(Emails)}:{nameof(SendGridKey)}"];
            public static string ToAddresses => ConfigMan.Values[$"{nameof(Emails)}:{nameof(ToAddresses)}"];
            public static string FromAddress => ConfigMan.Values[$"{nameof(Emails)}:{nameof(FromAddress)}"];
            public static string[] ToAddressesList => ToAddresses.Split(',');
        }

        public static class ReCaptcha
        {
            public static string SecretKey => ConfigMan.Values[$"{nameof(ReCaptcha)}:{nameof(SecretKey)}"];
            public static string SiteKey => ConfigMan.Values[$"{nameof(ReCaptcha)}:{nameof(SiteKey)}"];
        }

        public static class ConnectionStrings
        {
            public static string DataContext => ConfigMan.Values[$"{nameof(ConnectionStrings)}:{nameof(DataContext)}"];
        }

        public static DeploymentContext DeploymentContext => Enums.Parse<DeploymentContext>(ConfigMan.Values["DeploymentContext"]);

        public static class Notifications
        {
            public static bool ShowUserExceptionDetails => Convert.ToBoolean(ConfigMan.Values[$"{nameof(Notifications)}:{nameof(ShowUserExceptionDetails)}"]);
            public static string SystemWatcher => ConfigMan.Values[$"{nameof(Notifications)}:{nameof(SystemWatcher)}"];
            public static bool LogAllMessages => Convert.ToBoolean(ConfigMan.Values[$"{nameof(Notifications)}:{nameof(LogAllMessages)}"]);
            public static bool LogAllIntegrationDetails => Convert.ToBoolean(ConfigMan.Values[$"{nameof(Notifications)}:{nameof(LogAllIntegrationDetails)}"]);
            public static bool ShowUserIntegrationDetails => Convert.ToBoolean(ConfigMan.Values[$"{nameof(Notifications)}:{nameof(ShowUserIntegrationDetails)}"]);
        }

        /// <summary>
        /// camelCasing 
        /// </summary>
        public static JsonSerializerSettings ClientSideSerializerSettings => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(), StringEscapeHandling = StringEscapeHandling.EscapeHtml //EscapeHtmlInJson? StringEscapeHandling.EscapeHtml : StringEscapeHandling.Default //can toggle for debugging as escaping is pretty annoying (makes all the e.g. render templates in tabledata unreadable) 
        };

        public static class Authorization
        {
            public const string EditRights = "EditRights";
            public static string[] EditRightsRoles => new[]
            {
                SecurityRoles.Volunteer.ToString(),
                SecurityRoles.Admin.ToString(),
                SecurityRoles.SuperAdmin.ToString()
            };
        }

       
    }
}