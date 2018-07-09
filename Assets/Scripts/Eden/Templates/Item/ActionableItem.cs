namespace Eden.Templates {
	
	public class ActionableItem : Item {

		public override Eden.Model.Item CreateInstance() {
			return new Eden.Model.ActionableItem( _id, _displayName, _maxCount, _expendable, _sprite );
		}
	}
}