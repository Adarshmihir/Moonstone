using UnityEngine;
using System.Collections;
using TMPro;

namespace Dialogue
{
	public class DialogueUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI questText;

		private PlayerDialogue _playerDialogue;

		// Use this for initialization
		private void Start()
		{
			_playerDialogue = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogue>();

		}

		// Update is called once per frame
		private void Update()
		{

		}
	}
}
