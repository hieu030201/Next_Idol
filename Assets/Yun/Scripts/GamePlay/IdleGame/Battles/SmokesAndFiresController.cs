using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Yun.Scripts.GamePlay.IdleGame.Battles
{
    public class SmokesAndFiresController : MonoBehaviour
    {
        private List<GameObject> _smokesList;
        private List<GameObject> _firesList;
        private void Awake()
        {
            _smokesList = new List<GameObject>();
            if (transform.Find("Smokes_Container"))
            {
                for (var i = 0; i < transform.Find("Smokes_Container").childCount; i++)
                {
                    _smokesList.Add(transform.Find("Smokes_Container").GetChild(i).gameObject);
                    transform.Find("Smokes_Container").GetChild(i).gameObject.transform.localScale = Vector3.zero;
                }
            }
            
            _firesList = new List<GameObject>();
            if (transform.Find("Fires_Container"))
            {
                for (var i = 0; i < transform.Find("Fires_Container").childCount; i++)
                {
                    _firesList.Add(transform.Find("Fires_Container").GetChild(i).gameObject);
                    transform.Find("Fires_Container").GetChild(i).gameObject.transform.localScale = Vector3.zero;
                }
            }
        }

        public void UpdateProgress(float progress, bool isImmediately = false)
        {
            var duration = isImmediately ? 0 : .5f;
            foreach (var smoke in _smokesList)
            {
                smoke.transform.DOKill();
                if(smoke.GetComponent<SmokesAndFiresStatus>())
                    smoke.transform.DOScale(smoke.GetComponent<SmokesAndFiresStatus>().endScale * progress, duration);
                else
                    smoke.transform.DOScale(progress, duration);
            }
            
            foreach (var fire in _firesList)
            {
                fire.transform.DOKill();
                if(fire.GetComponent<SmokesAndFiresStatus>())
                    fire.transform.DOScale(fire.GetComponent<SmokesAndFiresStatus>().endScale * progress, duration);
                else
                    fire.transform.DOScale(progress, duration);
            }
        }

        public void Stop()
        {
            foreach (var smoke in _smokesList)
            {
                if(smoke.GetComponent<ParticleSystem>())
                    smoke.GetComponent<ParticleSystem>().Stop();
            }
            
            foreach (var fire in _firesList)
            {
                if(fire.GetComponent<ParticleSystem>())
                    fire.GetComponent<ParticleSystem>().Stop();
            }
        }
    }
}
