using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Panel
{
    public string Name;
    public CanvasGroup FadePanel;
    public bool Fade;
    public float Speed;

}

public class UIController : MonoBehaviour
{
    [SerializeField] private List <Panel> _panels;
    [SerializeField] private float _fadeAmount;

    void OnValidate()
    {
        foreach (var panel in _panels)
        {
            panel.Name = panel.FadePanel.name;
        }
    } 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPanels();
        
    }

    public void CheckPanels()
    {
        foreach (var panel in _panels)
        {
            /* if (panel.Fade)
            {
                FadePanel(panel.FadePanel, panel.Fade, panel.Speed);
            } */

            FadePanel(panel.FadePanel, panel.Fade, panel.Speed);
        }
    }

    public void FadePanel(CanvasGroup panel, bool fade, float speed)
    {
        if(fade)
        {
            panel.alpha = Mathf.MoveTowards(panel.alpha, 0 , speed * Time.deltaTime);
        }

        else
        {
            panel.alpha = Mathf.MoveTowards(panel.alpha, 1 , speed * Time.deltaTime);
        }

        _fadeAmount = panel.alpha;
    }
    
}
