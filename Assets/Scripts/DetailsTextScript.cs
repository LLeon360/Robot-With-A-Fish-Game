using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class DetailsTextScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI detailsText;

    public string details;
    // Start is called before the first frame update
    void Start()
    {
        if(detailsText == null)
        {
            detailsText = transform.Find("Details Text").GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(detailsText != null)
        {
            detailsText.text = details;
        }
    }
}
