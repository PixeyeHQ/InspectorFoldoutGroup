[![license]](https://github.com/dimmpixeye/InspectorFoldoutGroup/blob/master/LICENSE)

# InspectorFoldoutGroup
Group variables in Unity 3d inspector with style!

[![https://gyazo.com/9f18eab9dfb123d928aecf7daa3e01da](https://i.gyazo.com/9f18eab9dfb123d928aecf7daa3e01da.gif)](https://gyazo.com/9f18eab9dfb123d928aecf7daa3e01da)


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
