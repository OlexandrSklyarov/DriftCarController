using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SA.Game
{
    public static class MonoComponentExtensions
    {
        public static async UniTask<T> FindComponentAsync<T>() where T : MonoBehaviour
        {
            T comp = null;

            while (comp == null)
            {                
                comp = Object.FindAnyObjectByType<T>();     
                await UniTask.DelayFrame(1);           
            }
            return comp;
        }
    }
}