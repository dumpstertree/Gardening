using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Characteristics {

	public class Alignment : Dumpster.Core.Characteristic {
		
		public enum Type {
			Player, Ally, Enemy, Nuetral
		}
		public Type MyAlignment {
			get { return _myAlignment; }
		}


		[SerializeField] private Type _myAlignment;
	}
}