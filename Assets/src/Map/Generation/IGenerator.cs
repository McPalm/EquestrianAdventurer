using UnityEngine;
using System.Collections;

public interface IGenerator
{
	void Generate(CompassDirection connections);

	int[][] GetResult();
}
