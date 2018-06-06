using UnityEngine;

public class PlayerMovement : Brain, IInputReciever {
	
	void IInputReciever.OnConfirmDown () {}
	void IInputReciever.OnConfirmUp () {}
	void IInputReciever.OnCancelDown () {}
	void IInputReciever.OnCancelUp () {}
	void IInputReciever.OnStartDown (){}
	void IInputReciever.OnStartUp (){}
	void IInputReciever.HorizontalChanged ( float horizontal ) {
		
		_horizontal = horizontal;
	}
	void IInputReciever.VerticalChanged ( float vertical ) {

		_vertical = vertical;
	}

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
