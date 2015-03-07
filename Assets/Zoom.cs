using UnityEngine;

namespace Assets
{
    public class Zoom : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void LateUpdate()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll < 0) // Zoom in
            {
                Camera.main.orthographicSize = Camera.main.orthographicSize * 1.1f;
            }
            else if (scroll > 0) // zoom out
            {
                Camera.main.orthographicSize = Camera.main.orthographicSize / 1.1f;
            }
        }
    }
}
