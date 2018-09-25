using Eden.Life;
using UnityEngine;

namespace Eden.UI.Panels {

	public class Interactable : Dumpster.Core.BuiltInModules.UI.Panel {

		private BlackBox _blackBox {
			get{ return EdensGarden.Instance.Rooms.CurrentArea.LoadedPlayer.GetComponent<BlackBox>(); }
		}

		[SerializeField] private Transform _visual;

		private void Update () {

			var interactable = _blackBox.Visual.InteractingWith;
			if ( interactable != null ) {

				_visual.gameObject.SetActive( true );

				var pos = Camera.main.WorldToScreenPoint( interactable.UIAnchor.transform.position );
				_visual.position = pos;
			
			} else {

				_visual.gameObject.SetActive( false );
			}

		}
	}
}
