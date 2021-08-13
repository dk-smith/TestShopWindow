using UnityEngine;

namespace UI
{
    public class GameWindow : MonoBehaviour
    {
        public virtual void Close()
        {
            Destroy(gameObject);
        }
    }
}