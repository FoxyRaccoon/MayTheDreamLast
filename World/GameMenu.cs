using Godot;
using System.Collections.Generic;
using System;

public partial class GameMenu : Node2D
{
    int[,] A = {{0,1,0},{1,0,1},{1,1,1},{1,0,1},{1,0,1}};
    int[,] E = {{1,1,1},{1,0,0},{1,1,0},{1,0,0},{1,1,1}};
    int[,] D = {{1,1,0},{1,0,1},{1,0,1},{1,0,1},{1,1,0}};
    int[,] R = {{1,1,0},{1,0,1},{1,1,0},{1,0,1},{1,0,1}};
    int[,] Y = {{1,0,1},{1,0,1},{1,0,1},{0,1,0},{0,1,0}};
    int[,] L = {{1,0,0},{1,0,0},{1,0,0},{1,0,0},{1,1,1}};
    int[,] H = {{1,0,1},{1,0,1},{1,1,1},{1,0,1},{1,0,1}};
    int[,] T = {{1,1,1},{0,1,0},{0,1,0},{0,1,0},{0,1,0}};
    int[,] S = {{0,1,1},{1,0,0},{0,1,0},{0,0,1},{1,1,0}};
    int[,] M = {{1,0,1},{1,1,1},{1,0,1},{1,0,1},{1,0,1}};
    public override void _Ready()
    {
        GetNode<MusicListener>("/root/MusicListener").Play();
        string sentence = "MAY THE DREAM LAST";
        int[,] terrain = new int[5, sentence.Length*4];
        for(int i = 0; i < 5; i++){
            for(int j = 0; j < sentence.Length; j++){
                switch(sentence[j]){
                    case 'A':
                        for(int k = 0; k < 3; k++){
                            terrain[i, j*4 + k] =  A[i,k];
                        }
                        break;
                    case 'E':
                        for(int k = 0; k < 3; k++){
                            terrain[i, j*4 + k] =  E[i,k];
                        }
                        break;
                    case 'D':
                        for(int k = 0; k < 3; k++){
                            terrain[i, j*4 + k] =  D[i,k];
                        }
                        break;
                    case 'R':
                        for(int k = 0; k < 3; k++){
                            terrain[i, j*4 + k] =  R[i,k];
                        }
                        break;
                    case 'Y':
                        for(int k = 0; k < 3; k++){
                            terrain[i, j*4 + k] =  Y[i,k];
                        }
                        break;
                    case 'L':
                        for(int k = 0; k < 3; k++){
                            terrain[i, j*4 + k] =  L[i,k];
                        }
                        break;
                    case 'H':
                        for(int k = 0; k < 3; k++){
                            terrain[i, j*4 + k] =  H[i,k];
                        }
                        break;
                    case 'T':
                        for(int k = 0; k < 3; k++){
                            terrain[i, j*4 + k] =  T[i,k];
                        }
                        break;
                    case 'S':
                        for(int k = 0; k < 3; k++){
                            terrain[i, j*4 + k] =  S[i,k];
                        }
                        break;
                    case 'M':
                        for(int k = 0; k < 3; k++){
                            terrain[i, j*4 + k] =  M[i,k];
                        }
                        break;
                    default:
                        for(int k = 0; k < 3; k++){
                            terrain[i, j*4 + k] =  0;
                        }
                        break;
                }
                terrain[i, j*4 + 3] = 0;
            }
        }

        PackedScene wallScene = (PackedScene)ResourceLoader.Load("res://World/wall.tscn");

        for(int i = 0; i < 5; i++){
			for(int j = sentence.Length*4-1; j >= 0; j-= 1 ){
				if(terrain[i,j] == 1){
                    Wall instance = (Wall)wallScene.Instantiate();
                    instance.GlobalPosition = new Vector2(j*20, i*20) - new Vector2(sentence.Length*4*10, 6*20);
                    GetNode("Terrain").AddChild(instance);
				}
			}
		}

    }
}
