using UnityEngine;
using System.Collections;

public class PlayerAgressiveSubBrain : MonoBehaviour {

	public void Think( float horizontal, float vertical, float cameraHorizontal, float cameraVertical, bool dash ){
		
		_shoulderCameraController.MovementHorizontal = horizontal;
		_shoulderCameraController.MovementVertical = vertical;
		_shoulderCameraController.CameraHorizontal = cameraHorizontal;
		_shoulderCameraController.CameraVertical = cameraVertical;

		RaycastHit hit;
		if (Physics.Raycast( _player.ProjectileSpawner.position, _player.ProjectileSpawner.forward, out hit, Mathf.Infinity, _layerMask )) {
			_shoulderCameraController.ReticlePosition =  Camera.main.WorldToScreenPoint( hit.point );
		}else {
			_shoulderCameraController.ReticlePosition = Vector2.zero;
		}

		Rotate( horizontal, vertical, cameraHorizontal, cameraVertical );

		if (horizontal != 0 || vertical != 0){
			Move( horizontal, vertical );
		}

		Animate( horizontal, vertical );

		if ( dash ) {
			Dash( horizontal, vertical);
		}
	}

	[SerializeField] private LayerMask _layerMask;
	[SerializeField] private Dumpster.Core.BuiltInModules.ShoulderCameraController _shoulderCameraController;
	[SerializeField] private Eden.Life.BlackBoxes.Player _player;
	[SerializeField] private float _speed;
	[SerializeField] private AnimationCurve _dashCurve;
	[SerializeField] private float _dashTime;

	private const string HORIZONTAL_ANIMATION_NAME = "Horizontal";
	private const string VERTICAL_ANIMATION_NAME = "Vertical";

	private bool _isDashing;

	public void WillBecomeActive () {

		var worldUp = Vector3.up;
		var cameraLeft = -Camera.main.transform.right;
		var cameraForward = Vector3.Cross( worldUp , cameraLeft );

		_player.transform.forward = cameraForward;
		_player.ProjectileSpawner.transform.localRotation = Quaternion.identity;
	}
	private void Rotate ( float horizontal, float vertical, float cameraHorizontal, float cameraVertical ) {
	
		_player.ProjectileSpawner.rotation = Quaternion.AngleAxis( cameraHorizontal * 3, Vector3.up ) * _player.ProjectileSpawner.rotation;
		_player.ProjectileSpawner.rotation = Quaternion.AngleAxis( -cameraVertical * 3 , _player.ProjectileSpawner.right ) * _player.ProjectileSpawner.rotation;

		var worldUp = Vector3.up;
		var projectileSpawnerLeft = -_player.ProjectileSpawner.right;
		var cameraForward = Vector3.Cross( worldUp , projectileSpawnerLeft );
		var p = _player.ProjectileSpawner.parent;

		_player.ProjectileSpawner.parent = null;
		_player.transform.forward = cameraForward;
		_player.ProjectileSpawner.parent = p;

		_shoulderCameraController.Forward = _player.ProjectileSpawner.forward;
	}
	private float FindDifInVectors ( Vector3 a, Vector3 b ) {

		var aa = Mathf.Sqrt( (a.x * a.x) + (a.y * a.y) + (a.z * a.z) );
		var bb = Mathf.Sqrt( (b.x * b.x) + (b.y * b.y) + (b.z * b.z) );
		var dot = (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
		
		return Mathf.Acos( dot / (aa * bb) ) * Mathf.Rad2Deg;
	}

	private void Dash ( float horizontal, float vertical ) {

		if ( _isDashing ) { return; }
		_isDashing = true;


		var rads = Mathf.Atan2( vertical, horizontal );
		var degrees = rads * Mathf.Rad2Deg;
		var adjusted = ( degrees - 90 ) * -1;
		var y = Camera.main.transform.eulerAngles.y + adjusted;

		var worldUp = Vector3.up;
		var camLeft = -Camera.main.transform.right;
		var camForward = Vector3.Cross( worldUp , camLeft );

		var x = Quaternion.AngleAxis( y, worldUp ) * Vector3.forward * 3f;
		
		StartCoroutine( DashCoroutine( _player.Physics.transform.position, _player.Physics.transform.position + x ));

		EdensGarden.Instance.Effects.FreezeFrame( .5f );
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
	
	private IEnumerator DashCoroutine ( Vector3 startPos, Vector3 targetPos ) {

		for( float t=0f; t<_dashTime; t+=Time.deltaTime ) {
			
			var frac = _dashCurve.Evaluate( t/_dashTime );
			var thisFrameTarget = Vector3.Lerp( startPos, targetPos, frac );
			var delta = thisFrameTarget - _player.Physics.transform.position ;

			_player.Physics.MovePosition( delta );
			
			yield return null;
		}

		_isDashing = false;
	}
}
