using UnityEngine;

public class UiPanel : MonoBehaviour {

	[SerializeField] private bool _presented;
	[SerializeField] private bool _hasBeenInited;

	public void Init () {

		_hasBeenInited = true;
		OnInit();
	}
	public void Present(){

		_presented = true; 
		gameObject.SetActive( true );
		OnPresent();
	}
	public void Dismiss(){
		
		_presented = false; 
		gameObject.SetActive( false );
		OnDismiss();
	}

	protected virtual void OnInit () {}
	protected virtual void OnPresent () {}
	protected virtual void OnDismiss () {}
}
