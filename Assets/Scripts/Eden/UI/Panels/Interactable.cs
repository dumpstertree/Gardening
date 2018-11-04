using Dumpster.Core;
using Dumpster.BuiltInModules;
using Eden.Life;
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
				
				var interactable = interactor.GetActor( playerLogic.CurrentItemInHand );
				if ( interactable != null ) {

					_visual.gameObject.SetActive( true );

					var pos = Camera.main.WorldToScreenPoint( interactable.transform.position );
					_visual.position = pos;
				
				} else {

					_visual.gameObject.SetActive( false );
				}
			}
		}
	}
}
