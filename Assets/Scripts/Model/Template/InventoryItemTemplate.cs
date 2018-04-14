using UnityEngine;

namespace Model.Template {
	
	public class InventoryItemTemplate : ScriptableObject {

		public InventoryItem GetInstance ( int startingCount ) {

			var item = new InventoryItem (
				_id, 
				startingCount, 
				_displayName, 
				_maxCount, 
				_sprite, 
				_holdItem, 
				_expendable,
				
				// interact data
				_canInteract,
				_interactor.GetController(),
				
				// shoot data
				_canShoot,
				_shootData.GetController()
			);

			return item;
		}

		private const string NULL_TEMPLATE_ID = "null.null";
		public static InventoryItemTemplate GetTemplate( string forID ) {
			
			var templates = Resources.LoadAll<InventoryItemTemplate>( "" );
			InventoryItemTemplate nullReturn = null; 
			
			foreach ( InventoryItemTemplate t in templates ) {
				
				if ( t._id == forID ) { return t; }
				if ( t._id == NULL_TEMPLATE_ID ){ nullReturn = t; }
			}

			return nullReturn;
		}

		// *********************************


		[Header( "Storage" )]
		[SerializeField] private string _id = "example.id";


		[Header( "Name" )]
		[SerializeField] private string _displayName = "example name";


		[Header( "Visuals" )]
		[SerializeField] private Sprite _sprite = null;
		[SerializeField] private GameObject _holdItem = null;


		[Header( "Count" )]
		[SerializeField] private int _maxCount = 1;
		[SerializeField] private bool _expendable = true;


		[Header( "Usability" )]
		[SerializeField] private bool _canInteract = true;
		[SerializeField] private Model.Template.InteractData _interactor = null;
		
		[SpaceAttribute]
		[SerializeField] private bool _canShoot = false;
		[SerializeField] private Model.Template.ShootData _shootData = null;
	}
}
