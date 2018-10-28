using UnityEngine;
using Dumpster.Core;
using Eden.Modules;

namespace Eden.Properties {
	
	public class Targetable : MonoBehaviour, IProperty {

		void IProperty.Update () {

			Game.GetModule<Targeting>()?.RegisterTargetableForFrame( this );
		}
	}
}