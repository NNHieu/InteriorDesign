using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableScript : MonoBehaviour
{
    

    private Material originalMaterial;
    private Renderer myRenderer;
    private bool hasRenderer = false;
    private SelectionManager selectionManager;
    // State variable
    private bool _selected = false;
    private bool _pointed = false;


    protected Outline highlightOutline;

    // Start is called before the first frame update
    protected void Start()
    {
        selectionManager = FindObjectOfType<SelectionManager>();

        myRenderer = gameObject.GetComponent<Renderer>();
        if (myRenderer != null)
        {
            originalMaterial = myRenderer.material;
            hasRenderer = true;
        }

        highlightOutline = gameObject.AddComponent<Outline>();

        highlightOutline.OutlineMode = Outline.Mode.OutlineAll;
        highlightOutline.enabled = false;

        highlightOutline.OutlineColor = Color.yellow;
        highlightOutline.OutlineWidth = selectionManager.outlineWidth;
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

    public void HandleSelect()
    {
        _selected = true;
        if (hasRenderer)
        {
            highlightOutline.OutlineColor = Color.red;
        }
    }

    public void HandleDeselect()
    {
        _selected = false;
        if (hasRenderer)
        {
            highlightOutline.OutlineColor = Color.yellow;
        }
    }

    public void HandlePointed()
    {
        _pointed = true;
        if (!_selected && hasRenderer)
        {
            //myRenderer.material = highlightMaterial;
            highlightOutline.enabled = true;

        }
    }

    public void HandleUnpointed()
    {
        _pointed = false;
        if (!_selected && hasRenderer)
        {
            //myRenderer.material = originalMaterial;
            highlightOutline.enabled = false;

        }
    }
}
