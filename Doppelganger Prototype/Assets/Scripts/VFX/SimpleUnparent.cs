using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleUnparent : MonoBehaviour
{
    private Transform lastParent;
    [SerializeField] private Transform actualParent;

    protected virtual void UnparentAnimEvent()
    {
        lastParent = this.gameObject.transform.parent;
        actualParent.position = lastParent.position;
        actualParent.rotation = lastParent.rotation;
        this.gameObject.transform.SetParent(actualParent,false);
    }

    protected virtual void ParentAnimEvent()
    {
        this.gameObject.transform.SetParent(lastParent,false);
    }
}
