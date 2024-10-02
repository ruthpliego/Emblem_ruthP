using UnityEngine;
using UnityEngine.UI;

public class ColorPresets : MonoBehaviour
{
	public ColorPicker picker;
	public GameObject[] presets;
	public Image createPresetImage;

	void Awake()
	{
//		picker.onHSVChanged.AddListener(HSVChanged);
		picker.onValueChanged.AddListener(ColorChanged);
	}

	public void CreatePresetButton()
	{
		for (var i = 0; i < presets.Length; i++)
		{
			if (!presets[i].activeSelf)
			{
				presets[i].SetActive(true);
				presets[i].GetComponent<Image>().color = picker.CurrentColor;
				break;
			}
		}
	}

	public void PresetSelect(Image sender)
	{
		picker.CurrentColor = sender.color;
	}

	private void ColorChanged(Color color)
	{
		createPresetImage.color = color;
	}
}
