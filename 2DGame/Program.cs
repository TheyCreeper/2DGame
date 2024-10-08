﻿using System.Diagnostics.Metrics;
using System.Text;
using System.Xml;

namespace _2DGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Defining variables
            const int DEF_POS_X = 46, DEF_POS_Y = 8;
            int posX = DEF_POS_X, posY = DEF_POS_Y;
            int newPosX, newPosY;
            char orientation;
            string[] mapBlockList = new string[16];

            LoadMapInRam(out mapBlockList, "..\\..\\..\\..\\data\\map.txt");
            DrawMap(mapBlockList);
            LoadPlayer(DEF_POS_X, DEF_POS_Y);
            PlayerInteraction(mapBlockList, posX, posY, out newPosX, out newPosY, out orientation);
        }

        private static void DrawMap(string[] mapBlockList)
        {
            int yCounter = 0;
            int cCounter = 0;
            while (yCounter < mapBlockList.Length)
            {
                char[] charList = mapBlockList[yCounter].ToCharArray();
                while (cCounter < mapBlockList[yCounter].ToCharArray().Length)
                {
                    DrawBlockWithColor(charList[cCounter]);
                    cCounter++;
                }
                cCounter = 0;
                Console.WriteLine("");
                yCounter++;
            }
        }

        private static void LoadMapInRam(out string[] mapBlockList, string pathToFile)
        {

            mapBlockList = new string[16];
            int lineCount = 0;

            FileStream mapFile = new FileStream(pathToFile, FileMode.Open, FileAccess.Read);
            using (StreamReader map = new StreamReader(mapFile, Encoding.UTF8))
            {
                while (lineCount < 16)
                {
                    mapBlockList[lineCount] = map.ReadLine();
                    lineCount++;
                }
            }
        }

        private static void DrawBlockWithColor(char c)
        {
            //gives out a console color for wathever char it gets, might make this a switch or an enum in the future idk
            if (c == '-') Console.BackgroundColor = ConsoleColor.Green;
            if (c == 'T') Console.BackgroundColor = ConsoleColor.DarkGreen;
            if (c == 'I') Console.BackgroundColor = ConsoleColor.DarkGray;
            if (c == 'Z') Console.BackgroundColor = ConsoleColor.White;

            Console.Write("  ");
            Console.ResetColor();
        }

        private static void LoadPlayer(int posX, int posY)
        {
            Console.SetCursorPosition(posX, posY);
            DrawBlockWithColor('Z');
            Console.SetCursorPosition(posX, posY);
        }

        private static void PlayerInteraction(string[] mapBlockList, int xPos, int yPos, out int newPosX, out int newPosY, out char orientation)
        {
            bool isAbleBreak = false;
            newPosX = xPos;
            newPosY = yPos;
            orientation = 'N';
            char block = '-';

            ConsoleKeyInfo keyPress;
            do
            {
                xPos = newPosX;
                yPos = newPosY;
                keyPress = new ConsoleKeyInfo();
                keyPress = Console.ReadKey(true);

                //Check if the input is WASD, else dont move the player because its likely going to be doing something else like breaking a block or wathever
                if (keyPress.Key == ConsoleKey.W || keyPress.Key == ConsoleKey.S || keyPress.Key == ConsoleKey.A || keyPress.Key == ConsoleKey.D)
                { block = PlayerMovement(mapBlockList, keyPress, xPos, yPos, out newPosX, out newPosY, out orientation); }
                else if (keyPress.Key == ConsoleKey.B)
                {

                }

            } while (keyPress.Key != ConsoleKey.Escape);
        }

        //This function is a bit too complicated for my liking, each case statement is very similar so there might be a way to make it less complicated
        private static char PlayerMovement(string[] mapBlockList, ConsoleKeyInfo keyPress, int xPos, int yPos, out int newPosX, out int newPosY, out char orientation)
        {
            newPosX = xPos;
            newPosY = yPos;
            orientation = 'N';

            bool isAble;

            char[] charList = mapBlockList[yPos].ToCharArray();
            char block;

            switch (keyPress.Key)
            {
                case ConsoleKey.W:
                    orientation = 'N';
                    CheckIfAbleToMove(orientation, mapBlockList, keyPress, xPos, yPos, out isAble, out block);

                    if (isAble)
                    {
                        block = charList[(xPos / 2)];
                        DrawBlockWithColor(block);

                        LoadPlayer(xPos, yPos - 1);
                        yPos--;
                        newPosX = xPos;
                        newPosY = yPos;
                    }
                    isAble = true;
                    break;
                case ConsoleKey.S:
                    orientation = 'S';
                    CheckIfAbleToMove(orientation, mapBlockList, keyPress, xPos, yPos, out isAble, out block);

                    if (isAble)
                    {
                        block = charList[(xPos / 2)];
                        DrawBlockWithColor(block);

                        LoadPlayer(xPos, yPos + 1);
                        yPos++;
                        newPosX = xPos;
                        newPosY = yPos;
                    }
                    isAble = true;
                    break;
                case ConsoleKey.A:
                    orientation = 'W';
                    CheckIfAbleToMove(orientation, mapBlockList, keyPress, xPos - 2, yPos, out isAble, out block);

                    if (isAble)
                    {
                        block = charList[(xPos / 2)];
                        DrawBlockWithColor(block);

                        LoadPlayer(xPos - 2, yPos);
                        xPos -= 2;
                        newPosX = xPos;
                        newPosY = yPos;
                    }
                    isAble = true;
                    break;
                case ConsoleKey.D:
                    orientation = 'E';
                    CheckIfAbleToMove(orientation, mapBlockList, keyPress, xPos + 2, yPos, out isAble, out block);

                    if (isAble)
                    {
                        block = charList[(xPos / 2)];
                        DrawBlockWithColor(block);

                        LoadPlayer(xPos + 2, yPos);
                        xPos += 2;
                        newPosX = xPos;
                        newPosY = yPos;
                    }
                    isAble = true;
                    break;
                default:
                    newPosX = xPos;
                    newPosY = yPos;
                    block = 'Z';
                    break;
            }
            return block;
        }


        //This function is a bit too complicated for my liking, each case statement is very similar so there might be a way to make it less complicated
        private static void CheckIfAbleToMove(char orientation, string[] mapBlockList, ConsoleKeyInfo keyPress, int xPos, int yPos, out bool isAbleMove, out bool isAbleBreak, out char block)
        {
            char[] charListBelow = new char[2];
            char[] charListAbove = new char[2];

            char[] charListCurrent = mapBlockList[yPos].ToCharArray();
            if (yPos != 0) { charListAbove = mapBlockList[yPos - 1].ToCharArray(); }
            if (yPos != 15) { charListBelow = mapBlockList[yPos + 1].ToCharArray(); }

            //Set the variables as the default ones
            block = '-';
            isAbleMove = true;
            isAbleBreak = false;

            switch (orientation)
            {
                case 'N':
                    block = charListAbove[(xPos / 2)];
                    if (block != '-')
                    {
                        isAbleMove = false;
                        isAbleBreak = true;
                    }
                    break;
                case 'S':
                    block = charListBelow[(xPos / 2)];
                    if (block != '-')
                    {
                        isAbleMove = false;
                        isAbleBreak = true;
                    }
                    break;
                case 'W':
                    block = charListCurrent[(xPos / 2)];
                    if (block != '-')
                    {
                        isAbleMove = false;
                        isAbleBreak = true;
                    }
                    break;
                case 'E':
                    block = charListCurrent[(xPos / 2)];
                    if (block != '-')
                    {
                        isAbleMove = false;
                        isAbleBreak = true;
                    }
                    break;
            }
        }

        private static void BreakBlock(char orientation, char block, int xPos, int yPos)
        {

        }
    }
}

