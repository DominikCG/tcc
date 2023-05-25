using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountAux : MonoBehaviour
{
    private TMP_Text Count;
    private int Score = 0;
    // Start is called before the first frame update
    void Start()
    {
        Count = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopCount(int score){
        Score += score;
        Count.text =""+Score;
    }
}
