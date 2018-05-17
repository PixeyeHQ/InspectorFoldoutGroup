using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Homebrew
{
	[CustomEditor(typeof(Object), true, isFallback = true)]
	[CanEditMultipleObjects]
	public class EditorOverride : Editor
	{
		private Dictionary<string, Cache> cache = new Dictionary<string, Cache>();
		private List<SerializedProperty> props = new List<SerializedProperty>();
		private SerializedProperty propScript;
		private Type type;
		private int length;
		private FieldInfo[] objectFields;
		private bool initialized;


		void OnEnable()
		{
			Repaint();
			initialized = false;

			objectFields = target.GetType().GetFields();
			length = objectFields.Length;
		}

		private void OnDisable()
		{
			foreach (var cach in cache)
			{
				cach.Value.Dispose();
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (!initialized)
			{
				for (var i = 0; i < length; i++)
				{
					var fold = Attribute.GetCustomAttribute(objectFields[i], typeof(FoldoutAttribute)) as FoldoutAttribute;

					if (fold == null) continue;
					Cache c;
					if (!cache.TryGetValue(fold.name, out c))
					{
						cache.Add(fold.name, new Cache {atr = fold, types = new HashSet<string> {objectFields[i].Name}});
					}
					else
					{
						c.types.Add(objectFields[i].Name);
					}
				}


				var property = serializedObject.GetIterator();
				var next = property.NextVisible(true);
				if (next)
				{
					do
					{
						HandleProp(property);
					} while (property.NextVisible(false));
				}
			}

			initialized = true;

			using (new EditorGUI.DisabledScope("m_Script" == props[0].propertyPath))
			{
				EditorGUILayout.PropertyField(props[0], true);
			}

			EditorGUILayout.Space();

			foreach (var pair in cache)
			{
				var rect = EditorGUILayout.BeginVertical();

				EditorGUILayout.Space();
				EditorGUI.DrawRect(new Rect(rect.x - 1, rect.y - 1, rect.width + 1, rect.height + 1),
					new Color(0.2f, 0.2f, 0.2f, 1f));

				EditorGUI.DrawRect(new Rect(rect.x - 1, rect.y - 1, rect.width + 1, rect.height + 1), new Color(1, 1, 1, 0.1f));


				var v = EditorGUILayout.GetControlRect();

				pair.Value.expanded = EditorGUI.Foldout(new Rect(v.x + 12, v.y - 4, v.width, v.height),
					pair.Value.expanded, new GUIContent(pair.Value.atr.name), EditorStyles.foldout);


				EditorGUILayout.EndVertical();

				rect = EditorGUILayout.BeginVertical();

				EditorGUI.DrawRect(new Rect(rect.x - 1, rect.y - 1, rect.width + 1, rect.height + 1),
					new Color(0.25f, 0.25f, 0.25f, 1f));

				if (pair.Value.expanded)
				{
					EditorGUILayout.Space();
					{
						for (int i = 0; i < pair.Value.props.Count; i++)
						{
							if (pair.Value.props[i].hasChildren)
								EditorGUI.indentLevel++;

							EditorGUILayout.PropertyField(pair.Value.props[i], new GUIContent(pair.Value.props[i].name), true);
							if (i == pair.Value.props.Count - 1)
								EditorGUILayout.Space();
						}
					}
				}

				EditorGUI.indentLevel = 0;
				EditorGUILayout.EndVertical();
			}


			for (var i = 1; i < props.Count; i++)
			{
				EditorGUILayout.PropertyField(props[i], true);
			}


			serializedObject.ApplyModifiedProperties();
			EditorGUILayout.Space();
		}


		public void HandleProp(SerializedProperty prop)
		{
			bool shouldBeFolded = false;

			foreach (var pair in cache)
			{
				if (pair.Value.types.Contains(prop.name))
				{
					shouldBeFolded = true;
					pair.Value.props.Add(prop.Copy());

					break;
				}
			}

			if (shouldBeFolded == false)
			{
				props.Add(prop.Copy());
			}
		}


		public class Cache
		{
			public HashSet<string> types = new HashSet<string>();
			public List<SerializedProperty> props = new List<SerializedProperty>();
			public FoldoutAttribute atr;
			public bool expanded;

			public void Dispose()
			{
				props.Clear();
				types.Clear();
				atr = null;
			}
		}
	}
}