using TMPro;
using UnityEngine;

public class BallInfoView : MonoBehaviour
{
	private const string HIT_LABEL = "Hits: ";
	private const string POWER_LABEL = "Power: ";

	[SerializeField]
	private TMP_Text hitCountLabel;

	[SerializeField]
	private TMP_Text speedLabel;

	public void SetHitCounter(int hitCount)
	{
		hitCountLabel.text = HIT_LABEL + hitCount;
	}

	public void ResetHitCounter()
	{
		SetHitCounter(0);
	}
	public void SetPower(float power)
	{
		speedLabel.text = POWER_LABEL + power.ToString("0.00");
	}
}
