using UnityEngine;

namespace LastToTheGlobe.Scripts.Localization
{
	public enum SupportedLanguagesEnum
	{
		ENGLISH,
		FRENCH,
	}

	public class SupportedLanguages
	{

		public static readonly SystemLanguage[] Langs =
		{
			SystemLanguage.English,
			SystemLanguage.French
		};
	}
}