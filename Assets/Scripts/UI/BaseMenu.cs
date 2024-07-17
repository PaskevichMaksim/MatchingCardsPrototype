using UnityEngine;
namespace UI
{
    public class BaseMenu : MonoBehaviour
    {
        public virtual void Awake()
        {
            Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
