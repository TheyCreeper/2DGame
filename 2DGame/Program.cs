using System.Diagnostics.Metrics;
using System.Text;
using System.Xml;

namespace _2DGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
            while(yCounter < mapBlockList.Length)
            {
                char[] charList = mapBlockList[yCounter].ToCharArray();
                while (cCounter < mapBlockList[yCounter].ToCharArray().Length)
                {
                    int t = mapBlockList[yCounter].ToCharArray().Length;
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
            //Retransforme l'input de certaines piece en l'original écris par l'utilisateur. Rend le tout plus consistant avec ce qui est écris dans la console et ce qui est 
            //dessiner dans le board
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

            ConsoleKeyInfo keyPress;
            do
            {
                xPos = newPosX;
                yPos = newPosY;
                keyPress = new ConsoleKeyInfo();
                keyPress = Console.ReadKey(true);

                if (keyPress.Key == ConsoleKey.W ||  keyPress.Key == ConsoleKey.S || keyPress.Key == ConsoleKey.A || keyPress.Key == ConsoleKey.D)
                { PlayerMovement(mapBlockList, keyPress, xPos, yPos, out newPosX, out newPosY, out orientation, out isAbleBreak); }


            } while (keyPress.Key != ConsoleKey.Escape);
        }


        private static void PlayerMovement(string[] mapBlockList, ConsoleKeyInfo keyPress, int xPos, int yPos, out int newPosX, out int newPosY, out char orientation, out bool isAbleBreak)
        {
            newPosX = xPos;
            newPosY = yPos;
            orientation = 'N';
            isAbleBreak = false;

            bool isAble;

            char[] charList = mapBlockList[yPos].ToCharArray();
            char block;

            switch (keyPress.Key) {
                case ConsoleKey.W:
                    orientation = 'N';
                    CheckIfAbleToMove(orientation, mapBlockList, keyPress, xPos, yPos, out isAble, out isAbleBreak);

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
                    CheckIfAbleToMove(orientation, mapBlockList, keyPress, xPos, yPos, out isAble, out isAbleBreak);

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
                    CheckIfAbleToMove(orientation, mapBlockList, keyPress, xPos - 2, yPos, out isAble, out isAbleBreak);

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
                    CheckIfAbleToMove(orientation, mapBlockList, keyPress, xPos + 2, yPos, out isAble, out isAbleBreak);

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
                    break;
            }
        }

        private static void CheckIfAbleToMove(int orientation, string[] mapBlockList, ConsoleKeyInfo keyPress, int xPos, int yPos, out bool isAbleMove, out bool isAbleBreak)
        {
            char[] charListBelow = mapBlockList[yPos].ToCharArray();
            char[] charListAbove = mapBlockList[yPos].ToCharArray();

            char[] charListCurrent = mapBlockList[yPos].ToCharArray();
            if (yPos != 0) { charListAbove = mapBlockList[yPos - 1].ToCharArray(); }
            if (yPos != 15) { charListBelow = mapBlockList[yPos + 1].ToCharArray(); }
            
            char block;
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
    }
}

