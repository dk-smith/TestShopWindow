using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

public class Initer : MonoBehaviour
{
    [SerializeField] private Game game;

    private void Awake()
    {
        if (!game)
        {
            Resources.LoadAll<Game>("");
            game = Resources.FindObjectsOfTypeAll<Game>().FirstOrDefault();
            
            if (!game)
            {
                Application.Quit();
                return;
            }
        }
        
        game.Init();
        
        Destroy(gameObject);
    }
}
