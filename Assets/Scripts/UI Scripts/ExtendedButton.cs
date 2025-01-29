
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExtendedButton : Button
{
    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    protected override void OnValidate()
    {
        var text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = gameObject.name;
        base.OnValidate();
    }   
}
