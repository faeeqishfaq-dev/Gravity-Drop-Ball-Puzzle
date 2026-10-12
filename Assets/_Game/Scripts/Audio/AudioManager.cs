using UnityEngine;

namespace PhysicsDraw.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlaySound(AudioClip clip, float volume = 1f)
        {
            if (clip == null || !Save.SaveManager.Instance.CurrentData.soundEnabled) return;
            
            // Basic implementation
            GameObject obj = new GameObject("TempAudio");
            AudioSource source = obj.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.Play();
            Destroy(obj, clip.length + 0.1f);
        }
    }
}
