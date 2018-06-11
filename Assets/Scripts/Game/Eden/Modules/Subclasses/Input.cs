using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden {
	
	public class Input : Dumpster.Core.BuiltInModules.Input.Controller<Input.Package> {

		protected override Package PollPackage () {
			return new Package ();
		} 	
		protected override Package GetEmptyPackage () {
			return new Package ();
		}

		private void Update () {
			
			PushInputPackage( GetEmptyPackage() );
		}
		public struct Package {
		
		}
	}
}