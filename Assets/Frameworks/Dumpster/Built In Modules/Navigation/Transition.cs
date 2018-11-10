using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Dumpster.Core.BuiltInModules.Rooms {
	
	public abstract class Transition : ScriptableObject {

		public abstract void PerformTransition( Game game, Action onComplete );
		public abstract void DismissTransition();
	}
}