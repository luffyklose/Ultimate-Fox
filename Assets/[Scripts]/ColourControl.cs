////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: ColourControl.cs
//Author: Zihan Xu
//Student Number: 101288760
//Last Modified On : 12/10/2021
//Description : Class for color control
//Revision History:
//12/10/2021: Implement feature of coloring special platform
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColourControl : MonoBehaviour
{
    public Color color;

    private List<SpriteRenderer> renderers;

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>().ToList();
        ChangeColour();
    }

    //Change color to default color by modifying renderer
    private void ChangeColour()
    {
        foreach (var renderer in renderers)
        {
            renderer.material.SetColor("_Color", color);
        }
    }

    //Change color to new color by modifying renderer
    public void ChangeColour(Color color  = new Color())
    {
        foreach (var renderer in renderers)
        {
            renderer.material.SetColor("_Color", color);
        }
    }
}
