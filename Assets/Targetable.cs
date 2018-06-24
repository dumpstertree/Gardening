using UnityEngine;

namespace Eden.Properties {
	
	public class Targetable : MonoBehaviour {

		public void Update () {

			EdensGarden.Instance.Targeting.RegisterTargetableForFrame( this );
		}
	}
}