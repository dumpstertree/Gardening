using UnityEngine;
using UnityEngine.EventSystems;

public class UiPanel : MonoBehaviour, IPointerDownHandler,  IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

	public delegate void PointerEvent();
	
	public PointerEvent OnPointerDownEvent;
	public PointerEvent OnPointerIsStillDownEvent;
	public PointerEvent OnPointerUpEvent;
	public PointerEvent OnPointerEnterEvent;
	public PointerEvent OnPointerExitEvent;

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

	//******************************

    void IPointerDownHandler.OnPointerDown( PointerEventData eventData ) {
    	HandlePointerDown();
    }
    void IPointerUpHandler.OnPointerUp( PointerEventData eventData ) {
    	HandlePointerUp();
    }
    void IPointerEnterHandler.OnPointerEnter( PointerEventData eventData ) {
    	HandlePointerEnter();
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
    	HandlePointerExit();
    }

	//******************************

	protected virtual void OnInit () {}
	protected virtual void OnPresent () {}
	protected virtual void OnDismiss () {}

	//******************************
	
	[SerializeField] private bool _presented;
	[SerializeField] private bool _hasBeenInited;

	private bool _pointerDown;
	
	protected virtual void Update () {
		
		if ( _pointerDown ){
			HandlePointerIsStillDown();
		}
	}
	private void HandlePointerDown () {
		
		_pointerDown = true;

		if ( OnPointerDownEvent != null ){
			OnPointerDownEvent();
		}
	}
	private void HandlePointerIsStillDown () {
		
		if ( OnPointerIsStillDownEvent != null ){
			OnPointerIsStillDownEvent();
		}
	}
    private void HandlePointerUp () {

    	_pointerDown = false;
    	
    	if ( OnPointerUpEvent != null ){
			OnPointerUpEvent();
		}
    }
    private void HandlePointerEnter () {
    	
    	if ( OnPointerEnterEvent != null ){
			OnPointerEnterEvent();
		}
    }
    private void HandlePointerExit () {
    	
    	if ( OnPointerExitEvent != null ){
			OnPointerExitEvent();
		}
    }
}
