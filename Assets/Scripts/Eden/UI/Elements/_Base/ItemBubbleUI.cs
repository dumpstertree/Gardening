using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Eden.Model;

public class ItemBubbleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler {

	// ********* PUBLIC ******************

	public Item Item {
		get { return _item; }
	}
	public bool Indestuctable {
		get{ return _indestuctable; }
	}
	public int Index {
		get { return _index; }
		set { _index = value; }
	}
 	public void SetItem ( Item item ) {

 		_item = item;
		if (item == null){
			SetUnfilledSlot();
		}
		else{
			SetFilledSlot( item );
		}
	}
	public bool HasItem {
		get{ return _item != null;  }
	}

	public delegate void OnPointerEvent ();
	public OnPointerEvent PointerEnter;
	public OnPointerEvent PointerExit;
	public OnPointerEvent PointerDown;
	public OnPointerEvent PointerUp;
	
	public delegate void SelectEvent ();
	public SelectEvent OnSelect;
	public SelectEvent OnDeselect;

	public delegate void ClickEvent ();
	public ClickEvent OnClick;


	private void FireClickEvent () {
		if ( OnClick != null ) {
			OnClick ();
		}
	}
	void ISelectHandler.OnSelect( BaseEventData eventData ) {
    	if (OnSelect != null) {
    		OnSelect ();
    	}
    }
    void IDeselectHandler.OnDeselect( BaseEventData eventData ) {
    	if (OnDeselect != null) {
    		OnDeselect ();
    	}
    }

	void IPointerEnterHandler.OnPointerEnter( PointerEventData eventData ) {
		if (PointerEnter != null){
			PointerEnter();
		}
	}
	void IPointerExitHandler.OnPointerExit( PointerEventData eventData ) {
		if (PointerExit != null){
			PointerExit();
		}
	}
	void IPointerDownHandler.OnPointerDown( PointerEventData eventData ) {
		if (PointerDown != null){
			PointerDown();
		}
	}
	void IPointerUpHandler.OnPointerUp( PointerEventData eventData ) {
		if (PointerUp != null){
			PointerUp();
		}
	}


	// ********* PRIVATE ******************

	[SerializeField] private GameObject _filledObject;
	[SerializeField] private Image _sprite;
	[SerializeField] private Text _countText;
	[SerializeField] private bool _indestuctable;
	[SerializeField] private Button _button;

	private const string DEFAULT_STATE_TRIGGER = "Default";
	private const string HOVER_STATE_TRIGGER = "Hover";

	private int _index = -1;
	private Item _item;

	private void Awake () {
	
		_button.onClick.AddListener( () => FireClickEvent()  );
	}
	private void SetFilledSlot ( Item item) {

		_filledObject.SetActive( true );

		_sprite.sprite = item.Sprite;
		_countText.text = item.Count.ToString() + "x";
	}
	private void SetUnfilledSlot () {

		_filledObject.SetActive( false );
	}

	// ************************************

	public enum State {
		Default,
		Hover
	}
}
