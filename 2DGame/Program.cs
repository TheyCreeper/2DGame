using System.Text;

namespace _2DGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] mapBlockList = new string[16];
            LoadMapInRam(out mapBlockList, "E:\\2DGame\\2DGame\\data\\map.txt");
            DrawMap(mapBlockList);

        }
        
        private static void DrawMap(string[] mapBlockList)
        {
            int yCounter = 0;
            while(yCounter < mapBlockList.Length)
            {
                mapBlockList[yCounter].ToCharArray
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

        private static void DrawItemWithColor( char c)
        {
            

            //Retransforme l'input de certaines piece en l'original écris par l'utilisateur. Rend le tout plus consistant avec ce qui est écris dans la console et ce qui est 
            //dessiner dans le board
            if (c == '-') bgColor = ConsoleColor.Green;
            if (c == 'T') bgColor = ConsoleColor.DarkGreen;
            if (c == 'I') bgColor = ConsoleColor.DarkGray;

            Console.Write("  ");
            Console.ResetColor();
        }

    }
}