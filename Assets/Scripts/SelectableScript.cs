using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableScript : MonoBehaviour
{
    private Material originalMaterial;
    private Renderer myRenderer;
    private bool hasRenderer = false;

    // State variable
    private bool _selected = false;
    private bool _pointed = false;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
        if (myRenderer != null)
        {
            originalMaterial = myRenderer.material;
            hasRenderer = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsSelected()
    {
        return _selected;
    }

    public bool IsPointed()
    {
        return _pointed;
    }

    public void HandleSelect(Material selectedMaterial)
    {
        _selected = true;
        if (hasRenderer)
        {
            myRenderer.material = selectedMaterial;
        }
    }

    public void HandleDeselect(Material highlightMaterial)
    {
        _selected = false;
        if (hasRenderer)
        {
            myRenderer.material = highlightMaterial;
        }
    }

    public void HandlePointed(Material highlightMaterial)
    {
        _pointed = true;
        if (!_selected && hasRenderer)
        {
            myRenderer.material = highlightMaterial;
        }
    }

    public void HandleUnpointed()
    {
        _pointed = false;
        if (!_selected && hasRenderer)
        {
            myRenderer.material = originalMaterial;
        }
    }
}
