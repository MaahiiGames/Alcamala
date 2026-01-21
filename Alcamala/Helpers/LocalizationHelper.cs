using Alcamala.Enums;
using System.Globalization;

namespace Alcamala.Helpers;

public static class LocalizationHelper
{
    public static Language CurrentLanguage { get; private set; } = GetLanguageFromCulture(CultureInfo.CurrentUICulture);

    public static void SetLanguage(Language language)
    {
        CurrentLanguage = language;

        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(TwoLetterISOLanguageNames[language]);
    }

    private static Language GetLanguageFromCulture(CultureInfo culture)
    {
        return culture.TwoLetterISOLanguageName switch
        {
            "en" => Language.English,
            "nl" => Language.Dutch,
            _ => Language.English
        };
    }

    private static readonly Dictionary<Language, string> TwoLetterISOLanguageNames = new()
    {
        { Language.English, "en" },
        { Language.Dutch, "nl" }
    };
}