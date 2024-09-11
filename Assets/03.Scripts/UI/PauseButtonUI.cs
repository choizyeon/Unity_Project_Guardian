using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonUI : MonoBehaviour
{
    [SerializeField] GameObject m_PauseUI;
    bool isActive = false;
    
    public void PauseButton()
    {
        m_PauseUI.SetActive(true);
        GameManager.Instance.gameState = GameManager.GameState.pause;
    }

}
