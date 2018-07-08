using UnityEngine;

namespace Eden.Templates {

	public class Item : ScriptableObject {

		
		// *************** Static ******************

		public static Item GetTemplate( string forID ) {
			
			var templates = Resources.LoadAll<Item>( "" );
			Item nullReturn = null; 
			
			foreach ( Item t in templates ) {
				
				if ( t._id == forID ) { return t; }
				if ( t._id == NULL_TEMPLATE_ID ){ nullReturn = t; }
			}

			return nullReturn;
		}

		
		// **************** Public *****************

		public virtual Eden.Model.Item CreateInstance() {
			
			return new Eden.Model.Item( _id, _displayName, _maxCount, _expendable, _sprite );
		}


		// **************** Private ***************

		private const string NULL_TEMPLATE_ID = "null.null";

		[Header( "Storage" )]
		[SerializeField] protected string _id = "example.id";

		[Header( "Name" )]
		[SerializeField] protected string _displayName = "example name";

		[Header( "Visuals" )]
		[SerializeField] protected Sprite _sprite = null;
		[SerializeField] protected GameObject _holdItem = null;

		[Header( "Count" )]
		[SerializeField] protected int _maxCount = 1;
		[SerializeField] protected bool _expendable = true;	
	}
}