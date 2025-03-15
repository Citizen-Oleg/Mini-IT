using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _main;
    [SerializeField]
    private AudioSource _audioSource;
    
    private ParticleSpawner _fxSpawner;
    private bool _isStarPlay;

    public void Initialize(ParticleSpawner particleSpawner)
    {
        _fxSpawner = particleSpawner;
    }

    public void Play()
    {
        gameObject.SetActive(true);
        _isStarPlay = true;
        _main.Play();
        _audioSource.Play();
    }

    private void Update()
    {
        if (_isStarPlay && !_main.isPlaying && gameObject.activeSelf)
        {
            _fxSpawner.Release(this);
        }
    }
}
