﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiPanel : MonoBehaviour, IPointerDownHandler,  IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IInputReciever {

	public delegate void PointerEvent();
	
	public PointerEvent OnPointerDownEvent;
	public PointerEvent OnPointerIsStillDownEvent;
	public PointerEvent OnPointerUpEvent;
	public PointerEvent OnPointerEnterEvent;
	public PointerEvent OnPointerExitEvent;

	public Action OnExit {
		get { return _onExit; }
		set { _onExit = value; }
	}
	public bool ShouldRecieveInput { 
		get { return _shouldRecieveInput; } 
	}
	public void Init () {

		_hasBeenInited = true;
		OnInit();
	}
	public void Present () {

		_presented = true; 
		gameObject.SetActive( true );
		OnPresent();
	}
	public void Dismiss () {
		
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

	void IInputReciever.OnConfirmDown () {
		OnConfirmDown ();
	}
	void IInputReciever.OnConfirmUp () {
		OnConfirmUp ();
	}
	void IInputReciever.OnCancelDown () {
		OnCancelDown ();
	}
	void IInputReciever.OnCancelUp () {
		OnCancelUp ();
	}
	void IInputReciever.OnStartDown () {
		OnStartDown ();
	}
	void IInputReciever.OnStartUp () {
		OnStartlUp ();
	}
	void IInputReciever.HorizontalChanged ( float horizontal ) {
		HorizontalChanged ( horizontal );
	}
	void IInputReciever.VerticalChanged ( float vertical ) {
		VerticalChanged ( vertical );
	}


	//******************************
	
	#pragma warning disable 0414
	[SerializeField] private bool _presented;
	[SerializeField] private bool _hasBeenInited;
	#pragma warning restore 0414

	[SerializeField] protected bool _shouldRecieveInput;

	private bool _pointerDown;
	private Action _onExit;

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

    //******************************

	protected virtual void OnInit () {}
	protected virtual void OnPresent () {}
	protected virtual void OnDismiss () {}
	protected virtual void OnConfirmDown () {}
	protected virtual void OnConfirmUp () {}
	protected virtual void OnCancelDown () {}
	protected virtual void OnCancelUp () {}
	protected virtual void OnStartDown () {}
	protected virtual void OnStartlUp () {}
	protected virtual void HorizontalChanged ( float horizontal ) {}
	protected virtual void VerticalChanged ( float vertical ) {}

	protected void Exit () {
		
		if ( OnExit != null ) {
			
			Action onExit = OnExit;
			OnExit = null;

			onExit ();
		}
	}

}
