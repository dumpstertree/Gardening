using UnityEngine;

namespace Eden.Interactable {
	
	public class Explode : Dumpster.Events.SmartEvent {
			
		[SerializeField] private GameObject _objectRoot;
		[SerializeField] private HitData _hitData;
		[SerializeField] private Explosion _explosionPrefab;

		public override void EventTriggered () {

			var inst = Instantiate( _explosionPrefab );
			inst.transform.position = _objectRoot.transform.position;
			inst.transform.rotation = _objectRoot.transform.rotation;
			inst.Set( null, _hitData );
		}
	}
}
