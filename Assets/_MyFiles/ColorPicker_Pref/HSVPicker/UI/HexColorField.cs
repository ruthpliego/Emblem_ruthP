﻿using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

[RequireComponent(typeof(InputField))]
public class HexColorField : MonoBehaviour
{
    public ColorPicker hsvpicker;

    public bool displayAlpha;
    [HideInInspector]
    public InputField hexInputField;

    private const string hexRegex = "^#?(?:[0-9a-fA-F]{3,4}){1,2}$";



    private void Awake()
    {
        hexInputField = GetComponent<InputField>();
        hsvpicker = GameObject.Find("Picker 2.0").GetComponent<ColorPicker>();

        // Add listeners to keep text (and color) up to date
        hexInputField.onEndEdit.AddListener(UpdateColor);
        hsvpicker.onValueChanged.AddListener(UpdateHex);
    }

    private void OnDestroy()
    {
        hexInputField.onValueChanged.RemoveListener(UpdateColor);
        hsvpicker.onValueChanged.RemoveListener(UpdateHex);
    }

    private void UpdateHex(Color newColor)
    {
        hexInputField.text = ColorToHex(newColor);
    }

    private void UpdateColor(string newHex)
    {
        Color32 color;
        if (HexToColor(newHex, out color))
        {
            Debug.Log("Hex to Color conversion successful: " + color);
            hsvpicker.CurrentColor = color;
        }
        else
        {
            Debug.Log("Hex value is in the wrong format. Input: " + newHex);
            Debug.Log("Valid formats are: #RGB, #RGBA, #RRGGBB, #RRGGBBAA (# is optional)");
        }
    }

        private string ColorToHex(Color32 color)
    {
        if (displayAlpha)
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.r, color.g, color.b, color.a);
        else
            return string.Format("#{0:X2}{1:X2}{2:X2}", color.r, color.g, color.b);
    }

    public static bool HexToColor(string hex, out Color32 color)
    {
        // Check if this is a valid hex string (# is optional)
        if (System.Text.RegularExpressions.Regex.IsMatch(hex, hexRegex))
        {
            int startIndex = hex.StartsWith("#") ? 1 : 0;

            if (hex.Length == startIndex + 8) //#RRGGBBAA
            {
                color = new Color32(byte.Parse(hex.Substring(startIndex, 2), NumberStyles.AllowHexSpecifier),
                    byte.Parse(hex.Substring(startIndex + 2, 2), NumberStyles.AllowHexSpecifier),
                    byte.Parse(hex.Substring(startIndex + 4, 2), NumberStyles.AllowHexSpecifier),
                    byte.Parse(hex.Substring(startIndex + 6, 2), NumberStyles.AllowHexSpecifier));
            }
            else if (hex.Length == startIndex + 6)  //#RRGGBB
            {
                color = new Color32(byte.Parse(hex.Substring(startIndex, 2), NumberStyles.AllowHexSpecifier),
                    byte.Parse(hex.Substring(startIndex + 2, 2), NumberStyles.AllowHexSpecifier),
                    byte.Parse(hex.Substring(startIndex + 4, 2), NumberStyles.AllowHexSpecifier),
                    255);
            }
            else if (hex.Length == startIndex + 4) //#RGBA
            {
                color = new Color32(byte.Parse("" + hex[startIndex] + hex[startIndex], NumberStyles.AllowHexSpecifier),
                    byte.Parse("" + hex[startIndex + 1] + hex[startIndex + 1], NumberStyles.AllowHexSpecifier),
                    byte.Parse("" + hex[startIndex + 2] + hex[startIndex + 2], NumberStyles.AllowHexSpecifier),
                    byte.Parse("" + hex[startIndex + 3] + hex[startIndex + 3], NumberStyles.AllowHexSpecifier));
            }
            else  //#RGB
            {
                color = new Color32(byte.Parse("" + hex[startIndex] + hex[startIndex], NumberStyles.AllowHexSpecifier),
                    byte.Parse("" + hex[startIndex + 1] + hex[startIndex + 1], NumberStyles.AllowHexSpecifier),
                    byte.Parse("" + hex[startIndex + 2] + hex[startIndex + 2], NumberStyles.AllowHexSpecifier),
                    255);
            }
            return true;
        }
        else
        {
            color = new Color32();
            return false;
        }
    }
}
