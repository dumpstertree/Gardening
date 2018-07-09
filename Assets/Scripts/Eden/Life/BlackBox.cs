using UnityEngine;
using Eden.Life.Chip;
using Dumpster.Core.Life;
using System.Collections.Generic;
using Eden.Controller;

namespace Eden.Life {

	public enum Alignment {
		Nuetral,
		Hostile,
		Friendly,
		Player
	}
	
	public class BlackBox : Dumpster.Core.Life.BlackBox<Model.Life.Visual> {
			
		public Transform ProjectileSpawner {
			get{ return _projectileSpawner; }
		}
		public Transform MeleeSpawner {
			get{ return _meleeSpawner; }
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
		public Inventory Inventory {
			get{ return _inventory; }
		}
		public Inventory EquipedItems {
			get{ return _equipedItems; }
		}
		public Dumpster.Physics.Controller Physics {
			get{ return _physics; }
		}
		public List<Collider> Colliders {
			get { return new List<Collider>( GetComponentsInChildren<Collider>() ); } 
		}



		[Header( "Chips" )]
		[SerializeField] private InteractorChip _interactorChip;
		[SerializeField] private QuickSlotChip  _quickslotChip;
		[SerializeField] private SightChip _sightChip;
		[SerializeField] private Interactable.InteractableObject _interactableObject;

		[Header( "Interactables" )]
		[SerializeField] private Interactable.Stats _stats;

		[Header( "Misc" )]
		[SerializeField] private Dumpster.Physics.Controller _physics;

		[SerializeField] private Eden.Templates.Item _equippedItemCenter;
		[SerializeField] private Eden.Templates.Item _equippedItemTop;
		[SerializeField] private Eden.Templates.Item _equippedItemBottom;
		[SerializeField] private Eden.Templates.Item _equippedItemLeft;
		[SerializeField] private Eden.Templates.Item _equippedItemRight;

		[Header( "Spawners" )]
		[SerializeField] private Transform _projectileSpawner;
		[SerializeField] private Transform _meleeSpawner;

		[Header( "Properties" )]
		[SerializeField] private Alignment _alignment;


		private Inventory _inventory;
		private Inventory _equipedItems;

		private const int INVENTORY_ITEMS_COUNT = 15;
		private const int EQUIPED_ITEMS_COUNT = 5;


		protected override void Init () {

			base.Init ();

			BuildInventory ();
			BuildEquipedItems ();

			_stats.OnDeath += Shutdown;

			OnStartup += () => { _interactableObject.Active = true; };
			OnShutDown += () => { _interactableObject.Active = false; };
		}

		
		// ****************** Handler ****************

		private void BuildInventory () {
			
			_inventory = new Inventory( INVENTORY_ITEMS_COUNT );
		}
		private void BuildEquipedItems () {

			_equipedItems = new Inventory( EQUIPED_ITEMS_COUNT );
			
			if( _equippedItemCenter != null ) { _equipedItems.AddInventoryItem( _equippedItemCenter.CreateInstance() ); }
			if( _equippedItemTop != null )    { _equipedItems.AddInventoryItem( _equippedItemTop.CreateInstance() ); }
			if( _equippedItemRight != null )  { _equipedItems.AddInventoryItem( _equippedItemRight.CreateInstance() ); }
			if( _equippedItemBottom != null ) { _equipedItems.AddInventoryItem( _equippedItemBottom.CreateInstance() ); }
			if( _equippedItemLeft != null )   { _equipedItems.AddInventoryItem( _equippedItemLeft.CreateInstance()  ); }
		}


		// ****************** Handler ****************

		protected override Eden.Model.Life.Visual GetVisual () {
			
			return new Model.Life.Visual( _alignment );
		}
	}
}