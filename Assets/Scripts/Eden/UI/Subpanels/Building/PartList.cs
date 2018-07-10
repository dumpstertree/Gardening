using System.Collections.Generic;
using UnityEngine;
using Eden.UI.Elements.Building;

namespace Eden.UI.Subpanels.Building {

	public class PartList : BuildSubpanel {

		
		//******************* Public ***************************

		public delegate void ShiftEvent( int index );
		public ShiftEvent OnShift;

		public delegate void PartClickedEvent ( int index );
		public PartClickedEvent OnPartClicked;

		public delegate void TriedToBreakFreeEvent ();
		public TriedToBreakFreeEvent OnTriedToBreakFreeUp;
		public TriedToBreakFreeEvent OnTriedToBreakFreeDown;
		public TriedToBreakFreeEvent OnTriedToBreakFreeLeft;
		public TriedToBreakFreeEvent OnTriedToBreakFreeRight;

		public override void ReciveInput( Input.Package package ) {

			if ( !Enabled )  {
				return;
			}

			if ( package.Dpad.Up_Down ) {
				ShiftUp();
			}
			if ( package.Dpad.Down_Down ) {
				ShiftDown();
			}
			if ( package.Dpad.Left_Down ) {
				ShiftLeft();
			}
			if ( package.Dpad.Right_Down ) {
				ShiftRight();
			}
			if ( package.Face.Down_Down ) {
				FirePartClickedEvent( _index );	
			}
		}

		public Eden.Model.Building.Parts.Gun TakePart ( int atIndex ) {

			if ( _parts.Count > atIndex - 1 ) {
				
				var part = _parts[ atIndex ];
				_parts.RemoveAt( atIndex );
				_player.Inventory.SetInventoryItem( atIndex, null );
				RemovePartVisual( atIndex );
				__index = 0;
				
				return part;
			}

			return null;
		}
		public Eden.Model.Building.Parts.Gun PeakAtPart ( int atIndex  ) {
			
			return  ( _parts.Count > atIndex - 1 ) ? _parts[ atIndex ] : null;
		}

		
		//******************* Protected ***************************
		
		protected override void Enable () {
		
			_partVisuals[ _index ].IsSelected = true;
			FireShiftEvent( _index );
		}
		protected override void Disable () {
		
			foreach ( PartCell p in _partVisuals ) {
				p.IsSelected = false;
			}
		}


		//*************≠****** Private ***************************

		[SerializeField] private PartCell _partVisualPrefab;
		[SerializeField] private Transform _content;

		private List<PartCell> _partVisuals;
		private List<Eden.Model.Building.Parts.Gun> _parts;
		private int __index;
		
		private Eden.Life.BlackBox _player {
 			get{ 
 				return EdensGarden.Instance.Rooms.CurrentArea.LoadedPlayer.GetComponent<Eden.Life.BlackBox>(); 
 			}
 		}

		private int _index {
			get{ return __index; }
			set { 
				if ( _partVisuals[ __index ] != null ) { _partVisuals[ __index ].IsSelected = false; }
				__index = value;
				if ( _partVisuals[ __index ] != null ) { _partVisuals[ __index ].IsSelected = true; }
			}
		}
		private bool _canShiftUp {
			get{ 
				return _index > 0;
			}
		}
		private bool _canShiftDown {
			get{ 
				return _index < _partVisuals.Count - 1;
			}
		}
		private bool _canShiftLeft {
			get{ 
				return false;
			}
		}
		private bool _canShiftRight {
			get{ 
				return false;
			}
		}

		private void Awake () {

			_parts = new List<Eden.Model.Building.Parts.Gun>();
			_partVisuals = new List<PartCell>();
			
			for( int i =0; i<_player.Inventory.InventoryCount-1; i++ ) {
				
				var item = _player.Inventory.GetInventoryItem( i );
				if ( item != null ) {
					if ( item.IsGunBuildable ){
						_parts.Add( item.AsGunBuildable.Part );
					}
				}
			}

			BuildPartVisuals ();
		}
		private void ShiftUp () {

			if ( _canShiftUp ) {
				_index--;
				FireShiftEvent( _index );
			} else {
				FireTriedToBreakFreeUp ();
			}
		}
		private void ShiftDown () {

			if ( _canShiftDown ) {
				_index++;
				FireShiftEvent( _index );		
			} else {
				FireTriedToBreakFreeDown ();
			}
		}
		private void ShiftLeft () {

			if ( _canShiftLeft ) {
				FireShiftEvent( _index );
			} else {
				FireTriedToBreakFreeLeft ();
			}
		}
		private void ShiftRight () {

			if ( _canShiftRight ) {
				FireShiftEvent( _index );
			} else {
				FireTriedToBreakFreeRight ();
			}
		}	

		private void BuildPartVisuals () {

			foreach ( Eden.Model.Building.Parts.Gun p in _parts ) {
				
				var inst = Instantiate( _partVisualPrefab );
				
				inst.transform.SetParent( _content );
				inst.SetPart( p );
				inst.Button.onClick.AddListener( () => { 
					FirePartClickedEvent( _parts.IndexOf( p ) ); 
				});

				_partVisuals.Add( inst );
			}
		}
		private void RemovePartVisual ( int atIndex ) {

			_partVisuals[ atIndex ].Remove();
			_partVisuals.RemoveAt( atIndex );
		}
		private void AddPartVisual ( int atIndex ) {
		}

		
		//******************* Fire Events ***************************

		private void FirePartClickedEvent ( int index ) {

			if ( OnPartClicked != null ) {
				OnPartClicked( index );
			}
		}
		private void FireTriedToBreakFreeUp () {

			if ( OnTriedToBreakFreeUp != null ) {
				OnTriedToBreakFreeUp ();
			}
		}
		private void FireTriedToBreakFreeDown () {

			if ( OnTriedToBreakFreeDown != null ) {
				OnTriedToBreakFreeDown ();
			}
		}
		private void FireTriedToBreakFreeLeft () {

			if ( OnTriedToBreakFreeLeft != null ) {
				OnTriedToBreakFreeLeft ();
			}
		}
		private void FireTriedToBreakFreeRight () {

			if ( OnTriedToBreakFreeRight != null ) {
				OnTriedToBreakFreeRight ();
			}
		}
		private void FireShiftEvent ( int index ) {
			
			if ( OnShift != null ) {
				OnShift( index );
			}
		}
	}
}