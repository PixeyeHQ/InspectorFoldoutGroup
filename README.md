[![license](https://img.shields.io/badge/license-MIT-brightgreen.svg)](https://github.com/dimmpixeye/InspectorFoldoutGroup/blob/master/LICENSE)

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
