using Dumpster.BuiltInModules;
using Dumpster.Core;
using UnityEngine;

namespace Eden.Interactable {
	
	public class FreezeFrame : Dumpster.Events.SmartEvent {
		
		[Header( "Settings")]
		[SerializeField] private float _freezeFrameLength;
		
		public override void EventTriggered () {
		
			Game.GetModule<Effects>()?.FreezeFrame( _freezeFrameLength );
		}
	}
}