using UnityEngine;
using System;

namespace Eden.Model {

	[System.Serializable]
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

		
		// ***************** Serialize Properties *******************

		[SerializeField] private string _uniqueID;
		[SerializeField] private string _prefabID;
		[SerializeField] private int _count;
		
		public string UniqueID {
			get{return _uniqueID; }
			protected set{ _uniqueID = value; }
		}	
		public string PrefabID {
			get { return _prefabID; }
			protected set { _prefabID = value; }
		}
		public int Count { 
			get { return _count; }
			protected set { _count = value; }
		}
		

		// ***************** Properties *******************
		
		public string DisplayName { 
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
			get{ return (this as ShootableItem); }
		}

		
		// ***************** Action *******************
		
		public bool IsActionable {
			get{ return (AsActionableItem != null); }
		}
		public ActionableItem AsActionableItem {
			get{ return (this as ActionableItem); }
		}

		// ***************** Gun Buildable *******************
		
		public bool IsGunBuildable {
			get{ return (AsGunBuildable != null); }
		}
		public GunBuildableItem AsGunBuildable {
			get{ return (this as GunBuildableItem); }
		}
		
		
		
		// ***************** Public *******************
		
		public void Use( Eden.Life.Chips.InteractorChip interactor, Action onComplete ) {

			OnUse( interactor, onComplete );

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

		protected virtual void OnUse ( Eden.Life.Chips.InteractorChip interactor, Action onComplete ) {}
		
		
		// ***************** Private *******************
		
		private void FireCountChangedEvent () {
			
			if ( OnCountChanged != null ){
				OnCountChanged ();
			}
		}
	}
}
