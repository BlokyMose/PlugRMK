using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TestNameSpace;

namespace ExposedFiles
{
	public class TestComponent_Exposer
	{

		public int a = 10;
		public int b;
		public TestComponent targetComponent;

		public static List<TestComponent_Exposer> GetBindedExposers()
		{
			var exposers = new List<TestComponent_Exposer>();
			var targetComponents = new List<TestComponent>(GameObject.FindObjectsByType<TestComponent>(FindObjectsInactive.Include, FindObjectsSortMode.None));
			foreach (var targetComponent in targetComponents)
				exposers.Add(new() { targetComponent = targetComponent });

			return exposers;
		}
	}
}