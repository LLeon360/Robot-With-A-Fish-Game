using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [SerializeField]
    private int _startingMoney;
    private List<int> _currentMoney;
    
    void Start()
    {
        _currentMoney = new List<int>();
    }
    void Awake()
    {
        // Singleton pattern, only one instance of this class should exist
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
