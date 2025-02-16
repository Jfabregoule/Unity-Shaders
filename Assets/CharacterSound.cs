using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CharacterSound : MonoBehaviour
{
    public void PlayFootstep()
    {
        SoundManager.Instance.PlaySoundFXClipByKey("Character Footsteps", transform.position, Random.Range(0.6f, 1f), Random.Range(0.8f, 1.2f));
    }

    public void PlayDashSound()
    {
        SoundManager.Instance.PlayAllSoundFXClipsByKey("Character Dash", transform.position, Random.Range(0.6f, 1f), Random.Range(0.8f, 1.2f));
    }

    public void PlayAttackSound()
    {
        SoundManager.Instance.PlayAllSoundFXClipsByKey("Character Attack", transform.position, Random.Range(0.6f, 1f), Random.Range(0.8f, 1.2f));
    }

    public void PlayLongAttackSound()
    {
        //SoundManager.Instance.PlayAllSoundFXClipsByKey("Character Longattack", transform.position, Random.Range(0.6f, 1f), Random.Range(0.8f, 1.2f));
        SoundManager.Instance.PlaySoundFXClipByKey("Character Longattack Explosion", transform.position, Random.Range(0.6f, 1f), Random.Range(0.8f, 1.2f));
        SoundManager.Instance.PlaySoundFXClipByKey("Character Longattack Snor", transform.position, Random.Range(0.6f, 1f), Random.Range(0.8f, 1.2f));
    }

    public void PlaySkillSound()
    {
        SoundManager.Instance.PlaySoundFXClipByKey("Character Skill Snor", transform.position, Random.Range(0.6f, 1f), Random.Range(0.8f, 1.2f));
        //SoundManager.Instance.PlayAllSoundFXClipsByKey("Character Skill", transform.position, Random.Range(0.6f, 1f), Random.Range(0.8f, 1.2f));
    }

    public void PlayUltSound()
    {
        SoundManager.Instance.PlayAllSoundFXClipsByKey("Character Ult", transform.position, Random.Range(0.6f, 1f), Random.Range(0.8f, 1.2f));
    }
}
