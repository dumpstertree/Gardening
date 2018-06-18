using UnityEngine;
using Eden.Life.Chip;
using Dumpster.Core.Life;

namespace Eden.Life.Brain {
	
	public class BlackBoxBrain : Dumpster.Core.Life.BlackBox<Model2.Life.Visual> {
			
		public Transform ProjectileSpawner {
			get{ return _projectileSpawner; }
		}

		public SightChip SightChip {
			get{ return _sightChip; }
		}
		public InteractorChip Interactor {
			get{ return _interactorChip; }
		}
		public QuickSlotChip QuickslotChip {
			get { return _quickslotChip; }
		}
		public Inventory EquipedItems {
			get{ return _equipedItems; }
		}
		public Dumpster.Physics.Controller Physics {
			get{ return _physics; }
		}


		[Header( "Chips" )]
		[SerializeField] private InteractorChip _interactorChip;
		[SerializeField] private QuickSlotChip  _quickslotChip;
		[SerializeField] private SightChip _sightChip;


		[Header( "Interactables" )]
		[SerializeField] private Interactable.Stats _stats;


		[Header( "Misc" )]
		[SerializeField] private Dumpster.Physics.Controller _physics;
		[SerializeField] private Model.Template.InventoryItemTemplate _equippedItemTemp;


		[Header( "Spawners" )]
		[SerializeField] private Transform _projectileSpawner;


		private Inventory _inventory;
		private Inventory _equipedItems;

		private const int INVENTORY_ITEMS_COUNT = 10;
		private const int EQUIPED_ITEMS_COUNT = 10;


		protected override void Init () {

			base.Init ();

			_stats.OnHealthChanged += HandleOnHealthChanged;

			BuildInventory ();
			BuildEquipedItems ();
		}

		
		// ****************** Handler ****************

		private void BuildInventory () {
			
			_inventory = new Inventory( INVENTORY_ITEMS_COUNT );
		}
		private void BuildEquipedItems () {

			_equipedItems = new Inventory( INVENTORY_ITEMS_COUNT );
			_equipedItems.AddInventoryItem( _equippedItemTemp.GetInstance( 1 ) );
		}


		// ****************** Handler ****************
		
		private void HandleOnHealthChanged( int currentHealth ) {
			
			if ( currentHealth < 0 ) {
				Shutdown ();
			}
		}


		// ****************** Handler ****************

		protected override Eden.Model2.Life.Visual GetVisual () {
			
			return new Model2.Life.Visual();
		}
	}
}