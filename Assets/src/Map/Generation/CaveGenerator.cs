using UnityEngine;
using System.Collections;
using System;

public class CaveGenerator : AbsGenerator
{
	IntVector2 moduleAnchor;


	public override IntVector2 ModuleAnchor
	{
		get
		{
			return moduleAnchor;
		}
	}

	public override void Generate(CompassDirection connections, bool module)
	{
		CellularAutomata gen = new CellularAutomata();

		// add two together to get a natural texture on the floor
		gen.Generate(connections, false);
		map = gen.GetInverted();
		gen.Generate(connections, false);
		AddResults(gen.GetInverted());



		// stencil with our actual layout
		gen.Generate(0, false);
		Mask(gen.GetInverted());

		// ensure connectivity
		MinimumPath path = new MinimumPath();
		path.thickness = 1;
		path.Generate(connections, module);
		moduleAnchor = path.ModuleAnchor;

		ScatterSingleForbidSorround(3, 0, 0.01f, 1, 2, 0.65f); // scatter a bunch of pillars sorrounded with mushrooms
		ScatterSingle(3, 1, 0.01f, 1); // scatter a bunch of pillars in open areas

		UnionResults(path.GetResult());

	}

	


}
