﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GunCraftingPanel : InventoryUI {

	private CraftedGun GetGun () {
		
		List<CraftedGun.Component> components = new List<CraftedGun.Component>();
		foreach( Gun.Component c in _gunComponents ){
			components.Add( new CraftedGun.Component( c ) );
		}

		return new CraftedGun( components );
	}
	private void AddGun ( CraftedGun craftedGun ) {

		foreach( CraftedGun.Component c in craftedGun.GunComponents ) {
			
			var prefab = Resources.Load( c.PrefabName ) as GameObject;
			var instance = Instantiate( prefab );
			
			instance.transform.SetParent( _content, false );
			instance.transform.rotation = Quaternion.Euler( c.Rotation );
			instance.transform.position = c.Position;

			_componentsOnGraph.Add( instance.GetComponent<Gun.Component>() );
		}	

		SetNeedsRecalulatePath();
	}
	private void RemoveGun () {

		foreach ( Gun.Component c in _componentsOnGraph ){
			Destroy( c.gameObject );
		}
		_componentsOnGraph.Clear();

		SetNeedsRecalulatePath();
	}

	// *****************************

	public void SetNeedsRecalulatePath () {

		if ( !_recalculatePath ){
			_recalculatePath = true;
			
			Game.Async.WaitForEndOfFrame( () => {
				
				ClearGun();
				foreach ( Gun.Projector p in _baseProjectors ){
					
					var projectorX = GetX( p.transform.position );
					var projectorY = GetY( p.transform.position );
					Project( projectorX, projectorY );
				}

				_recalculatePath = false;
			});
		}

		CalculateGun();
	}
	public void Project( int x, int y ) {

		foreach ( Gun.Projector projector in _slotGraph[ x, y ].Projectors ) {		

			// if projector exists project forward
			var projectedPos = projector.transform.position + ( projector.transform.up * PROJECTOR_LENGTH );
			var projectedX = GetX( projectedPos );
			var projectedY = GetY( projectedPos );			
			
			foreach ( Gun.Reciever reciever in _slotGraph[ projectedX, projectedY ].Recievers ) {
				
				if ( reciever && projector.Connection == reciever.Connection ) {
					
					// if reciever exists reciever forward
					var recivedPos = reciever.transform.position + (reciever.transform.up * PROJECTOR_LENGTH);
					var recivedX = GetX( recivedPos );
					var recivedY = GetY( recivedPos );

					// if projector and reciever match activate the component
					if ( recivedX == x && recivedY == y ) {

						var component = _slotGraph[ projectedX, projectedY ].Component;
						component.Recieve();
						AddToGun( component );
					}
				}
			}
		}
	}
	
	// *****************************

	public void SetComponent( int x, int y, Gun.Component component ) {
		
		_slotGraph[ x, y ].Component = component;
	}
	public void AddProjector( Gun.Projector projector ) {
		
		var x = GetX( projector.transform.position );
		var y = GetY( projector.transform.position );
		
		_slotGraph[ x, y ].Projectors.Add( projector );
	}
	public void AddReciever( Gun.Reciever reciever ) {
		
		var x = GetX( reciever.transform.position );
		var y = GetY( reciever.transform.position );
		
		_slotGraph[ x, y ].Recievers.Add( reciever );
	}
	public void RemoveProjector( Gun.Projector projector ) {
		
		var x = GetX( projector.transform.position );
		var y = GetY( projector.transform.position );
		
		if ( _slotGraph[ x, y ].Projectors.Contains( projector ) ) {
			_slotGraph[ x, y ].Projectors.Remove( projector ); 
		}
	}
	public void RemoveReciever( Gun.Reciever reciever ) {
		
		var x = GetX( reciever.transform.position );
		var y = GetY( reciever.transform.position );

		if ( _slotGraph[ x, y ].Recievers.Contains( reciever ) ) {
			_slotGraph[ x, y ].Recievers.Remove( reciever ); 
		}
	}
	
	// *****************************

	public int GetX ( Vector3 pos ){

		var contentLocalX = _content.InverseTransformPoint( pos ).x - SNAPPING/2;
		var snappedX = Mathf.RoundToInt( (contentLocalX+_content.sizeDelta.x/2) / SNAPPING);
		
		return snappedX;
	}
	public int GetY ( Vector3 pos ){
		
		var contentLocalY = _content.InverseTransformPoint( pos ).y - SNAPPING/2;
		var snappedY = Mathf.RoundToInt( (contentLocalY+_content.sizeDelta.y/2) / SNAPPING);
		
		return snappedY;
	}
	public bool GetAvailable ( int x, int y ) {
		
		var slot = _slotGraph[ x, y ];

		if ( slot.Component == null && 
			 slot.Projectors.Count == 0 && 
			 slot.Recievers.Count == 0 ) {

			return true;
		}

		return false;
	}

	// *****************************
	
	[SerializeField] private InventoryItem _startItem;
	[SerializeField] private RectTransform _content;
	[SerializeField] private List<Gun.Projector> _baseProjectors;
	[SerializeField] private List<Gun.Collider> _baseColliders;

	private const float SNAPPING = 100;	
	private const int NUM_OF_SLOTS = 10;
	private const int PROJECTOR_LENGTH = 100;

	private bool _recalculatePath;
	private List<Gun.Component> _gunComponents;	
	private List<Gun.Component> _componentsOnGraph;
	private Slot[,] _slotGraph;

	// *****************************

	private void Awake () {
		
		// init
		_gunComponents = new List<Gun.Component>();
		_componentsOnGraph = new List<Gun.Component>();

		// Add a starting item
		_gunCraftingInventory = new Inventory( 1 );
		_gunCraftingInventory.SetInventoryItem( 0, Instantiate( _startItem ) );
		_itemBubble.Index = 0;

		foreach ( Gun.Component c in GetComponentsInChildren<Gun.Component>() ){
			_componentsOnGraph.Add( c );
		}

		//**************

		_gunCraftingInventory.OnInventoryItemChanged += (index, item) => {
			RemoveGun();
			if ( item != null && item._shootData.CraftedGun != null ) {
				AddGun( item._shootData.CraftedGun );
			}
		};
		
		// set items in graph
		_slotGraph = new Slot[ NUM_OF_SLOTS, NUM_OF_SLOTS];
		for (int y = 0; y < _slotGraph.GetLength( 0 ); y++ ) {
			for (int x = 0; x < _slotGraph.GetLength( 1 ); x++ ) {
				_slotGraph[ x, y ] = new Slot(); 
			}
		}

		
		foreach( Gun.Projector p in _baseProjectors ) {
			AddProjector( p );
		}

		_saveButton.onClick.AddListener( () => {
			_gunCraftingInventory.GetInventoryItem( _itemBubble.Index )._shootData.CraftedGun = GetGun(); 
		});
	}

	// *****************************

	private void AddToGun ( Gun.Component component ) {

		if ( !_gunComponents.Contains( component ) ) {
			_gunComponents.Add( component );
		}
	}
	private void ClearGun () {
		
		foreach ( Gun.Component c in _gunComponents ) {
			c.Reset();
		}
		_gunComponents.Clear();
	}
	private void CalculateGun () {
	}

	// *****************************

	[SerializeField] private ItemBubbleUI _itemBubble;
	[SerializeField] private Button _saveButton;
	private Inventory _gunCraftingInventory;

	protected override Inventory GetInventory () {
		return _gunCraftingInventory;
	}
	protected override ItemBubbleUI[] GetItemBubbles () {
		return new ItemBubbleUI[]{ _itemBubble };
	}

	public class Slot {

		public Gun.Component Component;
		public List<Gun.Projector> Projectors;
		public List<Gun.Reciever> Recievers; 

		public Slot () {

			Projectors = new List<Gun.Projector>();
			Recievers = new List<Gun.Reciever>(); 
		}
	}
}



