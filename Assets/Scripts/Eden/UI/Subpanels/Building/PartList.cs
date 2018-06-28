using System.Collections.Generic;
using UnityEngine;

public class PartList : MonoBehaviour {

	
	//******************* Public ***************************

	public bool Enabled {
		get {
			return _enabled;
		}
		set { 
			if ( _enabled != value ) {
				if ( value ) { Enable();
				} else { Disable(); }
			} 
		}
	}
	
	public Part TakePart ( int atIndex ) {

		if ( _parts.Count > atIndex - 1 ) {
			
			var part = _parts[ atIndex ];
			_parts.RemoveAt( atIndex );
			RemovePartVisual( atIndex );
			_index --;
			
			return part;
		}

		return null;
	}
	public Part PeakAtPart ( int atIndex  ) {

		if ( _parts.Count > atIndex - 1 ) {
			return _parts[ atIndex ];
		}

		return null;
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

	
	//******************* Private ***************************

	[SerializeField] private PartVisual _partVisualPrefab;
	[SerializeField] private Transform _content;

	private List<PartVisual> _partVisuals;
	private List<Part> _parts;
	private bool _enabled = true;
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


	private void Enable () {
			
		_enabled = true;
	}
	private void Disable () {
			
		_enabled = false;
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

		if ( !_enabled ) {
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


public class Part {

	public string Name { get; }
	public char[,] Blocks { get; }
	public BuiltStats BuilderStats { get; }

	public Part( string name ) {

		// Projectors 	⇡ ⇢ ⇣ ⇠
		// Recievers  	∪ ⊂ ∩ ⊃

		Name = name;
		BuilderStats = new BuiltStats( Random.Range( -2, 2 ),
									   Random.Range( -2, 2 ),
									   Random.Range( -2, 2 ),
									   Random.Range( -2, 2 ),
									   Random.Range( -2, 2 ),
									   Random.Range( -2, 2 ),
									   Random.Range( -2, 2 ) );
		Blocks = new char[3,3] {
			
			{ 'o','x','⇢' },
			{ 'x',' ',' ' },
			{ '∩',' ',' ' }
		};
	}
}