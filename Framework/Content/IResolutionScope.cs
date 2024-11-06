﻿namespace StardewUI.Framework.Content;

/// <summary>
/// Defines a scope in which certain types of external and potentially ambiguous binding attributes may be resolved.
/// </summary>
/// <remarks>
/// In general, resolution scopes are used where a document may include an unqualified or short-form reference, for
/// which such a reference means to look in the same mod that originally provided that document.
/// </remarks>
public interface IResolutionScope
{
    /// <summary>
    /// Attempts to obtain a translation value with the given key.
    /// </summary>
    /// <param name="key">The qualified or unqualified translation key. Unqualified keys are identical to their name in
    /// the translation file (i.e. in <c>i18n/default.json</c>), while qualified keys include a prefix with the specific
    /// mod, e.g. <c>authorname.ModName:TranslationKey</c>.</param>
    /// <returns>The translation, if available in the current language; the default-language (usually English) string if
    /// the <paramref name="key"/> exists but no translation is available; or a placeholder value if the
    /// <paramref name="key"/> is not valid or missing.</returns>
    Translation GetTranslation(string key);
}
