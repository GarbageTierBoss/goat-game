using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachyShit : MonoBehaviour
{
    int _wholeNumber = 5;           //integer
    float _decimalNumber = 5.32f;
    bool _trueFalse = true;         //boolean
    char _singleLetter = 'r';       //character
    string _fullFckingSentence = "Full fucking sentence";   //full sentences

    void Awake()
    {
        _wholeNumber = Method(6);
        
        _decimalNumber = 65.2f;

        _fullFckingSentence = "New ass sentence";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int Method(int temp)
    {
        return temp / 3;
    }
    
}
