using UnityEngine;

namespace Dumpster.Physics {
	
	public abstract class Animator : MonoBehaviour {
		
		// refrences
		[SerializeField] private Controller _physics;
		
		// mono
		private void Update () {
			if ( _physics ) { 
				Animate ( _physics.State ); 
			}
		}
		
		// abstract
		public abstract void Animate ( Controller.Package package  );
	}
}