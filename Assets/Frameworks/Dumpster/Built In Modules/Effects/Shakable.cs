using Dumpster.Core;
using UnityEngine;

namespace Dumpster.BuiltInModules {
	
	public class Shakable : MonoBehaviour {

		public delegate void ShakeEvent ( float magnitude );
		public ShakeEvent OnShake;

		public void Shake( float magnitude ) {

			FireShakeEvent( magnitude );
		}
		private void Update () {

			Game.GetModule<Effects>()?.RegisterShakableForFrame( this );
		}
		private void FireShakeEvent ( float magnitude ) {

			if ( OnShake != null ) {
				OnShake( magnitude );
			}
		}
	}
}