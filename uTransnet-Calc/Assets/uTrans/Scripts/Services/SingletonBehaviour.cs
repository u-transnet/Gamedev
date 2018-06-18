namespace uTrans.Services
{
    using UnityEngine;

    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T instance;

        public void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Duplicate subclass of type " + typeof(T) + "! eliminating " + name + " while preserving " + instance.name);
                Destroy(gameObject);
            }
            else
            {
                instance = this as T;
                Init();
            }
        }

        public void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }

        protected virtual void Init()
        {

        }
    }
}