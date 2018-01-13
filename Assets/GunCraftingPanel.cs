using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class GunCraftingPanel : UiPanel {

	// *****************************

	public void SetNeedsRecalulatePath () {

		if ( !_recalculatePath ){
			_recalculatePath = true;
			
			Game.Async.WaitForEndOfFrame( () => {
				
				ClearGun();
				foreach ( Gun.Projector p in _baseProjectors ){
					p.Project();
				}

				_recalculatePath = false;
			});
		}
	}


	// *****************************
	
	public void PresentInteractors ( Gun.Component component ) {
		
		_currentCraftingComponent = component;
	}
	public void DismissInteractors ( Gun.Component component ) {
	}

	// *****************************

	public int GetRow ( Vector3 pos ){

		var contentLocalX = _content.InverseTransformPoint( pos ).x - SNAPPING/2;
		var snappedX = Mathf.RoundToInt( (contentLocalX+_content.sizeDelta.x/2) / SNAPPING);
		
		return snappedX;
	}
	public int GetCollumn ( Vector3 pos ){
		
		var contentLocalY = _content.InverseTransformPoint( pos ).y - SNAPPING/2;
		var snappedY = Mathf.RoundToInt( (contentLocalY+_content.sizeDelta.y/2) / SNAPPING);
		
		return snappedY;
	}
	public Slot GetSlot( int collumn, int row ) {

		if ( _slots == null || row > 9 || collumn > 9 || row < 0 || collumn < 0 ){
			return null;
		}

		var slotsHeight = _slots.GetLength(0);
		var slotsWidth = _slots.GetLength(1);
		
		if (_slots != null && collumn < slotsWidth && row < slotsHeight ){
			return _slots[ collumn, row ];
		} else {
			return null;
		}
	}

	// *****************************
	
	[SerializeField] private RectTransform _content;
	[SerializeField] private List<Gun.Projector> _baseProjectors;
	[SerializeField] private List<GunCrafting.Collider> _baseColliders;

	private const float SNAPPING = 100;

	private bool _recalculatePath;
	private List<Gun.Component> _gunComponents;	
	private Gun.Component _currentCraftingComponent;
	
	private Slot[,] _slots;

	// *****************************

	private void ClearGun () {
		
		foreach ( Gun.Component c in GetComponentsInChildren<Gun.Component>() ) {
			c.Reset();
		}
	}

	// *****************************

	private void Awake () {
		
		_gunComponents = new List<Gun.Component>();
		_slots = new Slot[10,10];
		
		for (int y = 0; y < _slots.GetLength(0); y++ ){
			for (int x = 0; x < _slots.GetLength(1); x++ ){
				_slots[ x, y ] = new Slot( x, y ); 
			}
		}



		foreach ( Gun.Component c in GetComponentsInChildren<Gun.Component>() ){
			
			// pointer enter
			c.OnPointerEnterEvent += () => { PresentInteractors( c ); };
			
			// pointer exit
			c.OnPointerExitEvent += () => { DismissInteractors( c ); };
		
			// pointer down
			c.OnPointerDownEvent += () => {
				_currentCraftingComponent.UnSet();
			};

			// pointer held
			c.OnPointerIsStillDownEvent += () => {
				_currentCraftingComponent.Move();
			};

			// pointer up
			c.OnPointerUpEvent += () => {
				if ( _currentCraftingComponent.CanSet ) {
					_currentCraftingComponent.Set(); 
				}
			};
		}
	}
	private void Update(){
		
		if( Input.GetKeyDown( KeyCode.LeftArrow ) ){
			RotateLeft();
		}

		if( Input.GetKeyDown( KeyCode.RightArrow ) ){
			RotateRight();
		}
	}

	// *****************************

	private void RotateLeft () {

		_currentCraftingComponent.UnSet();
		_currentCraftingComponent.transform.Rotate( Vector3.forward, 90f );

		if ( _currentCraftingComponent.CanSet ){
			_currentCraftingComponent.Set();
		}
	}
	private void RotateRight () {

		_currentCraftingComponent.UnSet();
		_currentCraftingComponent.transform.Rotate( Vector3.forward, -90f );

		if ( _currentCraftingComponent.CanSet ){
			_currentCraftingComponent.Set();
		}
	}
	private void OnDrawGizmos () {

		Gizmos.color = Color.red;
		Gizmos.DrawWireCube( _content.transform.position, _content.sizeDelta );

		for (int y = 0; y < 10; y++ ){
		
			for (int x = 0; x < 10; x++ ){
				
				var s = GetSlot( x, y );
				
				if ( s == null ){
					continue;
				}

				if( s.Occupied ) {

					Gizmos.color = Color.white;
					Gizmos.DrawWireCube( new Vector2( x * SNAPPING + SNAPPING/2, y * SNAPPING + SNAPPING/2 ),new Vector3( 25, 25 ) );
				}
				else{

					Gizmos.color = Color.black;
					Gizmos.DrawWireCube( new Vector2( x * SNAPPING + SNAPPING/2, y * SNAPPING + SNAPPING/2 ),new Vector3( 25, 25 ) );
				}
				if( s.Projector ) {

					Gizmos.color = Color.blue;
					Gizmos.DrawWireCube( new Vector2( x * SNAPPING + SNAPPING/2, y * SNAPPING + SNAPPING/2 ),new Vector3( 35, 35 ) );
				}
				if( s.Reciever ) {

					Gizmos.color = Color.red;
					Gizmos.DrawWireCube( new Vector2( x * SNAPPING + SNAPPING/2, y * SNAPPING + SNAPPING/2 ),new Vector3( 35, 35 ) );
				}
			}
		}
	}

	// *****************************

	public class Slot {

		public int X;
		public int Y;

		public bool Occupied;
		public Gun.Component Component;
		public Gun.Projector Projector;
		public Gun.Reciever Reciever; 

		public Slot ( int x, int y ) {
			X = x;
			Y = y;
		}
	}
}


