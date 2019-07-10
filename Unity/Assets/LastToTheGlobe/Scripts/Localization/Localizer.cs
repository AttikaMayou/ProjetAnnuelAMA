using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LastToTheGlobe.Scripts.Localization
{
	public class Localizer : MonoBehaviour
	{
		private static List<Localizer> localizers = new List<Localizer>();
	
		TextMeshProUGUI _textMesh;
		TMP_InputField _inputField;
		Image _image;
		AudioSource _audioSource;

		private static void Register(Localizer l)
		{
			localizers.Add(l);
		}

		private static void Unregister(Localizer l)
		{
			localizers.Remove(l);
		}

		public static void UpdateLocalization()
		{
			for (int i = 0; i < localizers.Count; ++i)
			{
				localizers[i].Localize();
			}
		}

		public void DynamicLocalize()
		{
			Localize();
		}
	
		public string ID;
		public bool LanguageSelect;
		public bool FinalChallenge;
		public bool ArabicNeedReverse;
		public bool credits;

		private void Awake()
		{
			Register(this);
		}

		private void OnDestroy()
		{
			Unregister(this);
		}

		private void Start ()
		{
			Localize();
		}

		private void Localize()
		{
			if (string.IsNullOrEmpty(ID))
			{
				Debug.Log("Id empty");
				return;
			}

			_textMesh = GetComponent<TextMeshProUGUI>();
			_inputField = GetComponent<TMP_InputField>();
			_image = GetComponent<Image>();
			_audioSource = GetComponent<AudioSource>();

			if (_textMesh != null)
			{
				//LocalizeTextMeshProUGUI(_textMesh, ID, LanguageSelect, FinalChallenge, ArabicNeedReverse, credits);
			}
			else if (_inputField != null)
			{
				LocalizeTMPInputField(ID);
			}
			/*else if (_image != null)
			{
				_image.sprite = LastToTheGlobe.Scripts.Localization.LocalizationManager.LocalizeSprite(ID);
			}

			if (_audioSource == null) return;
			_audioSource.clip = LastToTheGlobe.Scripts.Localization.LocalizationManager.LocalizeAudio(ID);

			if (_audioSource.loop)
			{
				_audioSource.Play();	
			}*/
		}

		private void LocalizeTMPInputField(string ID)
		{
			var lang = LastToTheGlobe.Scripts.Localization.LocalizationManager.GetLanguage();
			
			var textArea = _inputField.transform.GetChild(0);
			if (textArea != null)
			{
				var placeholder = textArea.GetChild(1);
				var text = textArea.GetChild(2);
				if (placeholder != null && text != null)
				{
					var placeholderTextMesh = placeholder.GetComponent<TextMeshProUGUI>();
					var inputTextMesh = text.GetComponent<TextMeshProUGUI>();

					if (placeholderTextMesh != null && inputTextMesh != null)
					{
						//LocalizeTextMeshProUGUI(placeholderTextMesh, ID);
						
						
						/*if (lang == SystemLanguage.Arabic || lang == SystemLanguage.Russian)
						{
							inputTextMesh.font = VisualNovelController.Instance.ArabicRussianFont;
						}
						else
						{
							inputTextMesh.font = VisualNovelController.Instance.LucienLight;
						}
						
						if (lang == SystemLanguage.Arabic)
						{
							placeholderTextMesh.alignment = TextAlignmentOptions.TopRight;
							inputTextMesh.alignment = TextAlignmentOptions.TopRight;
							inputTextMesh.isRightToLeftText = true;
						}
						else
						{
							placeholderTextMesh.alignment = TextAlignmentOptions.TopLeft;
							inputTextMesh.alignment = TextAlignmentOptions.TopLeft;
						}
						
						if (lang != SystemLanguage.Arabic)
						{
							_inputField.onValueChanged.RemoveAllListeners();
						}
						else
						{
							_inputField.onValueChanged.AddListener(OnInputFieldChanged);
						}*/
					}
					else
					{
						Debug.LogError(_inputField.gameObject.name + " : Either one of placeholder or text doesn't have a TextMeshProUGUI");
					}
				}
				else
				{
					Debug.LogError(_inputField.gameObject.name + " : Invalid transform layout for TMP_InputField component, cannot find one TextArea's children gameObject");
				}
			}
			else
			{
				Debug.LogError(_inputField.gameObject.name + " : Invalid transform layout for TMP_InputField component, cannot find TextArea child");
			}
		}

		/*private static void LocalizeTextMeshProUGUI(TextMeshProUGUI t, string ID, bool language = false, bool challenge = false, bool reverse = false, bool creditsText = false)
		{
			var lang = UserSaveManager.GetUserSave().preferredLanguage;

			if (lang != SystemLanguage.Arabic || lang != SystemLanguage.Russian)
			{
				t.enableAutoSizing = false;
			}
			if (lang == SystemLanguage.Arabic || lang == SystemLanguage.Russian)
			{
				switch (lang)
				{
					case SystemLanguage.Arabic:
					{
						if(t.alignment == TextAlignmentOptions.TopLeft)
							t.alignment = TextAlignmentOptions.TopRight;

						if (language)
						{
							t.enableAutoSizing = true;
						}
						break;
					}
					case SystemLanguage.Russian:
					{
						if (language)
						{
							t.autoSizeTextContainer = false;
							t.enableAutoSizing = true;
							//t.fontSize = 30;
							t.font = VisualNovelController.Instance.ArabicRussianFont;
						}
						if (challenge)
						{
							t.autoSizeTextContainer = false;
							t.fontSize = 50;
						}
						if (creditsText)
						{
							RussianSizeFix(t, ID, 10);
							return;
						}
						if(!language && !challenge)
						{
							RussianSizeFix(t, ID, 30);
							return;
						}

						break;
					}
				}

				t.font = VisualNovelController.Instance.ArabicRussianFont;
			}
			else if (lang != SystemLanguage.Arabic && lang != SystemLanguage.Russian)
			{
				if (ID == "PRIZE_TOURNAMENT")
				{
					t.enableAutoSizing = false;
				}
				else
				{
					t.enableAutoSizing = true;
				}
				
				
				if (t.alignment == TextAlignmentOptions.TopRight)
				{
					t.alignment = TextAlignmentOptions.TopLeft;
				}
				
				if (language)
				{
					t.enableAutoSizing = true;
					t.fontSize = 70;
				}
				if (challenge)
				{
					t.enableAutoSizing = true;
					t.fontSize = 80;
				}

				if (ID == "SELECTED_LANG")
				{
					t.font = VisualNovelController.Instance.LucienLight;
				}

				if (ID == "TITLE_SELECT_LANGUAGE")
				{
					t.font = VisualNovelController.Instance.LucienRegular;
				}
			}
			
			t.text = LastToTheGlobe.Scripts.Localization.LocalizationManager.LocalizeText(ID);
			if (lang != SystemLanguage.Arabic || !reverse) return;
			t.isRightToLeftText = true;
			t.text = ArabiceReverseFix(t.text);
		}

		private static void RussianSizeFix(TextMeshProUGUI t, string ID, int size)
		{
			t.font = VisualNovelController.Instance.ArabicRussianFont;
			t.enableAutoSizing = false;
			t.autoSizeTextContainer = false;
			t.fontSize = size;
			t.text = LastToTheGlobe.Scripts.Localization.LocalizationManager.LocalizeText(ID);
			t.ForceMeshUpdate();
		}

		private static string ArabiceReverseFix(string s)
		{
			var chars = s.ToCharArray();
			Array.Reverse(chars);
			return new String(chars);
		}
		
		private static void PrepareTextMeshProUGUIForArabic(TextMeshProUGUI t)
		{
			t.isRightToLeftText = LastToTheGlobe.Scripts.Localization.LocalizationManager.GetLanguage() == SystemLanguage.Arabic;
		}

		private void OnInputFieldChanged(string str)
		{
			if (LastToTheGlobe.Scripts.Localization.LocalizationManager.GetLanguage() != SystemLanguage.Arabic) return;
			_inputField.onValueChanged.RemoveAllListeners();
			_inputField.text = ArabicFixer.Fix(str,true,true,false);
			_inputField.text = Reverse(_inputField.text);
			_inputField.onValueChanged.AddListener(OnInputFieldChanged);
		}
		
		private string Reverse(string s)
		{
			var charArray = s.ToCharArray();
			Array.Reverse(charArray);
			return new String(charArray);
		}

		public void EndInput(string str)
		{
			Debug.Log("end input");
			if (UserSaveManager.GetUserSave().preferredLanguage != SystemLanguage.Arabic) return;
			if (!_inputField) return;
			var textArea = _inputField.transform.GetChild(0);
			var text = textArea.GetChild(2);
			var inputTextMesh = text.GetComponent<TextMeshProUGUI>();
			inputTextMesh.text = ArabicFixer.Fix(str);
			inputTextMesh.text = Reverse(inputTextMesh.text);
		}*/
	}
}
