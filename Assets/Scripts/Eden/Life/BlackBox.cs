using Eden.Controller;
using Eden.Interactable;
using Eden.Model;
using Eden.Model.Life;
using UnityEngine;

namespace Eden.Life {

	public enum Alignment {
		Nuetral,
		Hostile,
		Friendly,
		Player
	}

	
	[RequireComponent( typeof( InteractableObject ) ) ]
	[RequireComponent( typeof( PropertiesObject ) ) ]
	public class BlackBox : Dumpster.Core.Life.BlackBox<Model.Life.Visual> {
		

		// ***************************** Public ************************************

			
		public Transform MeleeSpawner {
			get{ return _meleeSpawner; }
		}
		public Inventory Inventory {
			get{ return _inventory; }
		}
		public Inventory EquipedItems {
			get{ return _equipedItems; }
		}
		public Item PrimaryEquipedItem{
			get{ return _primaryItem; }
		}

		// ***************************** Private ************************************


		[Header( "ICanUseMeleeWeapon" )]
		[SerializeField] private Transform _meleeSpawner;


		[Header( "Interactables" )]
		[SerializeField] private Stats _stats;


		[Header( "Properties" )]
		[SerializeField] private Alignment _alignment;


		[Header( "Equipment" )]
		[SerializeField] private int InventoryItemCount = 15;
		[SerializeField] private int EquipedItemsCount = 5;
		[SerializeField] private Eden.Templates.Item _primaryEquipedItem;
		[SerializeField] private Eden.Templates.Item [] _equipedStartingItems;


		private Item _primaryItem;
		private Inventory _inventory;
		private Inventory _equipedItems;		
		private InteractableObject _interactableObject;
		private PropertiesObject _propertiesObject;



		protected override void Init () {


			// bas init
			base.Init ();


			// get dependencies
			_interactableObject = GetComponent<InteractableObject>();
			_propertiesObject   = GetComponent<PropertiesObject>();

			
			//  setup items
			_primaryItem = _primaryEquipedItem.CreateInstance();

			BuildInventory ();
			BuildEquipedItems ();


			// listen for life events
			_stats.OnDeath += Shutdown;

			OnStartup += () => { 
				_interactableObject.Active = true;
				_propertiesObject.Active = true;
			};

			OnShutDown += () => {
				_interactableObject.Active = false;
				_propertiesObject.Active = false;
			};


			// listen for get visual event
			OnGetVisual += GetVisual;
		}
		protected override Visual GetVisualInstance () {

			return new Visual ();
		}

		
		private void BuildInventory () {
			
			_inventory = new Inventory( InventoryItemCount );
		}
		private void BuildEquipedItems () {

			_equipedItems = new Inventory( EquipedItemsCount );

			for ( int i=0; i<_equipedStartingItems.Length; i++ ) {
				
				if ( i + 1 > EquipedItemsCount ) {
					break;
				}

				var item = _equipedStartingItems[ i ];
				_equipedItems.AddInventoryItem( item.CreateInstance() );
			}
		}
		private void GetVisual ( Visual visual ) {

			visual.Alignment = _alignment;
			visual.MaxHealth = _stats.MaxHealth;
			visual.CurrentHealth = _stats.CurrentHealth;
		}
	}
}