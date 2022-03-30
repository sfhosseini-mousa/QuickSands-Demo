using UnityEngine;

namespace Sands
{
    class HandPointerCreator : MonoBehaviour
    {
        GameObject instantiatedHandPointer;
        [SerializeField] GameObject handPointerPrefab;

        public void InstantiateHandPointer(Transform pos)
        {
            instantiatedHandPointer = Instantiate(handPointerPrefab, pos);
            instantiatedHandPointer.transform.localPosition = Vector3.zero;
        }

        public void DestroyHandPointer()
        {
            Destroy(instantiatedHandPointer);
        }
    }
}
