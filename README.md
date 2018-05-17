# InspectorFoldoutGroup
Group variables in Unity 3d inspector with style!

[![https://gyazo.com/9f18eab9dfb123d928aecf7daa3e01da](https://i.gyazo.com/9f18eab9dfb123d928aecf7daa3e01da.gif)](https://gyazo.com/9f18eab9dfb123d928aecf7daa3e01da)


## How to use 

```csharp
  public class Player : MonoBehaviour
{
	[Foldout("Setup")] public Transform selfTransform;
	
	[Foldout("Data")] public int HP;
	[Foldout("Data")] public int AT;
 
}
```
