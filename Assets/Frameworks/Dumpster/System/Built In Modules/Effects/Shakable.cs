
using UnityEngine;

namespace Dumpster.Core.BuiltInModules.Effects {
	
	public class Shakable : MonoBehaviour {

		public delegate void ShakeEvent ( float magnitude );
		public ShakeEvent OnShake;

		public void Shake( float magnitude ) {

			FireShakeEvent( magnitude );
		}
		private void Update () {

			EdensGarden.Instance.Effects.RegisterShakableForFrame( this );
		}
		private void FireShakeEvent ( float magnitude ) {

			if ( OnShake != null ) {
				OnShake( magnitude );
			}
		}
	}
}