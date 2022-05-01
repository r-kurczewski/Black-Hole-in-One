using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	private const int _menuSceneIndex = 0;
	public const string LEVEL_SCENE_PREFIX = "Level";

	public static GameController instance;

	public int MenuSceneIndex => _menuSceneIndex;

	public int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;

	public bool LevelExists(int level) => SceneUtility.GetBuildIndexByScenePath(LEVEL_SCENE_PREFIX + level) != -1;

	public bool NextLevelExists => LevelExists(CurrentSceneIndex + 1);

	private void Awake()
	{
		if (instance is null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	private void OnDestroy()
	{
		if (instance == this) instance = null;
	}

	public void LoadNextLevel()
	{
		LoadScene(CurrentSceneIndex + 1);
	}

	public void RestartLevel()
	{
		LoadScene(CurrentSceneIndex);
	}

	public void LoadMenu()
	{
		LoadScene(MenuSceneIndex);
	}

	public void LoadScene(int levelIndex)
	{
		SceneManager.LoadScene(levelIndex);
	}


}
