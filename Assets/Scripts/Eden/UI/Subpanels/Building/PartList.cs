using System.Collections.Generic;
using UnityEngine;

public class PartList : BuildSubpanel {

	
	//******************* Public ***************************
	
	public Part TakePart ( int atIndex ) {

		if ( _parts.Count > atIndex - 1 ) {
			
			var part = _parts[ atIndex ];
			_parts.RemoveAt( atIndex );
			RemovePartVisual( atIndex );
			__index = 0;
			
			return part;
		}

		return null;
	}
	public Part PeakAtPart ( int atIndex  ) {
		
		return  ( _parts.Count > atIndex - 1 ) ? _parts[ atIndex ] : null;
	}


	public delegate void ShiftEvent( int index );
	public ShiftEvent OnShift;

	public delegate void PartClickedEvent ( int index );
	public PartClickedEvent OnPartClicked;

	public delegate void TriedToBreakFreeEvent ();
	public TriedToBreakFreeEvent OnTriedToBreakFreeUp;
	public TriedToBreakFreeEvent OnTriedToBreakFreeDown;
	public TriedToBreakFreeEvent OnTriedToBreakFreeLeft;
	public TriedToBreakFreeEvent OnTriedToBreakFreeRight;

	
	//******************* Protected ***************************
	
	protected override void Enable () {
	
		_partVisuals[ _index ].IsSelected = true;
		FireShiftEvent( _index );
	}
	protected override void Disable () {
	
		foreach ( PartVisual p in _partVisuals ) {
			p.IsSelected = false;
		}
	}


	//*************≠****** Private ***************************

	[SerializeField] private PartVisual _partVisualPrefab;
	[SerializeField] private Transform _content;

	private List<PartVisual> _partVisuals;
	private List<Part> _parts;
	private int __index;
	
	
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

		_parts = new List<Part>();
		_partVisuals = new List<PartVisual>();
		
		_parts.Add( new Part( "1" ) );
		_parts.Add( new Part( "2" ) );
		_parts.Add( new Part( "3" ) );
		_parts.Add( new Part( "4" ) );
		_parts.Add( new Part( "5" ) );

		BuildPartVisuals ();
	}
	private void Update () {

		if ( !Enabled ) {
			return;
		}

		if ( Input.GetKeyDown( KeyCode.UpArrow ) ) {
			ShiftUp ();
		}
		if ( Input.GetKeyDown( KeyCode.DownArrow ) ) {
			ShiftDown ();
		}
		if ( Input.GetKeyDown( KeyCode.LeftArrow ) ){
			ShiftLeft ();
		}
		if ( Input.GetKeyDown( KeyCode.RightArrow ) ) {
			ShiftRight ();	
		}
		if ( Input.GetKeyDown( KeyCode.Space ) ) {
			FirePartClickedEvent ( _index );	
		}
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

		foreach ( Part p in _parts ) {
			
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