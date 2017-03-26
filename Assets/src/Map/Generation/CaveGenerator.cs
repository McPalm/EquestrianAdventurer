using UnityEngine;
using System.Collections;
using System;

public class CaveGenerator : AbsGenerator
{
	public override void Generate(CompassDirection connections)
	{
		CellularAutomata gen = new CellularAutomata();

		// add two together to get a natural texture on the floor
		gen.Generate(connections);
		map = gen.GetInverted();
		gen.Generate(connections);
		AddResults(gen.GetInverted());



		// stencil with our actual layout
		gen.Generate(0);
		Mask(gen.GetInverted());

		// ensure connectivity
		MinimumPath path = new MinimumPath();
		path.thickness = 1;
		path.Generate(connections);

		UnionResults(path.GetResult());

	}
}
