using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Model.Constants {
	
	[System.Serializable]
	public class UILayers {

		public int Foreground {
			get{ return _foreground; }
		}
		public int Midground  {
			get{ return _midground; }
		}
		public int Background {
			get{ return _background; }
		}

		[SerializeField] private int _foreground = 2;
		[SerializeField] private int _midground  = 1;
		[SerializeField] private int _background = 0;
	}
}
