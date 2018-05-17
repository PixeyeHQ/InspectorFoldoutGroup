/*===============================================================
Product:    ActorBasedBehaviors
Developer:  Dimitry Pixeye - info@pixeye,games
Company:    Homebrew
Date:       5/17/2018 6:43 AM
================================================================*/


using System;
using System.Linq;
using UnityEditor;

namespace Homebrew
{
	[InitializeOnLoad]
	public class ProcessingEditor
	{
		public static Type[] types = new Type[0];

		static ProcessingEditor()
		{
			GetTypes();
		}

		static void GetTypes()
		{
			types = AppDomain.CurrentDomain.GetAssemblies().Where(a =>
					a.FullName == "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null")
				.SelectMany(t => t.GetTypes())
				.Where(t => t.IsClass).ToArray();
		}
	}
}