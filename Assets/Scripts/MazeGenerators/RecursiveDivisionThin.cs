﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//http://progressor-blog.ru/qt/generatsiya-labirinta-i-ego-prohozhdenie/
public class RecursiveDivisionThin : MazeGenerator
{
    public void Generate() { while (NextStep()) ; }
    private Maze maze;
    public void Init(Maze TargetMaze)
    {
        maze = TargetMaze;

        rects.Clear();

        for (int i = 0; i < maze.Width; i++)
            for (int n = 0; n < maze.Height; n++)
                maze.SetPass(new IntVector2(i, n), true);

        for (int i = 0; i < maze.Width; i++)
            for (int n = 0; n < maze.Height - 1; n++)
                maze.SetTunnel(new IntVector2(i, n), new IntVector2(i, n + 1), true);

        for (int i = 0; i < maze.Width - 1; i++)
            for (int n = 0; n < maze.Height; n++)
                maze.SetTunnel(new IntVector2(i, n), new IntVector2(i + 1, n), true);

        rects.Enqueue(new IntRect(new IntVector2(0, 0), new IntVector2(maze.Width - 1, maze.Height - 1)));
    }

    Queue<IntRect> rects = new Queue<IntRect>();
    
    public bool NextStep()
    {
        if (rects.Count == 0)
            return false;

        var rect = rects.Dequeue();

        if (rect.type == IntRect.Type.Horizontal || (rect.type == IntRect.Type.Square && Random.value > 0.5f))
        {
            var newX = CustomRandom(rect.from.x, rect.to.x);
            rects.Enqueue(new IntRect(rect.from, new IntVector2(newX, rect.to.y)));
            rects.Enqueue(new IntRect(new IntVector2(newX + 1, rect.from.y), rect.to));

            var tunnelY = Random.Range(rect.from.y, rect.to.y + 1);
            for (int i = rect.from.y; i < rect.to.y + 1; i++)
                if (i != tunnelY)
                    maze.SetTunnel(new IntVector2(newX, i), new IntVector2(newX + 1, i), false);

            return true;
        }

        if (rect.type == IntRect.Type.Vertical || rect.type == IntRect.Type.Square)
        {
            var newY = CustomRandom(rect.from.y, rect.to.y);
            rects.Enqueue(new IntRect(rect.from, new IntVector2(rect.to.x, newY)));
            rects.Enqueue(new IntRect(new IntVector2(rect.from.x, newY + 1), rect.to));

            var tunnelX = Random.Range(rect.from.x, rect.to.x + 1);
            for (int i = rect.from.x; i < rect.to.x + 1; i++)
                if (i != tunnelX)
                    maze.SetTunnel(new IntVector2(i, newY), new IntVector2(i, newY + 1), false);

            return true;
        }

        return true;
    }

    //Borders are included
    private int CustomRandom(int from, int to)
    {
        var sample = 2f * Random.value - 1;
        //sample = -1f .. 1f
        sample = (to - from) * (Mathf.Pow(sample, 3) + 1f) / 2f + from;
        //(Mathf.Pow(sample, 3) + 1f) / 2f = 0 .. 1f
        //sample = from .. to
        return Mathf.FloorToInt(sample);
    }
}
