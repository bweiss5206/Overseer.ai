using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDown_Camera : MonoBehaviour
{
    #region Variables
    public Transform m_target;
    public float m_height = 10f;
    public float m_Distance = 20f;
    public float m_Angle = 45f;
    #endregion

    #region Main Methods
    // Start is called before the first frame update
    void Start()
    {
        HandleCamera();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCamera();
    }
    #endregion

    #region Helper Methods
    protected virtual void HandleCamera()
    {

    }
    #endregion
}
