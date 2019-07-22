using UnityEngine;
using System.Collections.Generic;
using Lean.Common;
#if UNITY_EDITOR
using UnityEditor;

namespace Lean.Localization
{
	[CustomEditor(typeof(LeanToken))]
	public class LeanToken_Inspector : LeanInspector<LeanToken>
	{
		protected override void DrawInspector()
		{
			Draw("value");
		}
	}
}
#endif

namespace Lean.Localization
{
	/// <summary>The class stores a token name (e.g. "AGE"), allowing it to be replaced with the token value (e.g. "20").
	/// To use the token in your text, simply include the token name surrounded by braces (e.g. "I am {AGE} years old!")</summary>
	[ExecuteInEditMode]
	[HelpURL(LeanLocalization.HelpUrlPrefix + "LeanToken")]
	[AddComponentMenu(LeanLocalization.ComponentPathPrefix + "Token")]
	public class LeanToken : LeanSource
	{
		[SerializeField]
		private string value;

		[System.NonSerialized]
		private HashSet<LeanLocalizedBehaviour> behaviours;

		[System.NonSerialized]
		private static HashSet<LeanLocalizedBehaviour> tempBehaviours = new HashSet<LeanLocalizedBehaviour>();

		public string Value
		{
			set
			{
				if (this.value != value)
				{
					this.value = value;

					if (behaviours != null)
					{
						tempBehaviours.Clear();

						tempBehaviours.UnionWith(behaviours);

						foreach (var behaviour in tempBehaviours)
						{
							behaviour.UpdateLocalization();
						}
					}
				}
			}

			get
			{
				return value;
			}
		}

		public void SetValue(float value)
		{
			Value = value.ToString();
		}

		public void SetValue(string value)
		{
			Value = value;
		}

		public void SetValue(int value)
		{
			Value = value.ToString();
		}

		public void Register(LeanLocalizedBehaviour behaviour)
		{
			if (behaviour != null)
			{
				if (behaviours == null)
				{
					behaviours = new HashSet<LeanLocalizedBehaviour>();
				}

				behaviours.Add(behaviour);
			}
		}

		public void Unregister(LeanLocalizedBehaviour behaviour)
		{
			if (behaviours != null)
			{
				behaviours.Remove(behaviour);
			}
		}

		public void UnregisterAll()
		{
			if (behaviours != null)
			{
				foreach (var behaviour in behaviours)
				{
					behaviour.Unregister(this);
				}

				behaviours.Clear();
			}
		}

		public override void Compile(string primaryLanguage, string secondaryLanguage)
		{
			if (string.IsNullOrEmpty(name) == false)
			{
				LeanLocalization.CurrentTokens.Add(name, this);
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			UnregisterAll();
		}
	}
}