using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    

    private Material originalMaterial;
    private Renderer myRenderer;
    private bool hasRenderer = false;
    private SelectionManager selectionManager;
    private MainSystem mainSystem;
    // State variable
    private bool _selected = false;
    private bool _pointed = false;


    protected Outline highlightOutline;

    // Start is called before the first frame update
    protected void Start()
    {
        gameObject.tag = "Selectable";
        if(gameObject.GetComponent<Collider>() == null)
            gameObject.AddComponent<MeshCollider>();

        mainSystem = FindObjectOfType<MainSystem>();
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
        highlightOutline.OutlineWidth = 5f ; //selectionManager.outlineWidth;
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

    public void HandleSelect()
    {
        _selected = true;
        if (hasRenderer)
        {
            highlightOutline.OutlineColor = Color.red;
        }
        if (mainSystem != null)
        {
            mainSystem.addToSelectedObjects(this);
        }
    }

    public void HandleDeselect()
    {
        _selected = false;
        if (hasRenderer)
        {
            highlightOutline.OutlineColor = Color.yellow;
        }
        if (mainSystem != null)
        {
            mainSystem.removeFromSelectedObjects(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HandlePointed();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HandleUnpointed();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_selected) HandleSelect();
        else HandleDeselect();
    }

    private void OnMouseEnter()
    {
        HandlePointed();
    }

    private void OnMouseExit()
    {
        HandleUnpointed();
    }

    private void OnMouseDown()
    {
        if (!_selected) HandleSelect();
        else HandleDeselect();
    }
}
