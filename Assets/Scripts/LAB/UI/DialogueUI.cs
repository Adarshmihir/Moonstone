using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Dialogue;

namespace UI
{
	public class DialogueUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI questText;
		[SerializeField] private TextMeshProUGUI aiName;
		[SerializeField] private Button nextButton;
		[SerializeField] private Button quitButton;
		[SerializeField] private Transform choiceList;
		[SerializeField] private GameObject choicePrefab;
		[SerializeField] private GameObject response;
		[SerializeField] private GameObject deco;

		private PlayerDialogue _playerDialogue;

		// Use this for initialization
		private void Start()
		{
			_playerDialogue = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDialogue>();
			_playerDialogue.OnUpdate += UpdateUI;
			nextButton.onClick.AddListener(Next);
			quitButton.onClick.AddListener(Quit);

			UpdateUI();
		}

		private void Next()
		{
			_playerDialogue.Next();
		}

		private void Quit()
		{
			_playerDialogue.Quit();
		}

		private void UpdateUI()
		{
			gameObject.SetActive(_playerDialogue.GetDialogue != null);
			deco.SetActive(gameObject.activeSelf);

			if (_playerDialogue.GetDialogue == null) return;

			aiName.text = _playerDialogue.GetName();
			response.SetActive(!_playerDialogue.IsChoosing);
			choiceList.gameObject.SetActive(_playerDialogue.IsChoosing);

			if (_playerDialogue.IsChoosing)
			{
				CreateChoiceList();
			}
			else
			{
				questText.text = _playerDialogue.GetText();
				nextButton.gameObject.SetActive(_playerDialogue.HasNextText());
			}
		}

		private void CreateChoiceList()
		{
			foreach (Transform choice in choiceList)
			{
				Destroy(choice.gameObject);
			}
			foreach (var choice in _playerDialogue.GetChoices())
			{
				var choiceInstance = Instantiate(choicePrefab, choiceList);
				choiceInstance.GetComponentInChildren<TextMeshProUGUI>().text = choice.Text;

				var button = choiceInstance.GetComponentInChildren<Button>();
				button.onClick.AddListener(() => _playerDialogue.SelectChoice(choice));
			}
		}
	}
}
