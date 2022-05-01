using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedView : MonoBehaviour
{
	private const string HITS_LABEL = "Hits: ";

	public Button nextLevelButton;
	public Button restartLevelButton;
	public Button menuButton;

	public TMP_Text hitsLabel;

	public void Start()
	{
		var controller = GameController.instance;
		if (GameController.instance.NextLevelExists)
		{
			nextLevelButton.onClick.AddListener(() => controller.LoadNextLevel());
		}
		else
		{
			nextLevelButton.interactable = false;
		}
		restartLevelButton.onClick.AddListener(() =>
		{
			LevelController.instance.DisableGamePause();
			controller.RestartLevel();
		});
		menuButton.onClick.AddListener(() => controller.LoadMenu());
	}

	public void SetHits(int hitCount)
	{
		hitsLabel.text = HITS_LABEL + hitCount;
	}
}

