using UnityEditor;

[CustomEditor(typeof(Tooltip))]
public class EToolTip : MyEditor
{

	public override void OnInspectorGUI()
	{
		(target as Tooltip).hint = TextField("Hint", (target as Tooltip).hint);
	}

}