using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [SerializeField]
    private int _startingMoney;
    [SerializeField] //for debugging purposes
    private List<int> _currentMoney;
    
    [SerializeField]
    private int passiveIncome;

    [SerializeField]
    private float incomeCooldown;
    private float nextIncomeTime;

    void Start()
    {
        _currentMoney = new List<int>();

        // Initialize the money for each player
        for(int i = 0; i < 2; i++)
        {
            _currentMoney.Add(_startingMoney);
        }
    }
    void Awake()
    {
        // Singleton pattern, only one instance of this class should exist
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
        if(Time.time > nextIncomeTime)
        {
            nextIncomeTime = Time.time + incomeCooldown;
            for(int i = 0; i < 2; i++)
            {
                AddMoney(i, passiveIncome);
            }
        }
    }

    public void SetStartingMoney(int startingMoney)
    {
        _startingMoney = startingMoney;
        for(int i = 0; i < _currentMoney.Count; i++)
        {
            _currentMoney[i] = startingMoney;
        }
    }

    public void AddMoney(int player, int amount)
    {
        _currentMoney[player] += amount;
    }

    public void RemoveMoney(int player, int amount)
    {
        _currentMoney[player] -= amount;
    }

    public int GetMoney(int player)
    {
        return _currentMoney[player];
    }

    public void SetMoney(int amount, int player)
    {
        _currentMoney[player] = amount;
    }

    public bool CanAfford(int amount, int player)
    {
        return _currentMoney[player] >= amount;
    }
}
