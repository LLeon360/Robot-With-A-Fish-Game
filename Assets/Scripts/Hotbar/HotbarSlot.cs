using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/**
* HotbarSlot class
* Stores the current HotbarElement and renders the information into this slot
*/
public class HotbarSlot : MonoBehaviour
{
    [SerializeField]
    private HotbarElement hotbarElement;
    [SerializeField]
    private GameObject slotBackground;
    [SerializeField]
    private GameObject slotIcon;
    [SerializeField]
    private GameObject cooldownOverlay;
    [SerializeField]
    private TextMeshProUGUI costOverlay;
    // Start is called before the first frame update
    void Start()
    {   
        GetSlotReferences();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(this.hotbarElement == null);
        if(hotbarElement != null)
        {
            if(hotbarElement.nextUsableTime <= Time.time)
            {
                cooldownOverlay.SetActive(false);
            }
            else
            {
                cooldownOverlay.SetActive(true);
                UpdateCooldownOverlay();
            }
        }
    }

    void UpdateCooldownOverlay()
    {
        float cooldownTimeRemaining = hotbarElement.nextUsableTime - Time.time;
        cooldownOverlay.GetComponent<Image>().fillAmount = cooldownTimeRemaining / hotbarElement.hotbarElementObject.cooldown;
        cooldownOverlay.GetComponent<Image>().color = Color.Lerp(new Color(1,1f,1f,0.7f), new Color(1,0.57f,0.57f,1f), cooldownTimeRemaining / hotbarElement.hotbarElementObject.cooldown);
    }
    
    void GetSlotReferences() {
        slotBackground = transform.Find("Slot Background").gameObject;
        slotIcon = transform.Find("Slot Icon").gameObject;
        cooldownOverlay = transform.Find("Cooldown Overlay").gameObject;
        costOverlay = transform.Find("Cost Overlay").GetComponent<TextMeshProUGUI>();
    }

    public void SetHotbarElement(HotbarElement hotbarElement)
    {
        if(!slotBackground || !slotIcon)
        {
            GetSlotReferences();
        }

        this.hotbarElement = hotbarElement;
        Debug.Log(this.hotbarElement);
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

        cooldownOverlay.SetActive(false);
        costOverlay.text = "$"+hotbarElement.hotbarElementObject.cost.ToString();
    }
}
