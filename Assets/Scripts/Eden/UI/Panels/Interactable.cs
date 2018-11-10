using Dumpster.Core;
using Dumpster.BuiltInModules;
using UnityEngine;
using Eden.Characteristics;

namespace Eden.UI.Panels {

	public class Interactable : Dumpster.BuiltInModules.Panel {

		private Actor _actor {
			get{ return Game.GetModule<Navigation>()?.CurrentArea.LoadedPlayer.GetComponent<Actor>(); }
		}

		[SerializeField] private Transform _visual;

		private void Update () {

			var interactor = _actor.GetCharacteristic<Interactor>( true );
			var playerLogic = _actor.GetCharacteristic<PlayerLogic>( true );

			if ( interactor != null && playerLogic != null ) {
				
				var otherActor = interactor.GetActor( playerLogic.CurrentItemInHand );
				if ( otherActor != null && otherActor.GetCharacteristic<Talkable>() != null ) {

					_visual.position = Camera.main.WorldToScreenPoint( otherActor.transform.position );
					_visual.gameObject.SetActive( true );
				
				} else {

					_visual.gameObject.SetActive( false );
				}
			}
		}
	}
}
