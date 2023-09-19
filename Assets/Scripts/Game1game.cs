using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Game1game : NetworkBehaviour
    {
        public Player playerHostPrefab;
        public Player playerClientPrefab;
        public Camera arenaCamera;

        private int positionIndex = 0;
        private Vector3[] startPositions = new Vector3[]
        {
            new Vector3(4, 10, 0),
            new Vector3(-4, 10, 0),
            new Vector3(0, 10, 4),
            new Vector3(0, 10, -4)
        };

        private int colorIndex = 0;
        private Color[] playerColors = new Color[] 
        {
            Color.blue,
            Color.green,
            Color.yellow,
            Color.magenta,
        };
    
        private Color NextColor() 
        {
            Color newColor = playerColors[colorIndex];
            colorIndex += 1;
            if (colorIndex > playerColors.Length - 1) 
            {
                colorIndex = 0;
            }
            return newColor;
        }

        void Start()
        {
            arenaCamera.enabled = !IsClient;
            arenaCamera.GetComponent<AudioListener>().enabled = !IsClient;
            if(IsServer)
            {
                SpawnPlayers();
            }
        }

        private Vector3 NextPosition() 
        {
            Vector3 pos = startPositions[positionIndex];
            positionIndex += 1;
            if (positionIndex > startPositions.Length - 1) 
            {
                positionIndex = 0;
            }
            return pos;
        }

        private void SpawnPlayers()
        {
            foreach(ulong clientId in NetworkManager.ConnectedClientsIds)
            {
                if(IsHost){
                Player playerHostSpawn = Instantiate(playerHostPrefab, Vector3.zero, Quaternion.identity);
                playerHostSpawn.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
                playerHostSpawn.playerColorNetVar.Value = NextColor();
                }
                else{
                Player playerClientSpawn = Instantiate(playerClientPrefab, Vector3.zero, Quaternion.identity);
                playerClientSpawn.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
                playerClientSpawn.playerColorNetVar.Value = NextColor();
                }
            }
        }
    }