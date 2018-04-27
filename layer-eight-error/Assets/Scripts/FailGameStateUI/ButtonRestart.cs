using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonRestart
    : MonoBehaviour
{
    private void Update()
    {
        if ( Input.GetMouseButtonDown( MouseButtonIndex.Left ) )
        {
            RaycastHit hit;
            if ( MathUtil.RaycastFromMousePointer( out hit ) )
            {
                // hitting myself with raycast
                if ( hit.collider.gameObject == gameObject )
                {
                    SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
                }
            }
        }
    }
}
