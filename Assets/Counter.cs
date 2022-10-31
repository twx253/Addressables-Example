using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Counter : MonoBehaviour
{
    float timer;
    public TextMeshProUGUI counter;
    int count = 0;
    void Start()
    {
        counter.text = count.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            timer -= 1;
            count++;
            counter.text = count.ToString();
        }
    }
}
