using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamePrinter : MonoBehaviour
{

    [SerializeField] private string myName = "Pepeco";

    private void Awake()
    {
        myName = "Amélia";
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(myName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
