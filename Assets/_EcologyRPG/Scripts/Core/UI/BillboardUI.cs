using UnityEngine;

namespace EcologyRPG._Core.UI
{
    public class BillboardUI : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
        }
    }

}