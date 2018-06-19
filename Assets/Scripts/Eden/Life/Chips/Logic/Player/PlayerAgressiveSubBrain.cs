using UnityEngine;

public class PlayerAgressiveSubBrain : MonoBehaviour {

	public void Think ( float horizontal, float vertical, float cameraHorizontal, float cameraVertical ){
		
		_horizontal = horizontal;
		_vertical = vertical;
		_cameraHorizontal = cameraHorizontal;
		_cameraVertical = cameraVertical;

		_shoulderCameraController.MovementHorizontal = _horizontal;
		_shoulderCameraController.MovementVertical = _vertical;
		_shoulderCameraController.CameraHorizontal = _cameraHorizontal;
		_shoulderCameraController.CameraVertical = _cameraVertical;

		if (_horizontal != 0 || _vertical != 0){
			Rotate();
			Move();
		}

		Animate();
	}

	[SerializeField] private Dumpster.Core.BuiltInModules.ShoulderCameraController _shoulderCameraController;
	[SerializeField] private Eden.Life.BlackBoxes.Player _player;
	[SerializeField] private float _speed;

	private const string HORIZONTAL_ANIMATION_NAME = "Horizontal";
	private const string VERTICAL_ANIMATION_NAME = "Vertical";

	private float _horizontal;
	private float _vertical;
	private float _cameraHorizontal;
	private float _cameraVertical;


	private void Rotate () {
		
		var worldUp = Vector3.up;
		var camLeft = -Camera.main.transform.right;
		var newForward = Vector3.Cross( worldUp , camLeft );

		_player.transform.forward = newForward;
	}
	private void Move () {

		var h = _horizontal * _speed * Time.deltaTime;
		var v = _vertical * _speed * Time.deltaTime;

		_player.Physics.MovePosition( transform.right * h );
		_player.Physics.MovePosition( transform.forward * v );
	}
	private void Animate () {

		_player.Animator.SetFloat( HORIZONTAL_ANIMATION_NAME, _horizontal );
		_player.Animator.SetFloat( VERTICAL_ANIMATION_NAME, _vertical );
	}
}
