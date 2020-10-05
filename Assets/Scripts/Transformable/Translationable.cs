using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translationable : MonoBehaviour
{

    private SelectableScript mySelectableScript;
    private Transform myTransform;
    // History


    // Start is called before the first frame update
    void Start()
    {
        mySelectableScript = gameObject.GetComponent<SelectableScript>();
        myTransform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandleTranslation(Vector3 v)
    {
        if (mySelectableScript.IsSelected())
        {
            myTransform.Translate(v);
        }
    }
}
