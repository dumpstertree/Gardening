
namespace Dumpster.AI {

	public interface IStateAction {

		bool Complete { get; }

		void Start ();
		void Kill ();
	}
}