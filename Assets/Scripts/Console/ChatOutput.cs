using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;

public class ChatOutput : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ChatWindowText;

    //list of messages
    private Queue<string> queue = new Queue<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddMessage(string msg)
    {
        
        queue.Enqueue(msg);
        var builder = new StringBuilder();
        foreach(string st in queue)
        {
            builder.Append(st).Append("\n");
        }
        ChatWindowText.text = builder.ToString();
    }

    void ClearQueue()
    {
        queue.Clear();
    }
}
