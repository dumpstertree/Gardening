using UnityEngine;
using Eden.Modules;

namespace Eden.Properties {
	
	public class Targetable : MonoBehaviour, IProperty {

		void IProperty.Update () {

			EdensGarden.Instance.Targeting.RegisterTargetableForFrame( this );
		}
	}
}