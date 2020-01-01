using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BoardProject.Models
{
    /* Localizer class, uses JSON-based dictionaries to enable
     * seamless translation of strings between various languages */
    public class Localizer
    {
        private string locale;

        public Localizer(string language = "en")
        {
            SetLocale(language);
        }
        public void SetLocale(UserBase user, Controller controller = null)
        {
            /* Pull user language from user object */
            if (!string.IsNullOrEmpty(user.Language))
                SetLocale(user.Language);

            /* If we got controller object, add session variable to keep language
             * setting for other parts that don't require user objects to function */
            if (controller != null)
                controller.HttpContext?.Session?.SetString("Language", locale);
        }
        public void SetLocale(string language)
        {
            /* Check for empty string */
            if (string.IsNullOrEmpty(language))
                return;

            /* Check if legitimate language identifier */
            if (Translations.AvailableTranslations.Contains(language))
                locale = language;
            else /* Something bogus has entered the system */
                throw new Exception("Invalid language ID was provided to Localizer object");
        }
        
        /* [] operator overload for seamless use */
        public string this[string ident]
        {
            get { return Translations.GetString(locale, ident); }
        }

        /* Static translation data container */
        public static class Translations
        {
            private static readonly Dictionary<string, Dictionary<string, string>> translations =
                new Dictionary<string, Dictionary<string, string>> {
                { "en", JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@"Lang\English.json", System.Text.Encoding.UTF8)) },
                { "he", JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(@"Lang\Hebrew.json", System.Text.Encoding.UTF8)) }
                    };
            public static ReadOnlyCollection<string> AvailableTranslations { get; }

            public static string GetString(string langid, string ident)
            {
                try
                {
                    return translations[langid][ident];
                }
                catch (KeyNotFoundException)
                {
                    return ident;
                }
            }
            static Translations()
            {
                List<string> result = new List<string>(translations.Count);

                foreach (KeyValuePair<string, Dictionary<string, string>> entry in translations)
                    result.Add(entry.Key);

                AvailableTranslations = result.AsReadOnly();
            }
        }
    }
}
