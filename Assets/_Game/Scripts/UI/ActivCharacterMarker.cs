using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivCharacterMarker : MonoBehaviour
{
    [SerializeField] GameObject Body;
    public void AttachTo(CharacterView characterView)
    {
        Body.SetActive(true);
        transform.position = characterView.transform.parent.position;
    }
    public void Hide(){
        Body.SetActive(false);
    }
}
