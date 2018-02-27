﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://habrahabr.ru/post/321210/
public class AldousBroderThin : IMazeGenerator
{
    public void Generate() { while (NextStep()) ; }

    private IMaze maze;
    private int cells;

    public void Init(IMaze TargetMaze)
    {
        maze = TargetMaze;
        Init();
    }

    public void Init()
    {
        cells = maze.Width * maze.Height - 1;
    }

    public bool NextStep()
    {
        maze.SetPass(maze.CurrentCell, true);
        var choices = new List<Vector2Int>();

        foreach (var item in CelledMaze.Shifts)
        {
            var adj = maze.CurrentCell + item;
            if (maze.InMaze(adj))
                choices.Add(adj);
        }

        var index = Random.Range(0, choices.Count);
        if (!maze.GetPass(choices[index]))
        {
            maze.SetTunnel(maze.CurrentCell, choices[index], true);
            cells--;
        }

        maze.CurrentCell = choices[index];
        return (cells != 0);
    }
}
