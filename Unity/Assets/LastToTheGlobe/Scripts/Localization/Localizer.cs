using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LastToTheGlobe.Scripts.Localization
{
	public class Localizer : MonoBehaviour
	{
		private static List<Localizer> _localizers = new List<Localizer>();

		[SerializeField] private TextMeshProUGUI textMesh;

		private static void Register(Localizer l)
		{
			if(!_localizers.Contains(l))
				_localizers.Add(l);
		}

		private static void Unregister(Localizer l)
		{
			_localizers.Remove(l);
		}

		public static void UpdateLocalization()
		{
			foreach (var t in _localizers)
			{
				t.Localize();
			}
		}

		public void DynamicLocalize()
		{
			Localize();
		}
	
		public string id;
		public bool languageSelect;

		private void Awake()
		{
			Register(this);
		}

		private void OnEnable()
		{
			Register(this);
		}

		private void OnDestroy()
		{
			Unregister(this);
		}

		private void OnDisable()
		{
			Unregister(this);
		}

		private void Start ()
		{
			Localize();
		}

		private void Localize()
		{
			if (string.IsNullOrEmpty(id))
			{
				Debug.Log("Id empty");
				return;
			}

			if (textMesh != null)
			{
				LocalizeTextMeshProUgui(textMesh, id);
			}
		}

		private static void LocalizeTextMeshProUgui(TextMeshProUGUI t, string id)
		{
			t.text = LocalizationManager.LocalizeText(id);
		}
	}
}
