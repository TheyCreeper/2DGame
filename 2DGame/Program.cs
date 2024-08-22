using System.Diagnostics.Metrics;
using System.Text;

namespace _2DGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int DEF_POS_X = 46, DEF_POS_Y = 8;
            int posX = DEF_POS_X, posY = DEF_POS_Y;
            int newPosX, newPosY;
            string[] mapBlockList = new string[16];

            LoadMapInRam(out mapBlockList, "..\\..\\..\\..\\data\\map.txt");
            DrawMap(mapBlockList);
            LoadPlayer(DEF_POS_X, DEF_POS_Y);
            PlayerInteraction(mapBlockList, posX, posY, out newPosX, out newPosY);
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

        private static void PlayerInteraction(string[] mapBlockList, int xPos, int yPos, out int newPosX, out int newPosY)
        {
            newPosX = xPos;
            newPosY = yPos;

            ConsoleKeyInfo keyPress;
            do
            {
                keyPress = new ConsoleKeyInfo();
                keyPress = Console.ReadKey(true);
                PlayerMovement(mapBlockList, keyPress, xPos, yPos, out newPosX, out newPosY);
            } while (keyPress.Key != ConsoleKey.Escape);
        }


        private static void PlayerMovement(string[] mapBlockList, ConsoleKeyInfo keyPress, int xPos, int yPos, out int newPosX, out int newPosY)
        {
            newPosX = xPos;
            newPosY = yPos;

            char[] charList = mapBlockList[yPos].ToCharArray();
            char block;

            switch (keyPress.Key) {
                case ConsoleKey.W:
                    block = charList[(xPos / 2)];
                    DrawBlockWithColor(block);

                    LoadPlayer(xPos, yPos - 1);
                    yPos--;
                    newPosX = xPos;
                    newPosY = yPos;
                    break;
                case ConsoleKey.S:
                    block = charList[(xPos / 2)];
                    DrawBlockWithColor(block);

                    LoadPlayer(xPos, yPos + 1);
                    yPos++;
                    newPosX = xPos;
                    newPosY = yPos;
                    break;
                case ConsoleKey.A:
                    block = charList[(xPos / 2)];
                    DrawBlockWithColor(block);

                    LoadPlayer(xPos - 2, yPos);
                    xPos -= 2;
                    newPosX = xPos;
                    newPosY = yPos;
                    break;
                case ConsoleKey.D:
                    block = charList[(xPos / 2)];
                    DrawBlockWithColor(block);

                    LoadPlayer(xPos + 2, yPos);
                    xPos += 2;
                    break;
                default:
                    newPosX = xPos;
                    newPosY = yPos;
                    break;
            }
        }
    }
}

