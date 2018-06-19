using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core.BuiltInModules.UI;

namespace Eden.UI {

	public abstract class InteractivePanel: Panel {

		public virtual void ReciveInput ( Eden.Input.Package package ) {}
	}
}