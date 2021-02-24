using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Dialogue
{
	public class DialogueUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI questText;
		[SerializeField] private Button nextButton;

		private PlayerDialogue _playerDialogue;

		// Use this for initialization
		private void Start()
		{
			_playerDialogue = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogue>();
			nextButton.onClick.AddListener(Next);

			UpdateUI();
		}

		// Update is called once per frame
		private void Update()
		{

		}

		private void Next()
		{
			_playerDialogue.Next();
			UpdateUI();
		}

		private void UpdateUI()
		{
			questText.text = _playerDialogue.GetText();
			nextButton.gameObject.SetActive(_playerDialogue.HasNextText());
		}
	}
}
