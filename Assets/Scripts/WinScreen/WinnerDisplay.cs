using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class WinnerDisplay : MonoBehaviour
{
    public TextMeshProUGUI winnerText;
    // Start is called before the first frame update
    void Start()
    {
        winnerText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        winnerText.text = WinState.Winner + " Wins!";
    }
}
