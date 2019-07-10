using UnityEngine;

namespace Majestic.Scripts.Localization
{
	public enum SupportedLanguagesEnum
	{
		ARABIC,
		ENGLISH,
		FRENCH,
		ITALIAN,
		RUSSIAN
	}

	public class SupportedLanguages {

		public static readonly SystemLanguage[] langs = {   SystemLanguage.Arabic,
			SystemLanguage.English,
			SystemLanguage.French,
			SystemLanguage.Italian,
			SystemLanguage.Russian};
	}
}