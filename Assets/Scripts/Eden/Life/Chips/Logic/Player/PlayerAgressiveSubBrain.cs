using UnityEngine;

public class PlayerAgressiveSubBrain : MonoBehaviour {

	public void Think ( float horizontal, float vertical, float cameraHorizontal, float cameraVertical ){
		
		_shoulderCameraController.MovementHorizontal = horizontal;
		_shoulderCameraController.MovementVertical = vertical;
		_shoulderCameraController.CameraHorizontal = cameraHorizontal;
		_shoulderCameraController.CameraVertical = cameraVertical;

		Rotate( horizontal, vertical );

		if (horizontal != 0 || vertical != 0){
			Move( horizontal, vertical );
		}

		Animate( horizontal, vertical );
	}

	[SerializeField] private Dumpster.Core.BuiltInModules.ShoulderCameraController _shoulderCameraController;
	[SerializeField] private Eden.Life.BlackBoxes.Player _player;
	[SerializeField] private float _speed;

	private const string HORIZONTAL_ANIMATION_NAME = "Horizontal";
	private const string VERTICAL_ANIMATION_NAME = "Vertical";

	private void Rotate ( float horizontal, float vertical ) {
		
		var worldUp = Vector3.up;
		var camLeft = -Camera.main.transform.right;
		var newForward = Vector3.Cross( worldUp , camLeft );

		_player.transform.forward = newForward;
	}
	private void Move ( float horizontal, float vertical  ) {

		var h = horizontal * _speed * Time.deltaTime;
		var v = vertical * _speed * Time.deltaTime;

		_player.Physics.MovePosition( transform.right * h );
		_player.Physics.MovePosition( transform.forward * v );
	}
	private void Animate ( float horizontal, float vertical  ) {

		_player.Animator.SetFloat( HORIZONTAL_ANIMATION_NAME, horizontal );
		_player.Animator.SetFloat( VERTICAL_ANIMATION_NAME, vertical );
	}

}
