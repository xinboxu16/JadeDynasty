using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryObject : MonoBehaviour {

	public void SetVisible(int visible)
    {
        Renderer[] renders = gameObject.GetComponentsInChildren<Renderer>();
        int len = renders.Length;
        for(int i = 0; i < len; ++i)
        {
            if(visible == 0)
            {
                renders[i].enabled = false;
            }
            else
            {
                renders[i].enabled = true;
            }
        }
    }

    public void PlayAnimation(object[] args)
    {
        if (args.Length < 2) return;
        string animName = (string)args[0];
        float speed = (float)args[1];

        Animation animation = gameObject.GetComponent<Animation>();
        if (null != animation)
        {
            if (null != animation[animName])
            {
                animation[animName].speed = speed;
                animation.Play(animName);
            }else
            {
                Debug.LogError(string.Format("StoryObject PlayAnimation: object {0} can't find anim {1}", gameObject.name, animName));
            }
        }
    }

    public void PlayParticle()
    {
        ParticleSystem[] pss = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem ps in pss)
        {
            ps.Play();
        }
    }

    public void StopParticle()
    {
        ParticleSystem[] pss = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in pss)
        {
            ps.Stop();
        }
    }

    public void PlaySound(int index)
    {
        AudioSource[] audios = gameObject.GetComponents<AudioSource>();
        int len = audios.Length;
        if (null != audios && index >= 0 && index < len)
        {
            audios[index].Play();
        }
    }

    public void StopSound(int index)
    {
        AudioSource[] audios = gameObject.GetComponents<AudioSource>();
        if (null != audios && index >= 0 && index < audios.Length)
        {
            audios[index].Stop();
        }
    }

}
