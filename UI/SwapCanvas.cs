namespace HSA.RehaGame.UI
{
    using UnityEngine;

    public class SwapCanvas : MonoBehaviour
    {
        public GameObject visibleCanvas;
        public GameObject invisibleCanvas;

        private Canvas canvas1;
        private Canvas canvas2;
        
        public void Start()
        {
            canvas1 = visibleCanvas.GetComponent<Canvas>();
            canvas2 = invisibleCanvas.GetComponent<Canvas>();

            canvas1.gameObject.SetActive(true);
            canvas2.gameObject.SetActive(false);
        }

        public void SwapVisibility()
        {
            if(canvas1.isActiveAndEnabled)
            {
                canvas2.gameObject.SetActive(true);
                canvas1.gameObject.SetActive(false);
            }
            else
            {
                canvas1.gameObject.SetActive(true);
                canvas2.gameObject.SetActive(false);
            }
        }
    }
}