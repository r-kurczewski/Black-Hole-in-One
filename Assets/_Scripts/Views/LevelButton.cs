using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(Button))]
public class LevelButton : MonoBehaviour
{
	[SerializeField]
	private TMP_Text text;

	[SerializeField]
	private int levelNumber;

	private void Start()
	{
		text.text = levelNumber.ToString();
		if (Application.isPlaying)
		{
			if (GameController.instance.LevelExists(levelNumber))
			{
				GetComponent<Button>().onClick.AddListener(() => GameController.instance.LoadScene(levelNumber));
			}
			else GetComponent<Button>().interactable = false;
		}
	}
}
