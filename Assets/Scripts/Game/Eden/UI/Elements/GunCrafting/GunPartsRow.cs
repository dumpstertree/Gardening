using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GunPartsRow : MonoBehaviour, IPointerDownHandler,  IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

	public delegate void DragEvent( Model.Gun.Part part );
	public DragEvent OnDragBegin;
	public DragEvent OnDragEnd;

	public void SetPart ( Model.Gun.Part part ) {
		
		_part = part;
		_text.text = part.PrefabName;
	}

	// ***********************************

	[SerializeField] private Text _text;
	[SerializeField] private Image _image;

	private Model.Gun.Part _part;
	private bool _pointerDown;
	private bool _pointerOverObject;
	
	private void UpdateState () {
    	
    	if ( _pointerDown && _pointerOverObject ) {
    		HandleOnDragBegin();
    	} else {
    		HandleOnDragEnd();
    	}
    }
    private void HandleOnDragBegin () {
    	
    	if ( OnDragBegin != null ) {
    		OnDragBegin( _part );
    	}
    }
    private void HandleOnDragEnd () {
    	
    	if ( OnDragEnd != null ) {
    		OnDragEnd( _part );
    	}
    }

	// ***********************************

	void IPointerDownHandler.OnPointerDown( PointerEventData eventData ) {
		
		_pointerDown = true;
		
		UpdateState();
    }
    void IPointerUpHandler.OnPointerUp( PointerEventData eventData ) {
    	
    	_pointerDown = false;
    	
    	UpdateState();
    }
    void IPointerEnterHandler.OnPointerEnter( PointerEventData eventData ) {
    	
        _pointerOverObject = true;
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
    
    	_pointerOverObject = false;
    }
}
