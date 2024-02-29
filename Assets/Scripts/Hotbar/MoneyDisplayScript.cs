using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(TextMeshProUGUI))]
public class MoneyDisplayScript : MonoBehaviour
{
    [SerializeField]
    private int player;
    private TextMeshProUGUI moneyText;

    // Start is called before the first frame update
    void Start()
    {
        moneyText = GetComponent<TextMeshProUGUI>();       
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$"+ResourceManager.Instance.GetMoney(player);
    }
}
