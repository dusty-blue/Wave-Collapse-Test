using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Build : MonoBehaviour
{
    // Start is called before the first frame update
    Transform m_t;
    private Vector3 m_v;
    void Start()
    {
        m_t = GetComponent<Transform>();
        m_v = new Vector3(1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        m_t.position += m_v *Time.deltaTime ;
    }
}
