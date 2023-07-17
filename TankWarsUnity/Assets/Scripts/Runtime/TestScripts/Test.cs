using UnityEngine;

[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    private Camera renderingCamera = null; 

    #region Unity Methods 

    private void Start()
    {
        renderingCamera = GetComponent<Camera>();
        Debug.Log("This is a test."); 
    }

    #endregion
}
