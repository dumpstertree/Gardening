namespace Dumpster.Core.Life {
	
	public abstract class LogicChip<T> : Chip<T> where T : class {

		protected abstract void Think ();
		protected override void Init () {
			
			base.Init();

			BlackBox.OnThink += Think;
		}
	}
}