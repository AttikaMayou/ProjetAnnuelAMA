using System;
using System.Collections.Generic;
using Majestic.Scripts.Localization;
using UnityEngine;

namespace LastToTheGlobe.Scripts.Localization
{
	public class LocalizationManager {

		private static SystemLanguage language;
		private static Dictionary<string, string> dictionary = new Dictionary<string, string>();
	
		public static void SetLanguage(SystemLanguage lang)
		{
			var result = Array.Find(SupportedLanguages.langs, systemLanguage => systemLanguage == lang);
			if (result != default(SystemLanguage))
			{
				language = result;
			}
			else
			{
				Debug.LogError("Unsupported lang " + lang + ", fallback to english.");
				language = SystemLanguage.English;
			}

//			UserSaveManager.GetUserSave().preferredLanguage = language;
//			UserSaveManager.SaveSettings();
		
			dictionary.Clear();
			//var textDictionary = PatchManager.Instance.GetAsset<TextAsset>("Localization/" + language);
			//ParseTextDictionary(textDictionary, dictionary);
			Localizer.UpdateLocalization();
		}

		private static void ParseTextDictionary(TextAsset txt, Dictionary<string, string> dict)
		{
			var lines = txt.text.Split(new[]{"\n", "\r", "\r\n"}, StringSplitOptions.None);
			
			var lineIndex = 1;
			foreach (var line in lines)
			{
				var separatorIndex = line.IndexOf("\t", StringComparison.Ordinal);
				if (line.Length != 0 && separatorIndex != -1)
				{
					var key = line.Substring(0, separatorIndex);
					var content = line.Substring(separatorIndex + 1);

					if (key.Length == 0)
					{
						Debug.LogError(txt.name + " : Key at line " + lineIndex + " is empty");
						continue;
					}

					if (content.Length == 0)
					{
						Debug.LogError(txt.name + " : content at line " + lineIndex + " for key " + key + " is empty");
						continue;
					}
				
					content = content.Replace("\\n", "\n");
					dict.Add(key, content);
				}

				++lineIndex;
			}
		}

		public static SystemLanguage GetLanguage()
		{
			return language;
		}

		public static string LocalizeText(string ID)
		{
			var translation = dictionary[ID];

			return translation;
		}

//		public static Sprite LocalizeSprite(string id)
//		{
//			return PatchManager.Instance.GetAsset<Sprite>("Localization/" + language.ToString() + "/Sprites/" + id);
//		}
//
//		public static AudioClip LocalizeAudio(string id)
//		{
//			return PatchManager.Instance.GetAsset<AudioClip>("Localization/" + language.ToString() + "/Audio/" + id);
//		}
	}
}
