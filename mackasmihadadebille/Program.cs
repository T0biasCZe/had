using System.Numerics;
using System.Runtime.InteropServices;

namespace mackasmihadadebille {
    internal class Program {
        //vytvori generator nahodnych cisel
        static Random rnd;
        //definice cisel pro smery. 
        const int up = 0;
        const int right = 1;
        const int down = 2;
        const int left = 3;
        public static void Main(string[] args) {
            //nastavi font na consolas a velikost na 30
            ConsoleHelper.SetCurrentFont("Lucida Console", 30);
            rnd = new Random();
            //jede herni smyčku dokud se nezavre aplikace
            while(true) {
                game();
            }
        }
        
        static int direction = up; //smer hada
        static int lenght = 1; //delka hada. každe čislo je jedna část hada
        static int ate = 0; //kolik jablek už snedl had
        static int speed = 500; //"rychlost" hada. čim nižší čislo tim vetši rychlost
        static int apple_x; //pozice jableka na ose x
        static int apple_y; //pozice jablka na ose y
        static Vector2[] position = new Vector2[100]; //pole s pozicema hada. vector2 je promena s dvěma čísly. x a y
        static bool firstrun = true; //promena indikujici prvni spusteni hry
        public static void game() {
            //pokud je to prvni spusteni hry, tak se vyčisti display, nastavi se pozici hada na 10,10 a vygeneruje pozici jableka
            if(firstrun) {
                Console.Clear();
                firstrun = false;
                position[0] = new Vector2(10, 10);

                apple_x = rnd.Next(0, 20);
                apple_y = rnd.Next(0, 20);
            }
            //vytiskne rámeček
            printborder(22, 22);
            //pokud je had na pozici jableka, tak se ho sní a zvetsi se o jednu část, zrychli se o 25 a vygeneruje se nová pozice jableka
            if(Vector2.Distance(position[0], new Vector2(apple_x, apple_y)) < 1) {
                ate++;
                lenght++;
                speed -= 25;
                apple_x = rnd.Next(0, 20);
                apple_y = rnd.Next(0, 20);
            }

            //pokud je had delší než 1, tak se posunou polička v poli pozic
            if(lenght > 1) {
                //smaže poličko za hadem
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition((int)position[lenght -1].X + 1, (int)position[lenght-1].Y + 1);
                Console.Write(" ");
                
                for(int i = lenght; i > 0; i--) {
                    position[i] = position[i - 1];
                }
            }
            //jinak se jen vymaže čislo
            else {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition((int)position[0].X + 1, (int)position[0].Y + 1);
                Console.Write(" ");
            }
            //posune hada
            movesnake();
            //vytiskne hada
            printsnake();
            //vytiskne hada
            printapple();
            //brani inputu z klavesnice a počkani na dalši snímek. aby se nestalo že to nesejme vstup, tak se čekani rozděli na 4 menší části
            takeinput();
            Thread.Sleep(speed/4);
            takeinput();
            Thread.Sleep(speed / 4);
            takeinput();
            Thread.Sleep(speed / 4);
            takeinput();
            Thread.Sleep(speed / 4);
            
        }
        public static void takeinput() {
            //sejme inputy s klávesnice, a omezi že novy směr nesmi byt opak stavajiciho směru
            if(Console.KeyAvailable) {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch(key.Key) {
                    case ConsoleKey.UpArrow:
                        if(direction != down) {
                            direction = up;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if(direction != left) {
                            direction = right;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if(direction != up) {
                            direction = down;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if(direction != right) {
                            direction = left;
                        }
                        break;
                }
            }
        }
        public static void movesnake() {
            //pohne hada o políčko podle směru
            switch(direction) {
                case up:
                    position[0] += new Vector2(0, -1);
                    break;
                case right:
                    position[0] += new Vector2(1, 0);
                    break;
                case down:
                    position[0] += new Vector2(0, 1);
                    break;
                case left:
                    position[0] += new Vector2(-1, 0);
                    break;
            }
            //pokud se had dotkne okraje, tak se hra ukonči
            if(position[0].X == 0) gameover();
            if(position[0].X == 21) gameover();
            if(position[0].Y == 0) gameover();
            if(position[0].Y == 21) gameover();
            

        }
        //
        public static void printsnake() {
            Console.ForegroundColor = ConsoleColor.White;
            for(int i = 0; i < lenght; i++) {
                Console.SetCursorPosition((int)position[i].X + 1, (int)position[i].Y + 1);
                Console.Write("#");
            }
        }
        public static void printborder(int x, int y) {
            //vytiskne okraje
            Console.ForegroundColor = ConsoleColor.Green;
            //vrchni okraj
            for(int i = 0; i < x; i++) {
                Console.SetCursorPosition(i, 0) ;
                Console.Write("#");
            }
            //spodni okraj
            for(int i = 0; i < x; i++) {
                Console.SetCursorPosition(i, y);
                Console.Write("#");
            }
            //levy okraj
            for(int i = 0; i < y; i++) {
                Console.SetCursorPosition(0, i);
                Console.Write("#");
            }
            //pravy okraj
            for(int i = 0; i < y; i++) {
                Console.SetCursorPosition(x, i);
                Console.Write("#");
            }
            //vypise score vlevo
            Console.SetCursorPosition(24, 0);
            Console.Write("Score: " + ate);
        }
        
        public static void printapple() {
            //nastavi barvu na červenou a vytiskne jablko
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(apple_x + 1, apple_y + 1);
            Console.WriteLine("#");
        }
        public static void gameover() {
            Console.Clear();
            printborder(20,20);
            //vypise game over v červene
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(6, 4);
            Console.Write("GAME OVER");
            Console.SetCursorPosition(7, 5);
            Console.Write("Score: " + ate);
            Console.SetCursorPosition(4, 8);
            Console.WriteLine("Press any key");
            Console.SetCursorPosition(6, 9);
            Console.WriteLine("to restart");
            //počka až člověk zmačkne klavesu
            Console.ReadLine();
            //resetne proměnné na vychozí
            firstrun = true;
            ate = 0;
            lenght = 1;
            speed = 500;
            direction = up;
            //vyčisti display
            Console.Clear();
        }
        
    }
}
