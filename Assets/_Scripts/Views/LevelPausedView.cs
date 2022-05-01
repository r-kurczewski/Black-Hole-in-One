using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LevelPausedView : MonoBehaviour
{
	public Button returnButton;
	public Button restartLevelButton;
	public Button menuButton;

	public void Start()
	{
		returnButton.onClick.AddListener(() => LevelController.instance.DisableGamePause());
		restartLevelButton.onClick.AddListener(() =>
		{
			LevelController.instance.DisableGamePause();
			LevelController.instance.RestartLevel();
		});
		menuButton.onClick.AddListener(() => GameController.instance.LoadMenu());
	}
}

