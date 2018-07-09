using UnityEngine;
using UnityEngine.UI;
using Eden.Life;
using Eden.Controller;
using Eden.Model;

namespace Eden.UI.Panels {
	
	public class InventoryUISub1 : InventoryUI1 {

		[SerializeField] private GridLayoutGroup _layoutGroup;
		[SerializeField] private ButtonActions _buttonActions;
		
		private const int COLLUMNS = 3;
		private const float PADDING = 25;

		private Item _item;
		
		private BlackBox _player {
			get{ 
				return EdensGarden.Instance.Rooms.CurrentArea.LoadedPlayer.GetComponent<Eden.Life.BlackBox>(); 
			}		
		}
		private bool _canCustomize  {
			get{ 
				return ( _item != null && _item.IsShootable ); 
			}
		}


		// ************** Override ****************
		
		public override void ReciveInput( Input.Package package ) {

			if ( package.Face.Right_Down ) {
				Exit ();	
			}

			if ( package.Face.Up_Down && _canCustomize ) {
				Customize ();
			}
		}
		
		protected override Inventory GetInventory () {
			
			return _player.Inventory;
		}
		protected override ItemBubbleUI[] GetItemBubbles () {

			var itemBubbles = new ItemBubbleUI[ _player.Inventory.InventoryCount ];
			for ( int i = 0; i<itemBubbles.Length; i++  ){

				var itemBubble = Instantiate( _itemBubblePrefab );
				itemBubble.transform.SetParent( _layoutGroup.transform, false );
				itemBubble.Index = i;

				itemBubbles[ i ] = itemBubble;

				itemBubble.OnSelect += () => { HandleOnSelectableChanged( itemBubble.Item ); };
			}

			return itemBubbles;
		}
		protected override void OnInit () {

			_layoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			_layoutGroup.constraintCount = COLLUMNS;
			_layoutGroup.cellSize = _itemBubblePrefab.GetComponent<RectTransform>().sizeDelta;
			_layoutGroup.spacing = new Vector2( PADDING, PADDING );
		}


		private void Start () {

			_buttonActions.FaceRightActionText = "Exit";
			_buttonActions.FaceDownActionText = "Move";
		}
		private void Customize () {

			EdensGarden.Instance.UI.Present( 
				EdensGarden.Constants.NewUILayers.Midground,
				EdensGarden.Constants.UIContexts.Building, context => {
					var building = context.GetContext( "Building(Clone)" ).GetComponent<Building>();
					building.SetItemToEdit( _item );
				}
			);
		}
		private void Exit () {
			
			EdensGarden.Instance.UI.Dismiss( 
				EdensGarden.Constants.NewUILayers.Midground,
				EdensGarden.Constants.UIContexts.Inventory 
			);
		}
		private void HandleOnSelectableChanged ( Item item ) {

			_item = item;

			if( _canCustomize ) {
				_buttonActions.FaceUpActionText = "Customize";
			} else {
				_buttonActions.FaceUpActionText = "";
			}
		} 
	}
}