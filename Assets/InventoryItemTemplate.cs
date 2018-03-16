using UnityEngine;

public class InventoryItemTemplate : ScriptableObject {

	public string ID {
		get { return _id; }
	}
	public void GetInstance () {

		

	}

	private const string NULL_TEMPLATE_ID = "null.null";
	public static InventoryItemTemplate Get( string forID ) {
		
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
	[SerializeField] private InteractData _interactData = null;
	
	[SpaceAttribute]
	[SerializeField] private bool _canShoot = false;
	[SerializeField] private ShootData _shootData = null;
	
	[SpaceAttribute]
	[SerializeField] private bool _canSwing = false;
	[SerializeField] private HitData _hitData = null;
	
	[SpaceAttribute]
	[SerializeField] private bool _canPlace = false;
	[SerializeField] private PlaceData _placeData = null;

	[SpaceAttribute]
	[SerializeField] private bool _canPlant = false;
	[SerializeField] private PlantData _plantData = null;
}
