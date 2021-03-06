using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class LevelGenerator
{
    static Random rand = new Random();

    static void PlaceBlock(int[,] level, int x, int y, int width, int height)
    {
        for (int i = 0; i < width; ++i)
            for (int j = 0; j < height; ++j)
            {
                level[y + j, x + i] = 1;
            }
    }

    static void PlaceSection(
        int[,] level, int currentY, int sectionHeight,
        int minBlockWidth, int maxBlockWidth,
        int minBlockHeight, int maxBlockHeight,
        int chanceToPlaceBlock)
    {
        // the actual amount of height we can work with
        sectionHeight = Math.Min(level.GetLength(0) - currentY, maxBlockHeight);

        // max block height cannot be bigger than section height
        maxBlockHeight = maxBlockHeight > sectionHeight ? sectionHeight : maxBlockHeight;

        // TODO: Place a random block at the base somewhere...

        for (int x = 1; x < level.GetLength(1);)
        {
            int chance = rand.Next(0, 100);
            if (chance >= chanceToPlaceBlock)
            {
                // random with width and height of the block I want to place
                int blockY = rand.Next(0, maxBlockHeight);
                int blockWidth = rand.Next(minBlockWidth, maxBlockWidth + 1);
                int blockHeight = rand.Next(minBlockHeight, maxBlockHeight + 1 - blockY);

                if (blockWidth < level.GetLength(1) - x)
                {
                    PlaceBlock(level, x, currentY + blockY, blockWidth, blockHeight);
                }

                x += blockWidth;

            }
            else
            {
                // move space by one
                x++;
            }
        }

    }

    public static int[,] GenerateLevel(
        int width, int height,
        int minBlockWidth, int maxBlockWidth,
        int minBlockHeight, int maxBlockHeight,
        int sectionHeight, int chanceToPlaceBlock, bool floor = true)
    {
        int[,] level = new int[height, width];

        int startPoint = 0;
        if (floor)
        {
            for (int i = 0; i < width; ++i)
            {
                level[0, i] = 1;
            }
            startPoint = 2;
        }

        for (int y = startPoint; y < height; y += sectionHeight)
        {
            PlaceSection(level, y, sectionHeight, minBlockWidth, maxBlockWidth, minBlockHeight, maxBlockHeight, chanceToPlaceBlock);
        }


        return level;
    }

}

