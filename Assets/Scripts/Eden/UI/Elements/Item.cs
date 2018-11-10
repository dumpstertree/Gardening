using UnityEngine;
using UnityEngine.UI;

namespace Eden.UI.Elements {

	public class Item : MonoBehaviour {

		// ******************* Public *******************

		public Eden.Model.Item BackingItem {
			get; private set;
		}
		
		public void SetBackingItem ( Eden.Model.Item item ) {

			BackingItem = item;
			ReloadIcon( item );
			ReloadCount( item );
		}

		
		// ******************* Private *******************


		[SerializeField] private Image _icon;
		[SerializeField] private Text _count;
		[SerializeField] private GameObject _countContainer;

		private void ReloadIcon ( Eden.Model.Item item ) {

			if ( item == null ) {
			
				_icon.sprite = null;
			
			} else {

				_icon.sprite = item.Sprite;
			}
		}
		private void ReloadCount ( Eden.Model.Item item ) {

			if ( item == null || item.Count > 1 ) {

				_countContainer.SetActive( false );

			} else {

				_countContainer.SetActive( true );
				_count.text = item.Count + "x";
			}
		}
	}
}