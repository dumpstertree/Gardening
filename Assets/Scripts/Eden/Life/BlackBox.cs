﻿using UnityEngine;
using Eden.Life.Chip;
using Dumpster.Core.Life;

namespace Eden.Life {
	
	public class BlackBox : Dumpster.Core.Life.BlackBox<Model2.Life.Visual> {
			
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


		[Header( "Chips" )]
		[SerializeField] private InteractorChip _interactorChip;
		[SerializeField] private QuickSlotChip  _quickslotChip;
		[SerializeField] private SightChip _sightChip;


		[Header( "Interactables" )]
		[SerializeField] private Interactable.Stats _stats;


		[Header( "Misc" )]
		[SerializeField] private Dumpster.Physics.Controller _physics;

		[SerializeField] private Model.Template.InventoryItemTemplate _equippedItemCenter;
		[SerializeField] private Model.Template.InventoryItemTemplate _equippedItemTop;
		[SerializeField] private Model.Template.InventoryItemTemplate _equippedItemBottom;
		[SerializeField] private Model.Template.InventoryItemTemplate _equippedItemLeft;
		[SerializeField] private Model.Template.InventoryItemTemplate _equippedItemRight;


		[Header( "Spawners" )]
		[SerializeField] private Transform _projectileSpawner;
		[SerializeField] private Transform _meleeSpawner;


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
			
			if( _equippedItemCenter != null ) { _equipedItems.AddInventoryItem( _equippedItemCenter.GetInstance( 0 ) ); }
			if( _equippedItemTop != null ) { _equipedItems.AddInventoryItem( _equippedItemTop.GetInstance( 1 ) ); }
			if( _equippedItemRight != null ) { _equipedItems.AddInventoryItem( _equippedItemRight.GetInstance( 2 ) ); }
			if( _equippedItemBottom != null ) { _equipedItems.AddInventoryItem( _equippedItemBottom.GetInstance( 3 ) ); }
			if( _equippedItemLeft != null ) { _equipedItems.AddInventoryItem( _equippedItemLeft.GetInstance( 4 ) ); }
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