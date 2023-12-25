using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SA.Game
{
    public class AppBoot : MonoBehaviour
    {
        private async UniTaskVoid Start() 
        {
            await UniTask.WhenAll
            (
                SceneManager.LoadSceneAsync("Game").ToUniTask(),
                SceneManager.LoadSceneAsync("Environment", LoadSceneMode.Additive).ToUniTask(), 
                SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive).ToUniTask()
            );

            Debug.Log("Game loaded completed!");
        }
    }
}