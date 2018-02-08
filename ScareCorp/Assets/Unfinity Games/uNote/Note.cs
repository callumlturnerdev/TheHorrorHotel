using UnityEngine;
using System.Collections;

namespace UnfinityGames.uNote
{
	[AddComponentMenu("Unfinity Games/uNote/Note")]
	public class Note : MonoBehaviour
	{
		[Note]
		public NoteData note;
	}

	//A data holder that keeps track of our note's data.
	[System.Serializable]
	public class NoteData
	{
		public string Title;
		public string Text;

		public bool Editing;
		public bool HasEditedDefaults;

		public bool ForceCompact = false;
	}
}
