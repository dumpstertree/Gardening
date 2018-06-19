/*using UnityEngine;

public class InteractorPostion : MonoBehaviour {

	private Eden.Life.BlackBox _player {
		get{ return EdensGarden.Instance.Rooms.CurrentArea.LoadedPlayer.GetComponent<Eden.Life.BlackBox>(); }		
	}

	public void ChangeState( OrbPosition.State newState ){
		
		if ( _state != newState ) {
			
			_state = newState;

			foreach ( OrbPosition o in _orbPositions )	{
				o.OrbState = newState;
			}
		}
	}
	public void ChangeTracking( Tracking newTracking ){

		if ( _tracking != newTracking ) {

			_tracking = newTracking;
		}
	}

	// *************************

	private const float LERP_SPEED = 0.5f;

	private OrbPosition[] _orbPositions;
	private OrbPosition.State _state;
	private Tracking _tracking;

	// *************************

	private void Awake () {
		
		_orbPositions = GetComponentsInChildren<OrbPosition>();
	}
	private void Update () {

		switch( _tracking ) {
			
			case Tracking.Interactable:
				LiveInteractableTracking();
				break;
			
			case Tracking.True:
				LiveTrueTracking();
				break;
			
			case Tracking.Player:
				LivePlayerTracking();
				break;
		}
	}

	// *************************

	private void LiveInteractableTracking () {
		
		var startPos = transform.position;
		var targetPos = _player.Interactor.InteractableObject.transform.position;
		transform.position = Vector3.Lerp( startPos, targetPos, LERP_SPEED );
	}
	private void LiveTrueTracking () {
		
		var startPos = transform.position;
		var targetPos = _player.Interactor.transform.position;
		transform.position = Vector3.Lerp( startPos, targetPos, LERP_SPEED );
	}
	private void LivePlayerTracking () {
		
		var startPos = transform.position;
		var targetPos = _player.transform.position;
		transform.position = Vector3.Lerp( startPos, targetPos, LERP_SPEED );
	}

	// *************************

	public enum Tracking {
		Invalid,
		Interactable,
		True,
		Player
	}
}
*/