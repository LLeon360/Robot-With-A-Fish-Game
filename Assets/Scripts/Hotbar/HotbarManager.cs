using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class HotbarManager : MonoBehaviour
{
    [SerializeField]
    private GameObject hotbarSlotPrefab;
    [SerializeField] //for testing purposes, shouldn't need to assign through inspector
    private List<HotbarElementObject> hotbarElementObjects;

    private List<HotbarElement> hotbarElements;
    private List<GameObject> hotbarSlots;
    [SerializeField]
    private float hotbarSlotWidth = 80f;

    [SerializeField]
    private GameObject slotSelector;
    private int selectedSlot;
    // Start is called before the first frame update
    void Start()
    {
        hotbarElements = new List<HotbarElement>();
        hotbarSlots = new List<GameObject>();
        selectedSlot = 0;
        UpdateLists();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("DEBUG: REMOVE THIS, Pressing Space will rerender all hotbar elements");
        if(Input.GetKeyDown(KeyCode.Space)) //testing purposes, TODO remove
        {
            //rerender all
            UpdateLists();
        }
        UpdateSelection();
    }

    public void SetHotbarElementObjects(List<HotbarElementObject> hotbarElementObjects)
    {
        this.hotbarElementObjects = hotbarElementObjects;
        UpdateLists();
    }

    public void UpdateLists() {
        UpdateHotbarElements();
        RenderHotbar();
    }

    public void RemoveElement(int index)
    {
        hotbarElementObjects.RemoveAt(index);
    }

    public void AppendElement(HotbarElementObject hotbarElementObject)
    {
        hotbarElementObjects.Add(hotbarElementObject);
    }

    public void ClearHotbar()
    {
        hotbarElementObjects.Clear();
        UpdateLists();
    }

    void UpdateHotbarElements() {
        //clear old wrappers
        hotbarElements.Clear();
        //wrap HotbarElementObjects (ScriptableObject data) into HotbarElements
        foreach(HotbarElementObject hotbarElementObject in hotbarElementObjects) {
            hotbarElements.Add(new HotbarElement(hotbarElementObject));
        }
    }

    void RenderHotbar() {
        //clear old hotbar, destroy previous objects
        foreach(GameObject hotbarSlot in hotbarSlots) {
            Destroy(hotbarSlot);
        }
        hotbarSlots.Clear();
        for(int i = 0; i < hotbarElements.Count; i++) {
            GameObject hotbarSlot = Instantiate(hotbarSlotPrefab, Vector3.zero, Quaternion.identity);
            RectTransform hotbarSlotRectTransform = hotbarSlot.GetComponent<RectTransform>();
            //parent the slot to this hotbar
            hotbarSlot.transform.SetParent(this.transform);
            hotbarSlotRectTransform.localPosition = new Vector3(hotbarSlotWidth * i, 0, 0);
            hotbarSlotRectTransform.localScale = new Vector3(1, 1, 1);
            hotbarSlot.GetComponent<HotbarSlot>().SetHotbarElement(hotbarElements[i]);
            hotbarSlots.Add(hotbarSlot);
        }
    }

    public void OnCycle(InputAction.CallbackContext ctx) {
        int hotbarSize = hotbarSlots.Count;
        if(ctx.performed) {
            float scroll = ctx.ReadValue<float>();
            if(scroll > 0) {
                //cycle right
                selectedSlot = (selectedSlot + 1) % hotbarSize;
            }
            else if(scroll < 0) {
                //cycle left
                selectedSlot = (selectedSlot - 1 + hotbarSize) % hotbarSize;
            }
        }
    }

    public void UpdateSelection() {
        RectTransform slotSelectorRectTransform = slotSelector.GetComponent<RectTransform>();
        //lerp to selection
        Vector3 targetPosition = new Vector3(hotbarSlotWidth * selectedSlot, 0, 0);
        slotSelectorRectTransform.localPosition = Vector2.Lerp(slotSelectorRectTransform.localPosition, targetPosition, 
        Time.deltaTime * 15);
    }
}
