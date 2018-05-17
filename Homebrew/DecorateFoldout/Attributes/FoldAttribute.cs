/*===============================================================
Product:    ActorBasedBehaviors
Developer:  Dimitry Pixeye - info@pixeye,games
Company:    Homebrew
Date:       5/17/2018 6:31 AM
================================================================*/


using UnityEngine;

namespace Homebrew
{
	public class FoldoutAttribute : PropertyAttribute
	{
		public string name;

		public FoldoutAttribute(string name)
		{
			this.name = name;
		}
	}
}