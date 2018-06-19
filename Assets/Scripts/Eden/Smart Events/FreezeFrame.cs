using UnityEngine;

namespace Eden.Interactable {
	
	public class FreezeFrame : Dumpster.Events.SmartEvent {
		
		[Header( "Settings")]
		[SerializeField] private float _freezeFrameLength;
		
		public override void EventTriggered () {
		
			EdensGarden.Instance.Effects.FreezeFrame( _freezeFrameLength );
		}
	}
}