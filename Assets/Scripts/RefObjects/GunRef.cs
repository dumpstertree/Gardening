using System.Collections;
using UnityEngine;

public class GunRef {

	private Model.Gun _gun;


	// ************* INIT ****************

	public GunRef ( Model.Gun gun ) {
		
		_gun = gun;
	}
	public void UpdateModel ( Model.Gun gun ) {

		_gun = gun;
		HandleGunChanged();
	}
	
	// ************* DELEGATES ****************

	public delegate void AvailableBulletsChangeEvent( int availableBullets );
	public AvailableBulletsChangeEvent OnAvailableBulletsChange;

	public delegate void ReloadTimeChangedEvent( float currentReloadTime, float maxReloadTime );
	public ReloadTimeChangedEvent OnReloadTimeChanged;

	public delegate void GunUpdatedEvent();
	public GunUpdatedEvent OnGunChanged;
	
	// ************ PUBLIC *****************

 	public Model.Gun Gun {
 		get{ return _gun; }
 	}
	public int AvailableBullets {
		get{ return _availableBullets; }
		set{ HandleOnAvailableBulletsChanged( value ); }
	}


	// **************************

	public void Reload () {
		
		if ( !_reloading ) {

			Game.Instance.StartCoroutine( WaitForReload() );
		}
	}
	public void Fire ( Creature user, GameObject bulletPrefab ) {

		// if trying to fire and no bullets reload
		if ( _availableBullets <= 0 ) {
			
			Reload();
			return;
		}

		// if not already firing start
		if ( !_firing ) {

			Game.Instance.StartCoroutine( WaitForFire( user, bulletPrefab ) );
		}
	}


	// ************ PRIVATE **************

	private bool _firing;
	private bool _reloading;
	private int _availableBullets;

	private void CreateBullet ( Creature user, GameObject bulletPrefab ) {

		var go = GameObject.Instantiate( bulletPrefab );
		var bulletSpread = 5f;
		var spreadR = UnityEngine.Random.Range( -bulletSpread, bulletSpread);
		var spreadU = UnityEngine.Random.Range( -bulletSpread, bulletSpread);
	
		go.transform.position = user.GunProjector.transform.position;
		go.transform.rotation = user.GunProjector.transform.rotation;
		go.transform.rotation = go.transform.rotation * Quaternion.AngleAxis( spreadR, go.transform.right );
		go.transform.rotation = go.transform.rotation * Quaternion.AngleAxis( spreadU, go.transform.up );

		var hitData = new HitData();
		hitData.Power = 1;
		go.GetComponent<Bullet>().SetBullet( user, hitData);

		AvailableBullets--;
	}


	// **************************
	
	private void HandleOnAvailableBulletsChanged ( int availableBullets ) {
		
		_availableBullets = availableBullets;
		
		if ( OnAvailableBulletsChange != null ) {
			OnAvailableBulletsChange( _availableBullets );
		}
	}
	private void HandleOnReloadTimeChanged ( float currentReloadTime, float maxReloadTime ) {
		
		if ( OnReloadTimeChanged != null ) {
			OnReloadTimeChanged( currentReloadTime, maxReloadTime );
		}
	}
	private void HandleGunChanged () {
		
		if ( OnGunChanged != null ) {
			OnGunChanged();
		}
	}
	

	// **************************

	private IEnumerator WaitForReload (){

		// start reloading
		_reloading = true;

		// wait for reload time to end
		var reloadTime = Gun.WeaponStats.ReloadTime;
		var numOfBullets = Gun.WeaponStats.NumberOfBullets;

		for ( float t = 0f; t < reloadTime; t+=Time.deltaTime ) {
			
			HandleOnReloadTimeChanged( t, reloadTime );
			yield return null;
		}

		HandleOnReloadTimeChanged( reloadTime, reloadTime );

		// set the available bullets to the clipsize
		AvailableBullets = Gun.WeaponStats.ClipSize;

		// end reloading
		_reloading = false;
	}
	private IEnumerator WaitForFire ( Creature user, GameObject bulletPrefab ){
		
		// set firing
		_firing = true;
		
		// create all the bullets
		var numOfBullets = Gun.WeaponStats.NumberOfBullets;
		for ( int i=0; i<numOfBullets; i++ ) { CreateBullet( user, bulletPrefab ); }
		
		// wait for the time between fire rate
		var fireRate = 1f / Gun.WeaponStats.FireRate;
		for ( float t = 0f; t < fireRate; t+=Time.deltaTime ) {
			yield return null;
		}

		// end firing
		_firing = false;
	}
}
