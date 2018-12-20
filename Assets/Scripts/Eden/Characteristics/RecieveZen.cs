using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;

namespace Eden.Characteristics {

	public class RecieveZen : Dumpster.Core.Characteristic {

		public void Recieve ( int zen ) {
			
			Game.GetModule<Eden.Modules.Zen>().AddZen( zen );
		}
	}
}