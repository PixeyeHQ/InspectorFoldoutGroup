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
 
		private SerializedProperty propScript;
		private Type type;
		private int length;
		private List<FieldInfo> objectFields;
		private bool initialized;
		private Colors colors;
		private FoldoutAttribute prevFold;
		private GUIStyle style;
		private List<Order> orders = new List<Order>();
		private int index = -1;

		private void Awake()
		{
			var uiTex_in = Resources.Load<Texture2D>("IN foldout focus-6510");
			var uiTex_in_on = Resources.Load<Texture2D>("IN foldout focus on-5718");


			var c_on = Color.white;

			style = new GUIStyle(EditorStyles.foldout);

			style.overflow = new RectOffset(-10, 0, 3, 0);
			style.padding = new RectOffset(25, 0, -3, 0);

			style.active.textColor = c_on;
			style.active.background = uiTex_in;
			style.onActive.textColor = c_on;
			style.onActive.background = uiTex_in_on;

			style.focused.textColor = c_on;
			style.focused.background = uiTex_in;
			style.onFocused.textColor = c_on;
			style.onFocused.background = uiTex_in_on;
		}

		void OnEnable()
		{
			index = -1;
			bool pro = EditorGUIUtility.isProSkin;
			if (!pro)
			{
				colors = new Colors();
				colors.col0 = new Color(0.2f, 0.2f, 0.2f, 1f);
				colors.col1 = new Color(1, 1, 1, 0.55f);
				colors.col2 = new Color(0.7f, 0.7f, 0.7f, 1f);
			}
			else
			{
				colors = new Colors();
				colors.col0 = new Color(0.2f, 0.2f, 0.2f, 1f);
				colors.col1 = new Color(1, 1, 1, 0.1f);
				colors.col2 = new Color(0.25f, 0.25f, 0.25f, 1f);
			}

			var t = target.GetType();
			var typeTree = t.GetTypeTree();
			objectFields = target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic |
			                                          BindingFlags.Instance)
				.OrderByDescending(x => typeTree.IndexOf(x.DeclaringType)).ToList();


			length = objectFields.Count;


			Repaint();
			initialized = false;
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

					Cache c;
					if (fold == null)
					{
						if (prevFold != null && prevFold.foldEverything)
						{
							if (!cache.TryGetValue(prevFold.name, out c))
							{
								cache.Add(prevFold.name, new Cache {atr = prevFold, types = new HashSet<string> {objectFields[i].Name}});
							}
							else
							{
								c.types.Add(objectFields[i].Name);
							}
						}

						continue;
					}

					prevFold = fold;
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
						index++;
						HandleProp(property, index);
					} while (property.NextVisible(false));
				}
			}


			if (orders.Count == 0)
			{
				DrawDefaultInspector();
				return;
			}

			initialized = true;

			using (new EditorGUI.DisabledScope("m_Script" == orders[0].prop.propertyPath))
			{
				EditorGUILayout.PropertyField(orders[0].prop, true);
			}

			EditorGUILayout.Space();

 
			for (int i = 1; i < orders.Count; i++)
			{
				var order = orders[i];

				if (order.prop != null)
				{
					EditorGUILayout.PropertyField(order.prop, true);
				}
				else
					RenderCache(cache[order.cacheID]);
			}

 
			serializedObject.ApplyModifiedProperties();
			EditorGUILayout.Space();
		}

		void RenderCache(Cache c)
		{
			var rect = EditorGUILayout.BeginVertical();

			EditorGUILayout.Space();

			EditorGUI.DrawRect(new Rect(rect.x - 1, rect.y - 1, rect.width + 1, rect.height + 1),
				colors.col0);

			EditorGUI.DrawRect(new Rect(rect.x - 1, rect.y - 1, rect.width + 1, rect.height + 1), colors.col1);


			c.expanded = EditorGUILayout.Foldout(c.expanded, c.atr.name, true,
				style != null ? style : EditorStyles.foldout);


			EditorGUILayout.EndVertical();

			rect = EditorGUILayout.BeginVertical();

			EditorGUI.DrawRect(new Rect(rect.x - 1, rect.y - 1, rect.width + 1, rect.height + 1),
				colors.col2);

			if (c.expanded)
			{
				EditorGUILayout.Space();
				{
					for (int i = 0; i < c.props.Count; i++)
					{
						EditorGUI.indentLevel = 1;

						EditorGUILayout.PropertyField(c.props[i],
							new GUIContent(c.props[i].name.FirstLetterToUpperCase()), true);
						if (i == c.props.Count - 1)
							EditorGUILayout.Space();
					}
				}
			}

			EditorGUI.indentLevel = 0;
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();
		}


		public void HandleProp(SerializedProperty prop, int index)
		{
	 
			var order = new Order();
			order.index = index;
			order.prop = null;
			order.cacheID = string.Empty;

			foreach (var pair in cache)
			{
				if (pair.Value.types.Contains(prop.name))
				{
			 
					pair.Value.props.Add(prop.Copy());
					var _o = orders.Find(o => o.cacheID == pair.Key);
					if (_o==null)
					{
						order.cacheID = pair.Key;
						orders.Add(order);
					}
	      
					return;
				}
			}

		 
	 
				order.prop = prop.Copy();
				orders.Add(order);
			 

	 
		}


		private struct Colors
		{
			public Color col0;
			public Color col1;
			public Color col2;
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

		public class Order
		{
			public int index;
			public string cacheID;
			public SerializedProperty prop;
		}
	}


	public static partial class FrameworkExtensions
	{
		public static string FirstLetterToUpperCase(this string s)
		{
			if (string.IsNullOrEmpty(s))
				return string.Empty;

			char[] a = s.ToCharArray();
			a[0] = char.ToUpper(a[0]);
			return new string(a);
		}

		public static IList<Type> GetTypeTree(this Type t)
		{
			var types = new List<Type>();
			while (t.BaseType != null)
			{
				types.Add(t);
				t = t.BaseType;
			}

			return types;
		}
	}
}
