[![license](https://img.shields.io/badge/license-MIT-brightgreen.svg?style=flat-square)](https://github.com/dimmpixeye/InspectorFoldoutGroup/blob/master/LICENSE)
[![Join the chat at https://discord.gg/ukhzx83](https://img.shields.io/badge/discord-join%20channel-brightgreen.svg?style=flat-square)](https://discord.gg/ukhzx83)
# InspectorFoldoutGroup
Group variables in Unity 3d inspector with style!

[![https://gyazo.com/9f18eab9dfb123d928aecf7daa3e01da](https://i.gyazo.com/9f18eab9dfb123d928aecf7daa3e01da.gif)](https://gyazo.com/9f18eab9dfb123d928aecf7daa3e01da)

[![https://gyazo.com/4e6566c2fca783e4cd557f95713a4ab5](https://i.gyazo.com/4e6566c2fca783e4cd557f95713a4ab5.gif)](https://gyazo.com/4e6566c2fca783e4cd557f95713a4ab5)

## How to use 

Put attribute before variable and you are done ! 

```csharp
[Foldout("DESIRED_NAME")]
```

```csharp
  public class Player : MonoBehaviour
{
	[Foldout("Setup")] public Transform selfTransform;
	
	[Foldout("Data")] public int HP;
	[Foldout("Data")] public int AT;
 
}
```

You don't need to write [Foldout] attribute each time! Instead, you can add "true" bool check afther naming to put all properties to the specified group
```csharp
  public class Player : MonoBehaviour
{
	[Foldout("DATA OBJECT", true)] 
	public int hp;
	public int attack = 20;
	[SerializeField]
	private GameObject self;
	
	[Foldout("DATA ATTACK")] 
	public int AT;
 
}
```
[![https://gyazo.com/2ce500e63fd604de8098aece2fa354fa](https://i.gyazo.com/2ce500e63fd604de8098aece2fa354fa.png)](https://gyazo.com/2ce500e63fd604de8098aece2fa354fa)

 
## Other content
* [Tag filters](https://github.com/dimmpixeye/Unity3d-Tags-Filters) -  an extension to add foldable groups to the inspector.
* [ACTORS](https://github.com/dimmpixeye/Actors-Unity3d-Framework) - Unity3d data-driven framework I'm currently working on.

