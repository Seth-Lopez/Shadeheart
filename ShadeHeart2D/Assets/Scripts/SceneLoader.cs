using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTransition;

public class SceneLoader : MonoBehaviour
{
    public TransitionSettings[] transitions;
    public TransitionSettings transition;
    public float loadDelay;
    public int transitionIndex;

    public void LoadScene(string _sceneName)
    {
        TransitionManager.Instance().Transition(_sceneName, transition, loadDelay);
    }
    public void LoadBattle(string _sceneName)
    {
        transitionIndex = Random.Range(0, transitions.Length);
        TransitionManager.Instance().Transition(_sceneName, transitions[transitionIndex], loadDelay);
    }
}
