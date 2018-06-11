using UnityEngine;
using Dumpster.Core.BuiltInModules.Input;

public class PlayerMovement : Brain, IInputReciever<Eden.Input.Package> {

	void IInputReciever<Eden.Input.Package>.RecieveInput ( Eden.Input.Package package ) {
		print( "recieved" );
		if ( _horizontal != package.Horizontal ){ _horizontal = package.Horizontal; }
		if ( _vertical != package.Vertical ){ _vertical = package.Vertical; }
	}
	void IInputReciever<Eden.Input.Package>.EnteredInputFocus () {}
	void IInputReciever<Eden.Input.Package>.ExitInputFocus () {}
	

	[SerializeField] private Player _player;
	[SerializeField] private PlayerAgressiveSubBrain _agressive;
	[SerializeField] private PlayerPassiveSubBrain _passive;
	[SerializeField] private LayerMask _mask;

	private CameraType _cameraType;
	private float _horizontal;
	private float _vertical;

	private const float RAYCAST_INSET = 0.01f;
	private const float RAYCAST_DISTANCE = 0.1f;

	public override void Think () {

		switch ( _cameraType ) {
			
			case CameraType.Passive:
				_passive.Think( _horizontal, _vertical );
				break;
			
			case CameraType.Agressive:
				_agressive.Think( _horizontal, _vertical );
				break;
		}
	}

	[SerializeField] private float _jumpVelocity = 10f;

	private void Update () {

		if ( _player.Physics.State.DownIsColliding && UnityEngine.Input.GetKeyDown( KeyCode.Space ) ) {
			_player.Physics.AddVelocity( new Vector3( 0, _jumpVelocity, 0) );
		}
	}
	private void Start () {

		EdensGarden.Instance.Input.RegisterToInputLayer( EdensGarden.Constants.InputLayers.Player, this );

		_player.QuickSlot.OnInputChanged += newSlotID => {
			
			var index = _player.QuickslotInventory.ConvertQuickSlotIDToIndex( newSlotID );
			var item = _player.QuickslotInventory.GetInventoryItem( index );

			if ( item.CanShoot ) {
				_cameraType = CameraType.Agressive;
			} else {
				_cameraType = CameraType.Passive;
			}
		};
	}
	

	private enum CameraType {
		Passive,
		Agressive
	}	
}
