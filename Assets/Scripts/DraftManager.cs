using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftManager : MonoBehaviour
{
    public List<HotbarElementObject> selectionElements;
    public static List<List<HotbarElementObject>> playerDrafts;

    private List<GameObject> selectionSlots;
    private List<int> selectedSlots;
    
    [SerializeField]
    private int draftSize = 8;

    [SerializeField]
    private int rowSize = 10;
    public int RowSize { get { return rowSize; } }
    [SerializeField]
    private int slotSize = 0;
    
    [SerializeField]
    private GameObject hotbarSlotPrefab;

    public static bool initialized = false;

    //Singleton pattern
    public static DraftManager Instance { get; private set; }

    [SerializeField]
    private List<GameObject> playerSelectors;
    [SerializeField]
    private List<GameObject> playerHotbars;

    // Start is called before the first frame update
    void Start()
    {
        initialized = false;

        selectionSlots = new List<GameObject>();
        //size 2
        List<int> selectedSlots = new List<int> { 0, 0 };

        List<HotbarElementObject> player1Draft = new List<HotbarElementObject>();
        List<HotbarElementObject> player2Draft = new List<HotbarElementObject>();
        
        playerDrafts = new List<List<HotbarElementObject>>();
        playerDrafts.Add(player1Draft);
        playerDrafts.Add(player2Draft);
        RenderSelections();
        initialized = true;
    }
    
    void Awake()
    {
        // Singleton pattern, only one instance of this class should exist
        // Retains the new instance of the class and destroys the old one, to maintain references within a scene
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if both drafts are full, allow the game to start
        if(playerDrafts[0].Count == draftSize && playerDrafts[1].Count == draftSize) {
            FinishDraft();
        }
    }

    public void AddUnitToDraft(int player, HotbarElementObject unit)
    {
        playerDrafts[player].Add(unit);
    }

    public void RemoveUnitFromDraft(int player, int index)
    {
        playerDrafts[player].RemoveAt(index);
    }

    public void ClearDraft(int player)
    {
        playerDrafts[player].Clear();
    }

    public void ClearDrafts()
    {
        playerDrafts[0].Clear();
        playerDrafts[1].Clear();
    }

    public List<HotbarElementObject> GetDraft(int player)
    {
        return playerDrafts[player];
    }

    public bool SelectElement(int player, int index) {
        //check if player's draft has already selected this element
        if(playerDrafts[player].Contains(selectionElements[index])) {
            return false;
        }
        playerDrafts[player].Add(selectionElements[index]);

        //swap to next players turn
        playerSelectors[player].GetComponent<DraftSelector>().EndTurn();
        playerSelectors[(player+1)%2].GetComponent<DraftSelector>().StartTurn();

        //update hotbars visually
        playerHotbars[player].GetComponent<HotbarManager>().SetHotbarElementObjects(playerDrafts[player]);

        return true;
    }

    void RenderSelections() {
        //clear old selections, destroy previous objects
        foreach(GameObject slot in selectionSlots) {
            Destroy(slot);
        }
        selectionSlots.Clear();
        for(int i = 0; i < selectionElements.Count; i++) {
            GameObject hotbarSlot = Instantiate(hotbarSlotPrefab, Vector3.zero, Quaternion.identity);
            RectTransform hotbarSlotRectTransform = hotbarSlot.GetComponent<RectTransform>();
            //parent the slot to this hotbar
            hotbarSlot.transform.SetParent(this.transform);
            float xOffset = i % rowSize - rowSize / 2;
            float yOffset = - i / rowSize + 1 ;
            hotbarSlotRectTransform.localPosition = new Vector3(xOffset * slotSize, yOffset * slotSize, 0);
            hotbarSlotRectTransform.localScale = new Vector3(1, 1, 1);
            hotbarSlot.GetComponent<HotbarSlot>().SetHotbarElement(new HotbarElement(selectionElements[i])); // not ideal, could make a custom display slot that doesn't carry all the HotbarElement logic
            selectionSlots.Add(hotbarSlot);
        }
    }

    public HotbarElementObject GetSelectionElement(int index) {
        return selectionElements[index];
    }

    public GameObject GetSelectionSlot(int index)
    {
        return selectionSlots[index];
    }

    public void FinishDraft() {
        SceneController.Instance.LoadScene("GameScene");
    }
}

