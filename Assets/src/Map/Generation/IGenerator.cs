using UnityEngine;
using System.Collections;

public interface IGenerator
{
	void Generate(CompassDirection connections, bool module);

	int[][] GetResult();

	IntVector2 ModuleAnchor { get; }
}
