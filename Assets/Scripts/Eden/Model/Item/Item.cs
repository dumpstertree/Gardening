using UnityEngine;
using System;

namespace Eden.Model {

	public class Item {

		
		// ***************** Constructor ***********************

		public Item () {}
		public Item( string prefabID, string displayName, int maxCount, bool expendable, Sprite sprite ) {

			Count = 1;
			UniqueID = Guid.NewGuid().ToString();
			
			PrefabID = prefabID;
			DisplayName = displayName;
			MaxCount = maxCount;
			Expendable = expendable;
			Sprite = sprite;
		}
		
		
		// ***************** Events ***********************
		
		public delegate void CountChangedEvent ();
		public CountChangedEvent OnCountChanged;

		
		// ***************** Properties *******************

		public string UniqueID {
			get; protected set;
		}		
		public string PrefabID { 
			get; protected set;
		}
		public string DisplayName { 
			get; protected set;
		}
		public int Count { 
			get; protected set;
		}
		public int MaxCount { 
			get; protected set;
		}
		public bool Expendable { 
			get; protected set;
		}
		public Sprite Sprite { 
			get; protected set;
		}


		// ***************** Shoot *******************
		
		public bool IsShootable {
			get{ return (AsShootableItem != null); }
		}
		public virtual ShootableItem AsShootableItem {
			get{ return (this as FixedShootableItem); }
		}

		
		// ***************** Action *******************
		
		public bool IsActionable {
			get{ return (AsActionableItem != null); }
		}
		public ActionableItem AsActionableItem {
			get{ return (this as ActionableItem); }
		}
		
		
		// ***************** Public *******************
		
		public void Use( Eden.Life.BlackBox user, Eden.Interactable.InteractableObject interactable, Action onComplete ) {

			OnUse( user, interactable, onComplete );

			if ( Expendable ) {
				ReduceCount( 1 );
			}
		}
		public void AddCount( int more ) {

			Count += more;
			FireCountChangedEvent ();
		}
		public void ReduceCount( int less ) {

			Count -= less;
			FireCountChangedEvent ();
		}
		public void SetCount( int count ) {
			
			Count = count;
			FireCountChangedEvent ();
		}
	
		
		// ***************** Protected *******************

		protected virtual void OnUse ( Eden.Life.BlackBox user, Eden.Interactable.InteractableObject interactable, Action onComplete ) {}
		
		
		// ***************** Private *******************
		
		private void FireCountChangedEvent () {
			
			if ( OnCountChanged != null ){
				OnCountChanged ();
			}
		}
	}
}
