using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
* HotbarSlot class
* Stores the current HotbarElement and renders the information into this slot
*/
public class HotbarSlot : MonoBehaviour
{
    private HotbarElement hotbarElement;
    [SerializeField]
    private GameObject slotBackground;
    [SerializeField]
    private GameObject slotIcon;
    // Start is called before the first frame update
    void Start()
    {   
        hotbarElement = null;
        GetSlotReferences();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void GetSlotReferences() {
        slotBackground = transform.Find("Slot Background").gameObject;
        slotIcon = transform.Find("Slot Icon").gameObject;
    }

    public void SetHotbarElement(HotbarElement hotbarElement)
    {
        if(!slotBackground || !slotIcon)
        {
            GetSlotReferences();
        }

        this.hotbarElement = hotbarElement;
        slotIcon.GetComponent<Image>().sprite = hotbarElement.hotbarElementObject.icon;
        
        //if is buliding set background to green, if unit set to purple
        switch(hotbarElement.hotbarElementObject.slotType)
        {
            case "Building":
                slotBackground.GetComponent<Image>().color = Color.green;
                break;
            case "Unit":
                slotBackground.GetComponent<Image>().color = Color.magenta;
                break;
            default:
                // log error unknown slot type
                Debug.LogError("Unknown slot type: " + hotbarElement.hotbarElementObject.slotType);
                break;
        }
    }
}
