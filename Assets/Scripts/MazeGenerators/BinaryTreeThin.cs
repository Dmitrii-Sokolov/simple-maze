﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://habrahabr.ru/post/320140/
public class BinaryTreeThin : IMazeGenerator
{
    public void Generate() { while (NextStep()) ; }
    private IMaze maze;

    public void Init(IMaze TargetMaze)
    {
        maze = TargetMaze;
        Init();
    }

    public void Init()
    {
        maze.CurrentCell = new Vector2Int(0, 0);
    }

    private static List<Vector2Int> shiftsSimple = new List<Vector2Int>()
    {
        Vector2Int.right, Vector2Int.up
    };

    public bool NextStep()
    {
        maze.SetPass(maze.CurrentCell, true);
        var choices = new List<Vector2Int>();

        foreach (var item in shiftsSimple)
        {
            var adj = maze.CurrentCell + item;
            if (maze.InMaze(adj))
                if (!maze.GetPass(adj))
                    choices.Add(adj);
        }

        if (choices.Count == 0)
            return false;
        else
        {
            var index = Random.Range(0, choices.Count);
            maze.SetTunnel(maze.CurrentCell, choices[index], true);
            maze.CurrentCell += shiftsSimple[0];
            if (!maze.InMaze(maze.CurrentCell))
                maze.CurrentCell = new Vector2Int(0, maze.CurrentCell.y + 1);
        }

        return true;
    }
}
