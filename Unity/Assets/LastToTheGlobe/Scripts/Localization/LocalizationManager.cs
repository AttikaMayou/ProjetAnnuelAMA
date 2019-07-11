using System;
using System.Collections.Generic;
using UnityEngine;

namespace LastToTheGlobe.Scripts.Localization
{
	public class LocalizationManager 
	{
		private static SystemLanguage _language;
		private static Dictionary<string, string> _dictionary = new Dictionary<string, string>();
	
		public static void SetLanguage(SystemLanguage lang)
		{
			var result = Array.Find(SupportedLanguages.Langs, systemLanguage => systemLanguage == lang);
			if (result != default)
			{
				_language = result;
			}
			else
			{
				Debug.LogError("Unsupported lang " + lang + ", fallback to english.");
				_language = SystemLanguage.English;
			}
			
			_dictionary.Clear();
			var textDictionary = Resources.Load("Localization" + _language) as TextAsset;
			ParseTextDictionary(textDictionary, _dictionary);
			
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
						Debug.LogError(txt.name + " : Content at line " + lineIndex + " for key " + key + " is empty");
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
			return _language;
		}

		public static string LocalizeText(string ID)
		{
			var translation = _dictionary[ID];

			return translation;
		}
	}
}
