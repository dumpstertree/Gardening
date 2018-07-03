using UnityEngine;

namespace Eden {

	public class PlayerUIController : MonoBehaviour {

		[SerializeField] private Eden.Life.BlackBoxes.Player _player;
		
		private void Awake () {

			_player.OnRecieveInput += RecieveInput;
		}
		private void RecieveInput ( Eden.Input.Package package ) {

			if ( package.Face.Up_Down ) {
				OpenInventory ();
			}
		}
		private void OpenInventory () {

			EdensGarden.Instance.UI.Present( 
				EdensGarden.Constants.NewUILayers.Midground, 
				EdensGarden.Constants.UIContexts.Inventory 
			);
		}
	}
}