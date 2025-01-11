
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TempleCode.Popup_SoundManager.Generic
{
    public class GlobalSetting : MMSingleton<GlobalSetting>
    {
        protected override void Awake()
        {
            if (GlobalSetting.Instance != null)
            {
                if (GlobalSetting.Instance != this)
                {
                    Destroy(gameObject);
                    return;
                }

            }

            base.Awake();
            DontDestroyOnLoad(gameObject);

        }
        
        public static bool NetWorkRequirements()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
        
  

    }
}