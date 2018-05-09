using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronalesNetz_V0._5
{
    public class Tetris
    {
        //Variablen.wertNeuron[last,0] = links
        //Variablen.wertNeuron[last,1] = rechts
        //Variablen.wertNeuron[last,2] = unten
        //Variablen.wertNeuron[last,3] = links drehen
        //Variablen.wertNeuron[last,4] = rechts drehen

        public static Double punkte = 0;
        public static bool leben = true;
        public static bool kiTutNichts = false;
        public static int sinnloseBewegung = 0;

        private static Random rnd = new Random();
        private static bool[,] map = new bool[10, 20];
        private static int xPosPlayer = rnd.Next(2, 4);
        private static int yPosPlayer = -5;


        private static readonly int[,,,] AlleBlock = new int[7, 4, 4, 2] { { { { 2,3 },{ 2,2 },{ 2,1 },{ 2,0 } } , { { 0,2 },{ 1,2 },{ 2,2 },{ 3,2 } } , { { 1,3 },{ 1,2 },{ 1,1 },{ 1,0 } } , { { 0,1 },{ 1,1 },{ 2,1 },{ 3,1 } } },		//Line-Block	//1. Dimension: Der Block an sich
						  { { { 1,1 },{ 1,2 },{ 2,1 },{ 2,2 } } , { { 1,1 },{ 1,2 },{ 2,1 },{ 2,2 } } , { { 1,1 },{ 1,2 },{ 2,1 },{ 2,2 } } , { { 1,1 },{ 1,2 },{ 2,1 },{ 2,2 } } },//O-Block		//2. Dimension: Die Drehung des Bocks
						  { { { 0,1 },{ 1,1 },{ 2,1 },{ 1,2 } } , { { 1,0 },{ 1,1 },{ 1,2 },{ 2,1 } } , { { 0,1 },{ 1,1 },{ 2,1 },{ 1,0 } } , { { 0,1 },{ 1,1 },{ 1,0 },{ 1,2 } } },//T-Block		//3. Dimension: Die Position der Einzelnen Blöcke
						  { { { 1,3 },{ 1,2 },{ 1,1 },{ 2,1 } } , { { 1,3 },{ 1,2 },{ 2,3 },{ 3,3 } } , { { 2,3 },{ 3,3 },{ 3,2 },{ 3,1 } } , { { 2,1 },{ 1,1 },{ 3,2 },{ 3,1 } } },//L-Block		//4. Dimension: Die {x,y} Position
						  { { { 3,3 },{ 3,2 },{ 3,1 },{ 2,1 } } , { { 1,2 },{ 1,1 },{ 3,1 },{ 2,1 } } , { { 1,2 },{ 1,1 },{ 1,3 },{ 2,3 } } , { { 1,3 },{ 2,3 },{ 3,3 },{ 3,2 } } },//J-Block
                          { { { 1,1 },{ 2,1 },{ 2,2 },{ 3,2 } } , { { 2,2 },{ 2,1 },{ 3,1 },{ 3,0 } } , { { 1,0 },{ 2,0 },{ 2,1 },{ 3,1 } } , { { 2,0 },{ 2,1 },{ 1,1 },{ 1,2 } } },//S-Block
                          { { { 1,2 },{ 2,2 },{ 2,1 },{ 3,1 } } , { { 1,1 },{ 1,2 },{ 2,2 },{ 2,3 } } , { { 1,3 },{ 2,3 },{ 2,2 },{ 3,2 } } , { { 2,1 },{ 2,2 },{ 3,2 },{ 3,3 } } } };//Z-Block

        private static bool neuerStein = false;
        private static int derzeitigeDrehung = 2000000;
        private static int derzeitigerStein = rnd.Next(7);

        public static double[] UserBewegung(char taste)
        {
            Console.SetCursorPosition(0, 0);

            double[] a = new double[5];

            if (taste == 'a')
                a[0] = 1;
            else if (taste == 'd')
                a[1] = 1;
            else if (taste == 's')
                a[2] = 1;
            else if (taste == 'q')
                a[3] = 1;
            else if (taste == 'e')
                a[4] = 1;

            BewegungShow(a,0.5);
            Console.SetCursorPosition(0, 0);
            Console.Write(" ");
            return a;
        }

        public static void TetrisSetup()
        {

            Console.ForegroundColor = ConsoleColor.Black;
            for (int ix = 0; ix < 10; ++ix)
            {
                for (int iy = 0; iy < 20; ++iy)
                {
                    Console.SetCursorPosition(ix * 2 + 10, iy + 5);
                    Console.Write("██");

                }
            }
            Console.ResetColor();
        }

        private static int[,] PosOfEachBlock()
        {
            int[,] block = new int[4, 2];

            for (int i = 0; i < 4; ++i)
            {
                block[i, 0] = xPosPlayer + AlleBlock[derzeitigerStein, derzeitigeDrehung % 4, i, 0];
                block[i, 1] = yPosPlayer + AlleBlock[derzeitigerStein, derzeitigeDrehung % 4, i, 1];
            }
            return block;
        }

        private static int[,] PosOfEachBlock(int xNeu, int yNeu, int drehNeu)
        {
            int[,] block = new int[4, 2];

            for (int i = 0; i < 4; ++i)
            {
                block[i, 0] = xNeu + AlleBlock[derzeitigerStein, drehNeu % 4, i, 0];
                block[i, 1] = yNeu + AlleBlock[derzeitigerStein, drehNeu % 4, i, 1];
            }
            return block;
        }

        public static void SteinSetzen(int x, int y, ConsoleColor c)
        {

            Console.ForegroundColor = c;
            Console.SetCursorPosition(x * 2 + 10, y + 10);
            Console.Write("██");
            Console.ResetColor();
        }

        private static void SteinAktualisieren(int xNeu, int yNeu, int neuerDreh)
        {
            int[,] blocke = PosOfEachBlock();
            for (int i = 0; i < 4; ++i)
            {
                if (blocke[i, 1] < 0)
                {
                    SteinSetzen(blocke[i, 0], blocke[i, 1], ConsoleColor.Black);
                }
                else
                {
                    SteinSetzen(blocke[i, 0], blocke[i, 1], ConsoleColor.Green);
                }
            }

            blocke = PosOfEachBlock(xNeu, yNeu, neuerDreh);
            for (int i = 0; i < 4; ++i)
            {
                SteinSetzen(blocke[i, 0], blocke[i, 1], ConsoleColor.Red);
            }

        }

        public static void MapAktualisieren(bool neu)
        {
            for (int ix = 0; ix < 10; ++ix)
            {
                for (int iy = 0; iy < 20; ++iy)
                {
                    if (map[ix, iy])
                        SteinSetzen(ix, iy, ConsoleColor.Blue);
                    else if (neu)
                        SteinSetzen(ix, iy, ConsoleColor.Green);
                }
            }
        }

        private static bool InDerMap(int neuerX, int neuerY, int neuerDreh)
        {
            bool a = true;
            int[,] blocke = PosOfEachBlock(neuerX, neuerY, neuerDreh);

            for (int i = 0; i < 4 && a; i++)
            {
                if (blocke[i, 0] < 0)
                    a = false;
                if (blocke[i, 0] > 9)
                    a = false;
                if (blocke[i, 1] > 19)
                    a = false;
            }

            return a;
        }

        private static bool WegFrei(int neuerX, int neuerY, int neuerDreh)
        {
            bool a = true;

            if (InDerMap(neuerX, neuerY, neuerDreh))
            {
                int[,] blocke = PosOfEachBlock(neuerX, neuerY, neuerDreh);

                for (int i = 0; i < 4 && a; ++i)
                {
                    if (blocke[i, 1] >= 0)
                    {
                        if (map[blocke[i, 0], blocke[i, 1]])
                        {
                            a = false;
                        }
                    }

                }
            }
            else
            {
                a = false;
            }


            return a;
        }

        private static bool ExistiertReiheMit10()
        {
            bool a = false;

            for (int iy = 0; iy < 20 && !a; ++iy)
            {
                int b = 0;

                for (int ix = 0; ix < 10; ++ix)
                {
                    if (map[ix, iy])
                        ++b;
                }

                if (b == 10)
                    a = true;
            }

            return a;
        }

        private static int ObersteReiheMit10()
        {
            int a = 100;

            for (int iy = 19; iy >= 0 && a == 100; --iy)
            {
                int b = 0;

                for (int ix = 0; ix < 10; ++ix)
                {
                    if (map[ix, iy])
                        ++b;
                }

                if (b == 10)
                    a = iy;
            }

            return a;
        }

        private static void ReiheKaputtHandeler()
        {
            for (int iy = ObersteReiheMit10(); iy >= 0; --iy)
            {

                for (int ix = 0; ix < 10; ++ix)
                {
                    if (iy == 0)
                        map[ix, iy] = false;
                    else
                        map[ix, iy] = map[ix, iy - 1];
                }
            }
        }

        public static void Reset()
        {
            xPosPlayer = rnd.Next(2, 4);
            yPosPlayer = -5;
            derzeitigerStein = rnd.Next(7);
            derzeitigeDrehung = 2000000;
            sinnloseBewegung = 0;
        }

        public static void AllesReset()
        {
            Reset();
            map = new bool[10, 20];
            neuerStein = false;
            kiTutNichts = false;

            leben = true;
        }

        public static void Bewegung(double[] output)
        {
            kiTutNichts = true;
            if (output[0] > 0.5)
            {
                kiTutNichts = false;
                if (WegFrei(xPosPlayer - 1, yPosPlayer, derzeitigeDrehung))
                {
                    --xPosPlayer;

                }
                ++sinnloseBewegung;
            }
            if (output[1] > 0.5)
            {
                kiTutNichts = false;
                if (WegFrei(xPosPlayer + 1, yPosPlayer, derzeitigeDrehung))
                {

                    ++xPosPlayer;

                }
                ++sinnloseBewegung;
            }
            if (output[2] > 0.5)
            {
                kiTutNichts = false;
                if (WegFrei(xPosPlayer, yPosPlayer + 1, derzeitigeDrehung))
                {
                    ++yPosPlayer;

                }
                else
                {
                    neuerStein = true;
                }
                ++sinnloseBewegung;
            }


            if (output[3] > 0.5)
            {
                kiTutNichts = false;
                if (WegFrei(xPosPlayer, yPosPlayer, derzeitigeDrehung + 1))
                {
                    ++derzeitigeDrehung;

                }
                sinnloseBewegung++;
            }
            if (output[4] > 0.5)
            {
                kiTutNichts = false;
                if (WegFrei(xPosPlayer, yPosPlayer, derzeitigeDrehung - 1))
                {
                    --derzeitigeDrehung;

                }
                sinnloseBewegung++;
            }

            if (!WegFrei(xPosPlayer, yPosPlayer + 1, derzeitigeDrehung))
            {
                int[,] a = PosOfEachBlock();

                for (int i = 0; i < 4; ++i)
                {
                    if (a[i, 1] >= 0)
                    {
                        map[a[i, 0], a[i, 1]] = true;
                    }
                    else
                    {
                        leben = false;
                    }
                }

                if (leben && ExistiertReiheMit10())
                {
                    int punkteMultiplikator = 500;

                    while (ExistiertReiheMit10())
                    {
                        punkte += punkteMultiplikator;
                        ReiheKaputtHandeler();
                        punkteMultiplikator *= 2;
                    }
                }
                //punkte += yPosPlayer;
                Reset();


            }
        }

        public static void BewegungShow(double[] output,double schwellwert)
        {
            kiTutNichts = true;
            if (output[0] > schwellwert)
            {
                kiTutNichts = false;
                if (WegFrei(xPosPlayer - 1, yPosPlayer, derzeitigeDrehung))
                {
                    SteinAktualisieren(xPosPlayer - 1, yPosPlayer, derzeitigeDrehung);
                    --xPosPlayer;

                }
                ++sinnloseBewegung;
            }
            if (output[1] > schwellwert)
            {
                kiTutNichts = false;
                if (WegFrei(xPosPlayer + 1, yPosPlayer, derzeitigeDrehung))
                {
                    SteinAktualisieren(xPosPlayer + 1, yPosPlayer, derzeitigeDrehung);
                    ++xPosPlayer;

                }
                ++sinnloseBewegung;
            }
            if (output[2] > schwellwert)
            {
                kiTutNichts = false;
                if (WegFrei(xPosPlayer, yPosPlayer + 1, derzeitigeDrehung))
                {
                    SteinAktualisieren(xPosPlayer, yPosPlayer + 1, derzeitigeDrehung);
                    ++yPosPlayer;

                }
                ++sinnloseBewegung;
            }


            if (output[3] > schwellwert)
            {
                kiTutNichts = false;
                if (WegFrei(xPosPlayer, yPosPlayer, derzeitigeDrehung + 1))
                {
                    SteinAktualisieren(xPosPlayer, yPosPlayer, derzeitigeDrehung + 1);
                    ++derzeitigeDrehung;
                }
                ++sinnloseBewegung;
            }
            if (output[4] > schwellwert)
            {
                kiTutNichts = false;
                if (WegFrei(xPosPlayer, yPosPlayer, derzeitigeDrehung - 1))
                {
                    SteinAktualisieren(xPosPlayer, yPosPlayer, derzeitigeDrehung - 1);
                    --derzeitigeDrehung;
                }
                ++sinnloseBewegung;
            }

            if (!WegFrei(xPosPlayer, yPosPlayer + 1, derzeitigeDrehung))
            {
                int[,] a = PosOfEachBlock();

                for (int i = 0; i < 4; ++i)
                {
                    if (a[i, 1] >= 0)
                    {
                        map[a[i, 0], a[i, 1]] = true;
                        SteinSetzen(a[i, 0], a[i, 1], ConsoleColor.Blue);

                    }
                    else
                    {
                        leben = false;

                    }
                }

                if (leben && ExistiertReiheMit10())
                {
                    Double punkteMultiplikator = 2;

                    while (ExistiertReiheMit10())
                    {
                        punkte += punkteMultiplikator;
                        ReiheKaputtHandeler();
                        punkteMultiplikator *= 1.5;
                    }
                    MapAktualisieren(true);
                }
                //punkte += yPosPlayer;
                Reset();

            }
        }

        public static double[] GetV()
        {
            double[] a = new double[209];
            int pos = 0;



            foreach (bool j in map)
            {
                if (j)
                    a[pos] = 1;
                else
                    a[pos] = -1;
                ++pos;
            }

            foreach (int j in PosOfEachBlock())
            {
                a[pos] = Convert.ToDouble(j);
                ++pos;
            }

            a[208] = punkte;

            return a;
        }

        public static double[] GetType2V()
        {
            double[] output = new double[280];

            for (int y = 0; y < 20; ++y)
            {
                for (int x = 0; x < 10; ++x)
                {
                    if (map[x, y])
                        output[x + ((y + 8) * 10)] = 1;
                    else
                        output[x + ((y + 8) * 10)] = -1;
                }
            }

            int[,] a = PosOfEachBlock();

            for (int i = 0; i < 4; ++i)
            {
                output[a[i, 0] + ((a[i, 1] + 8) * 10)] = 10;
            }

            return output;
        }

        public static short[] GetMaxHeightBlock()
        {
            short[] output = new short[18];

            for (int i0 = 0; i0 < 10; i0++)
            {
                byte a = 0;
                while (a < 20 && !map[i0, a])
                {
                    ++a;
                }

                output[i0] = a;
            }

            int pos = 10;
            foreach (int item in PosOfEachBlock())
            {
                output[pos] = Convert.ToInt16(item);
                ++pos;
            }

            return output;
        }

        public static short[] GetReihe(params int[] reihen)
        {
            short[] output = new short[reihen.Length * 10 + 4];

            for (int i0 = 0; i0 < reihen.Length; ++i0)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (map[i, 19])
                    {
                        output[i + i0] = 1;
                    }
                    else
                    {
                        output[i + i0] = -1;
                    }
                }
            }


            output[reihen.Length * 10] = (short)derzeitigerStein;
            output[reihen.Length * 10 + 1] = (short)derzeitigeDrehung;
            output[reihen.Length * 10 + 2] = (short)yPosPlayer;
            output[reihen.Length * 10 + 3] = (short)xPosPlayer;


            return output;
        }

        public static void CheckLocher()
        {
            for (int i0 = 0; i0 < 10; ++i0)
            {
                int obersterBlock = -1;

                for (int i1 = 0; i1 < 20 && obersterBlock == -1; ++i1)
                {
                    if (map[i0, i1])
                        obersterBlock = i1;
                }

                if (obersterBlock > -1)
                {
                    for (int i1 = obersterBlock; i1 < 20; ++i1)
                    {
                        if (!map[i0, i1])
                            punkte -= 20;
                    }
                }
            }
        }

        public static void CheckLocher(int layer)
        {
            int zugebauteSpalten = 0;
            for (int i0 = 0; i0 < 10; ++i0)
            {
                if (!map[i0, layer])
                {
                    int blockProSpalte = 0;

                    for (int i1 = 0; i1 < layer - 1; ++i1)
                    {
                        if (map[i0, i1])
                            ++blockProSpalte;
                    }

                    if (blockProSpalte > 0)
                    {
                        ++zugebauteSpalten;
                        punkte -= 30;
                    }
                }
                else
                {
                    ++zugebauteSpalten;
                }

            }

            if (zugebauteSpalten >= 10)
            {
                Tetris.punkte -= 300;
            }
        }

        public static void GetPunkteFurFastFolleReihe(int minBlocke, params int[] zuCheckeneReihe)
        {
            for (int i0 = 0; i0 < zuCheckeneReihe.Length; ++i0)
            {
                int blocke = 0;

                for (int i1 = 0; i1 < 10; ++i1)
                {
                    if (map[i1, zuCheckeneReihe[i0]])
                        ++blocke;
                }

                if (blocke > minBlocke)
                    punkte += 20 + blocke * 2;
            }
        }
    }

    public class SimplesSpiel
    {
        [Serializable]
        public struct SimplPos
        {
            public sbyte x;
            public sbyte y;

            public SimplPos(sbyte x, sbyte y)
            {
                this.x = x;
                this.y = y;
            }
            public SimplPos(int x, int y)
            {
                this.x = (sbyte)x;
                this.y = (sbyte)y;
            }

            public SimplPos(Pos a)
            {
                x = a.xPos;
                y = a.yPos;
            }

            public override string ToString()
            {
                return x + "|" + y;
            }

            public override bool Equals(object obj)
            {
                if(!(obj is SimplPos))
                {
                    return false;
                }

                SimplPos temp = (SimplPos)obj;

                return x == temp.x && y == temp.y;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public static bool operator <=(SimplPos p1, SimplPos p2)
            {
                return p1.y < p2.y || p1.y == p2.y && p1.x <= p2.x;
            }
            public static bool operator >=(SimplPos p1, SimplPos p2)
            {
                return p1.y > p2.y || p1.y == p2.y && p1.x >= p2.x;
            }

            public static bool operator ==(SimplPos p1, SimplPos p2)
            {
                return p1.x == p2.x && p1.y == p2.y;
            }
            public static bool operator !=(SimplPos p1, SimplPos p2)
            {
                return !(p1.x == p2.x && p1.y == p2.y);
            }
        }

        private static int punktzahl = 0;
        private static Random rnd = new Random();
        public static Pos playerPos;
        private static Pos FeldOffset;
        private static Pos FeldGrosse;
        public static double punkte=0;

        private static Dictionary<SimplPos, double> punkteDic = new Dictionary<SimplPos, double>();

        public static void Setup(sbyte xGrosse,sbyte yGrosse, int hindernissCount, int zielCount)
        {
            FeldGrosse = new Pos(xGrosse, yGrosse);

            for (int i = 0; i < zielCount; ++i)
            {
                sbyte x;
                sbyte y;

                do
                {

                    x = (sbyte)rnd.Next(FeldGrosse.xPos);
                    y = (sbyte)rnd.Next(FeldGrosse.yPos);

                } while (punkteDic.ContainsKey(new SimplPos(x, y)));

                punkteDic.Add(new SimplPos(x, y), 1);
            }

            for (int i = 0; i < hindernissCount; ++i)
            {
                sbyte x;
                sbyte y;

                do
                {

                    x = (sbyte)rnd.Next(FeldGrosse.xPos);
                    y = (sbyte)rnd.Next(FeldGrosse.yPos);

                } while (punkteDic.ContainsKey(new SimplPos(x, y)));

                punkteDic.Add(new SimplPos(x, y), -1);
            }

            FeldOffset = new Pos(10, 5);
            Reset();
        }

        public static void Reset()
        {
            Console.Clear();
            Console.ResetColor();
            for (int i = 0; i < FeldGrosse.xPos; i++)
            {
                Console.SetCursorPosition(FeldOffset.xPos + (i * 2), FeldOffset.yPos - 1);
                Console.Write(i % 10);
            }

            for (sbyte y = 0; y < FeldGrosse.yPos; ++y)
            {
                Console.SetCursorPosition(FeldOffset.xPos - 1, y + FeldOffset.yPos);
                Console.Write(y % 10);

                for (sbyte x = 0; x < FeldGrosse.xPos; ++x)
                {
                    SimplPos TempPos = new SimplPos(x, y);
                    ConsoleColor tempColor = ConsoleColor.White;

                    if (punkteDic.ContainsKey(TempPos))
                    {
                        if (punkteDic[TempPos] > 0)
                        {
                            tempColor = ConsoleColor.Green;
                        }
                        else
                        {
                            tempColor = ConsoleColor.Red;
                        }
                    }

                    BlockSetzen(TempPos, tempColor);
                }

                Console.ResetColor();
                Console.Write(y % 10);

                
            }

            Console.ResetColor();
            for (int i = 0; i < FeldGrosse.xPos; i++)
            {
                Console.SetCursorPosition(FeldOffset.xPos + (i * 2), FeldOffset.yPos + FeldGrosse.yPos);
                Console.Write(i % 10);
            }


            do
            {
                playerPos = new Pos((sbyte)rnd.Next(FeldGrosse.xPos), (sbyte)rnd.Next(FeldGrosse.yPos));
            } while (punkteDic.ContainsKey(new SimplPos(playerPos)));

            BlockSetzen(playerPos, ConsoleColor.Magenta);
        }


        public static double PlayerBewegen(int action)
        {
            //0=rechts;
            //1=links;
            //2=oben;
            //3=unten;

            double reward = 0;

            if (action == 0 && playerPos.xPos + 1 < FeldGrosse.xPos)
            {
                Bewegung(1, 0);
            }
            else if (action == 1 && playerPos.xPos - 1 >= 0)
            {
                Bewegung(-1, 0);
            }
            else if (action == 2 && playerPos.yPos - 1 >= 0)
            {
                Bewegung(0, -1);
            }
            else if (action == 3 && playerPos.yPos + 1 < FeldGrosse.yPos)
            {
                Bewegung(0, 1);
            }

            if (punkteDic.ContainsKey(new SimplPos(playerPos)))
            {
                reward = punkteDic[new SimplPos(playerPos)];
                if (punkteDic[new SimplPos(playerPos)] > 0)
                {
                    punktzahl++;
                    Console.SetCursorPosition(FeldOffset.xPos + FeldGrosse.xPos * 2 + 3, FeldOffset.yPos + FeldGrosse.yPos / 2);
                    Console.ResetColor();
                    Console.Write("Punktzahl: {0}", punktzahl);

                    for(int i = 0; i < 10; ++i)
                    {
                        System.Threading.Thread.Sleep(100);
                        if(i%2==0)
                            BlockSetzen(playerPos, ConsoleColor.Green);
                        else
                            BlockSetzen(playerPos, ConsoleColor.Magenta);
                    }

                    BlockSetzen(playerPos, ConsoleColor.Green);
                }
                else
                {
                    for (int i = 0; i < 10; ++i)
                    {
                        System.Threading.Thread.Sleep(20);
                        if (i % 2 == 0)
                            BlockSetzen(playerPos, ConsoleColor.Red);
                        else
                            BlockSetzen(playerPos, ConsoleColor.Magenta);
                    }
                    BlockSetzen(playerPos, ConsoleColor.Red);
                }
                do
                {
                    playerPos = new Pos((sbyte)rnd.Next(FeldGrosse.xPos), (sbyte)rnd.Next(FeldGrosse.yPos));
                } while (punkteDic.ContainsKey(new SimplPos(playerPos)));

                BlockSetzen(playerPos, ConsoleColor.Magenta);
                Console.ResetColor();
                //Reset();
            }
            punkte = reward;

            return reward;
        }

        public static void Bewegung(sbyte addToX, sbyte addToY)
        {
            BlockSetzen(playerPos, ConsoleColor.White);
            playerPos.xPos += addToX;
            playerPos.yPos += addToY;
            BlockSetzen(playerPos, ConsoleColor.Magenta);
        }

        public static void BlockSetzen(Pos pos, ConsoleColor color)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(pos.xPos * 2 + FeldOffset.xPos, pos.yPos + FeldOffset.yPos);
            Console.ForegroundColor = color;
            Console.Write("██");
        }

        public static void BlockSetzen(SimplPos pos, ConsoleColor color)
        {
            Console.SetCursorPosition(pos.x * 2 + FeldOffset.xPos, pos.y + FeldOffset.yPos);
            Console.ForegroundColor = color;
            Console.Write("██");
        }

        public static object GetPlayerPos()
        {
            return new SimplPos(playerPos);
        }

        public static double[] GetMap()
        {
            double[,] output = new double[FeldGrosse.xPos, FeldGrosse.yPos];

            foreach(KeyValuePair<SimplPos,double> item in punkteDic)
            {
                output[item.Key.x, item.Key.y] = item.Value;
            }

            List<double> outputList = new List<double>();

            foreach(double item in output)
            {
                outputList.Add(item);
            }
            return outputList.ToArray();
        } 

        public static void ExtraMapAusgeben(double[] Map)
        {
            for(int x = 0; x < FeldGrosse.xPos; ++x)
            {
                for (int y = 0; y < FeldGrosse.yPos; ++y)
                {
                    if (Map[x * y + y] > 0.5)
                    {
                        BlockSetzen(new SimplPos(x + Convert.ToInt32(FeldGrosse.xPos) + 20, y),ConsoleColor.Green);
                    }
                    else if(Map[x * y + y] < -0.5)
                    {
                        BlockSetzen(new SimplPos(x + Convert.ToInt32(FeldGrosse.xPos) + 20, y),ConsoleColor.Red);
                    }
                    else
                    {
                        BlockSetzen(new SimplPos(x + Convert.ToInt32(FeldGrosse.xPos) + 20, y),ConsoleColor.White);
                    }
                    
                }
            }
        }

        [Serializable]
        public class Pos
        {
            public sbyte xPos;
            public sbyte yPos;

            public Pos(sbyte x, sbyte y)
            {
                xPos = x;
                yPos = y;
            }

            //public Pos()
            //{
            //    xPos = (sbyte)rnd.Next(10);
            //    yPos = (sbyte)rnd.Next(10);
            //}


            public static bool operator ==(Pos p1, Pos p2)
            {
                return p1.yPos == p2.yPos && p1.xPos == p2.xPos;
            }

            public static bool operator !=(Pos p1, Pos p2)
            {
                return !(p1.yPos == p2.yPos && p1.xPos == p2.xPos);
            }

            public override string ToString()
            {
                return xPos + "|" + yPos;
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}
