using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NeuronalesNetz_V0._5.KI_Sachen;
using NeuronalesNetz_V0._5.LearnAlgos;
using System.Drawing;

namespace NeuronalesNetz_V0._5
{
    class Programme
    {
        public static void GenerateAnimie(string echtbildPath, string animiePath)
        {
            UCDIG styleTrans = new UCDIG(new SimpleNet(0.5, 760124124500, 100, 1), new SimpleNet(0.5, 67500, 100), new SimpleNet(0.5, 100, 76500));
            
            styleTrans.discriminator.SetAfNames("S", "S");
            styleTrans.discriminator.SetAfT(1, 20000);
            styleTrans.discriminator.SetAfT(2, 50123);
            
            styleTrans.TrainDisciminator(10000, @"C:\byteArr für ki");

            styleTrans.encoder.SetAfNames("s", "s", "s", "s");
            styleTrans.decoder.SetAfNames("s", "s", "s", "s");
            styleTrans.outputPath = @"C:\fertig\";
            styleTrans.savePer = 100;



            styleTrans.Train(10000, animiePath, echtbildPath);

        }

        public static void TestLC()
        {
            //double[][] input = new double[][]
            //{
            //    new double[]{0,0,0},
            //    new double[]{0,0,1},
            //    new double[]{0,1,0},
            //    new double[]{0,1,1},

            //    new double[]{1,0,0},
            //    new double[]{1,0,1},
            //    new double[]{1,1,0},
            //    new double[]{1,1,1}
            //};

            //bool[] result = new bool[]
            //{
            //    false,
            //    false,
            //    false,
            //    false,

            //    false,
            //    false,
            //    false,
            //    true
            //};

            double[][] input = new double[][]
            {
                new double[]{0,0},
                new double[]{0,1},
                new double[]{1,0},
                new double[]{1.1,1.1}
            };

            bool[] result = new bool[]
            {
                true,
                false,
                false,
                true
            };

            LinearClassifier LC = new LinearClassifier(input, result);

            while (true)
            {
                try
                {
                    Console.WriteLine("Eingabe: ");
                    double[] loop = new double[2];

                    for (int i = 0; i < 2; ++i)
                    {
                        loop[i] = Convert.ToDouble(Console.ReadLine());
                    }

                    Console.WriteLine("Ergebniss: " + LC.Classify(loop).ToString());
                }
                catch
                {
                    Console.WriteLine("Fehler bei der Eingabe");
                }
            }
        }

        public static void Jeff()
        {
            Random rnd = new Random();
            NeuronalNet Jeff = new NeuronalNet(0 ,209, 200, 100, 5);
            Jeff.StandartInitialisierung();
            Tetris.MapAktualisieren(true);
            bool nochmal = true;

            while (nochmal)
            {
                Tetris.MapAktualisieren(true);
                while (Tetris.leben)
                {
                    Jeff.InputGeben(Tetris.GetV());
                    Jeff.AllesBerechnen();
                    double[] b = Tetris.UserBewegung(Console.ReadKey().KeyChar);
                    Jeff.DeepLearning(b);
                }
                Tetris.AllesReset();
                Console.Clear();
                Console.Write("Nochmal");
                string text = Console.ReadLine();

                if (text == "nein" || text == "n")
                    nochmal = false;
            }
            Console.Clear();

            while (true)
            {
                Console.Clear();
                Tetris.MapAktualisieren(true);


                while (Tetris.leben && !Tetris.kiTutNichts && Tetris.sinnloseBewegung < 350)
                {
                    Console.SetCursorPosition(0, 0);
                    ArrayAusgeben(Jeff.GetOutput());
                    Jeff.InputGeben(Tetris.GetV());
                    Jeff.AllesBerechnen();


                    Tetris.BewegungShow(Jeff.GetOutput(),0.5);

                }


                Tetris.AllesReset();
            }


        }

        public static void Q_Learning_Tetris()
        {
            int lauf = 0;
            Q_Learning Q = new Q_Learning(0.9, 0.2, 5, 3,"Links","Rechts","Unten","Links Drehen","Rechts Drehen");
            while (true)
            {
                Tetris.TetrisSetup();
                Tetris.MapAktualisieren(true);

                while (Tetris.leben)
                {
                    double[] b = new double[5];
                   //Tetris.CheckLocher(18);
                    if (Tetris.punkte <= -300)
                    {
                        Tetris.leben = false;
                    }

                    Tetris.GetPunkteFurFastFolleReihe(5, 19, 18);
                    b[Q.Get_SetAction(Tetris.GetReihe(19, 18), Tetris.punkte)] = 1;
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Punkte: {0}", Tetris.punkte);


                    Tetris.punkte = 0;
                    Tetris.BewegungShow(b,0.5);
                    System.Threading.Thread.Sleep(15);
                }
                Q.Get_SetAction(Tetris.GetReihe(19, 18), -500);
                Tetris.AllesReset();
                Console.Clear();
                ++lauf;

                //if(lauf%100==0)
                //    Serialize(Q, @"C:\Users\Gregor\Ki\Q_LearningTable_1.bin");
            }
        }

        private static void ArrayAusgeben(double[] arr)
        {
            for (int i = 0; i < arr.Length; ++i)
                Console.Write("{0:F4}\t", arr[i]);

            Console.WriteLine("\n");
        }

        public static void BitmapConverter(string inputPath,string outputPath)
        {

        }

        public class BilderErkennen
        {
            public static void Run(int durchlaufe, int ausgebenPro)
            {
                double Fehler = 0;
                NeuronalNet Chantall = new NeuronalNet(0, 784, 20, 20, 10);
                Chantall.StatischeInitialisierung(true);
                Chantall.Etha = 4;

                FileStream ifsLabels = new FileStream(@"C:\Users\Gregor\memBrain\train-labels.idx1-ubyte", FileMode.Open); // test labels
                FileStream ifsImages = new FileStream(@"C:\Users\Gregor\memBrain\train-images.idx3-ubyte", FileMode.Open); // test images

                BinaryReader brLabels = new BinaryReader(ifsLabels);
                BinaryReader brImages = new BinaryReader(ifsImages);

                int magic1 = brImages.ReadInt32(); // discard
                int numImages = brImages.ReadInt32();
                int numRows = brImages.ReadInt32();
                int numCols = brImages.ReadInt32();

                int magic2 = brLabels.ReadInt32();
                int numLabels = brLabels.ReadInt32();

                byte[][] pixels = new byte[28][];
                for (int i = 0; i < pixels.Length; ++i)
                    pixels[i] = new byte[28];

                for(int i = 1; i < durchlaufe; ++i)
                {
                    if (i % 60000 == 0)
                    {
                        ifsImages.Close();
                        brImages.Close();
                        ifsLabels.Close();
                        brLabels.Close();
                        ifsLabels = new FileStream(@"C:\Users\Gregor\memBrain\train-labels.idx1-ubyte", FileMode.Open); // test labels
                        ifsImages = new FileStream(@"C:\Users\Gregor\memBrain\train-images.idx3-ubyte", FileMode.Open); // test images

                        brLabels = new BinaryReader(ifsLabels);
                        brImages = new BinaryReader(ifsImages);

                        magic1 = brImages.ReadInt32(); // discard
                        numImages = brImages.ReadInt32();
                        numRows = brImages.ReadInt32();
                        numCols = brImages.ReadInt32();

                        magic2 = brLabels.ReadInt32();
                        numLabels = brLabels.ReadInt32();

                        pixels = new byte[28][];
                        for (int j = 0; j < pixels.Length; ++j)
                            pixels[j] = new byte[28];
                    }

                    for (int i1 = 0; i1 < 28; ++i1)
                    {
                        for (int j = 0; j < 28; ++j)
                        {

                            byte b = brImages.ReadByte();
                            pixels[i1][j] = b;
                        }
                    }

                    byte lbl = brLabels.ReadByte();


                    double[] erwrteterOutput = new double[10];

                    erwrteterOutput[lbl] = 1;

                    Chantall.InputGeben(ConvertD(pixels));
                    Chantall.AllesBerechnen();
                    Chantall.DeepLearning(erwrteterOutput);

                    Fehler += Chantall.GetFehlerDif(erwrteterOutput);

                    if (i % ausgebenPro == 0)
                    {
                        Fehler /= ausgebenPro;
                        DigitImage dImage = new DigitImage(pixels, lbl);
                        Console.WriteLine(dImage.ToString());
                        ZweiArrayAusgeben(erwrteterOutput, Chantall.GetOutput());
                        Console.WriteLine("Treffer Quote: {0}", 100 - (Fehler * 10));
                        Console.WriteLine(Chantall.ToString());
                        Chantall.Etha = Fehler * 2;

                        Fehler = 0;
                    }

                }
            }

            private static void ZweiArrayAusgeben(double[] arr0, double[] arr1)
            {
                int sollIndex = 0;
                for(int i = 0; i < arr0.Length; ++i)
                {
                    if (arr0[i] == 1)
                        sollIndex = i;
                }

                for (int i = 0; i < arr0.Length; ++i)
                {
                    if (arr0[i] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (arr1[i] > arr1[sollIndex])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.WriteLine("{0}\t{1}\t{2:F4}",i ,arr0[i], arr1[i]);

                    Console.ResetColor();
                }


                Console.WriteLine("\n");
            }

            private static double[] ConvertD(byte[][] arr0)
            {
                double[] arr1 = new double[28 * 28];

                for (int i0 = 0, arr1pos = 0; i0 < 28; ++i0)
                {
                    for (int i1 = 0; i1 < 28; ++i1, ++arr1pos)
                    {
                        arr1[arr1pos] = Convert.ToDouble(arr0[i0][i1]);
                        arr1[arr1pos] /= 255;
                    }
                }
                return arr1;
            }


            private class DigitImage
            {
                public byte[][] pixels;
                public byte label;

                public DigitImage(byte[][] pixels, byte label)
                {
                    this.pixels = new byte[28][];
                    for (int i = 0; i < this.pixels.Length; ++i)
                        this.pixels[i] = new byte[28];

                    for (int i = 0; i < 28; ++i)
                        for (int j = 0; j < 28; ++j)
                            this.pixels[i][j] = pixels[i][j];

                    this.label = label;
                }

                public override string ToString()
                {
                    string s = "";
                    for (int i = 0; i < 28; ++i)
                    {
                        for (int j = 0; j < 28; ++j)
                        {
                            if (this.pixels[i][j] == 0)
                                s += " "; // white
                            else if (this.pixels[i][j] > 240)
                                s += "█"; // black
                            else if (this.pixels[i][j] > 170)
                                s += "0";
                            else if (this.pixels[i][j] > 155)
                                s += "O";
                            else
                                s += "."; // gray
                        }
                        s += "\n";
                    }
                    s += this.label.ToString();
                    return s;
                } // ToString

            }
        }

        public class SimpleBilderErkennen
        {
            public static void Run(int durchlaufe, int ausgebenPro)
            {
                double Fehler = 0;
                SimpleNet Chantall = new SimpleNet(0.1,784, 30, 20, 10);
                Chantall.SetAfNames(AF.Sigmoid, "n", "n", "n");
                Chantall.SetAfT(1, 1);
                Chantall.SetAfT(2, 1);
                Chantall.SetAfT(3, 1);

                FileStream ifsLabels = new FileStream(@"C:\Users\Gregor\DATENBSNKEBN\train-labels.idx1-ubyte", FileMode.Open); // test labels
                FileStream ifsImages = new FileStream(@"C:\Users\Gregor\DATENBSNKEBN\train-images.idx3-ubyte", FileMode.Open); // test images

                BinaryReader brLabels = new BinaryReader(ifsLabels);
                BinaryReader brImages = new BinaryReader(ifsImages);

                int magic1 = brImages.ReadInt32(); // discard
                int numImages = brImages.ReadInt32();
                int numRows = brImages.ReadInt32();
                int numCols = brImages.ReadInt32();

                int magic2 = brLabels.ReadInt32();
                int numLabels = brLabels.ReadInt32();

                byte[][] pixels = new byte[28][];
                for (int i = 0; i < pixels.Length; ++i)
                    pixels[i] = new byte[28];

                for (int i = 1; i < durchlaufe; ++i)
                {
                    if (i % 60000 == 0)
                    {
                        ifsImages.Close();
                        brImages.Close();
                        ifsLabels.Close();
                        brLabels.Close();
                        ifsLabels = new FileStream(@"C:\Users\Gregor\memBrain\train-labels.idx1-ubyte", FileMode.Open); // test labels
                        ifsImages = new FileStream(@"C:\Users\Gregor\memBrain\train-images.idx3-ubyte", FileMode.Open); // test images

                        brLabels = new BinaryReader(ifsLabels);
                        brImages = new BinaryReader(ifsImages);

                        magic1 = brImages.ReadInt32(); // discard
                        numImages = brImages.ReadInt32();
                        numRows = brImages.ReadInt32();
                        numCols = brImages.ReadInt32();

                        magic2 = brLabels.ReadInt32();
                        numLabels = brLabels.ReadInt32();

                        pixels = new byte[28][];
                        for (int j = 0; j < pixels.Length; ++j)
                            pixels[j] = new byte[28];
                    }

                    for (int i1 = 0; i1 < 28; ++i1)
                    {
                        for (int j = 0; j < 28; ++j)
                        {

                            byte b = brImages.ReadByte();
                            pixels[i1][j] = b;
                        }
                    }

                    byte lbl = brLabels.ReadByte();


                    double[] erwrteterOutput = new double[10];

                    erwrteterOutput[lbl] = 1;

                    Chantall.SetInput(ConvertD(pixels));
                    Chantall.OutputBerechnen();
                    Chantall.DeltawertBerechen(erwrteterOutput);
                    Chantall.Backpropagation();

                    Fehler += Chantall.GetFehlerDif(erwrteterOutput);

                    if (i % ausgebenPro == 0)
                    {
                        Fehler /= ausgebenPro;
                        DigitImage dImage = new DigitImage(pixels, lbl);
                        Console.WriteLine(dImage.ToString());
                        ZweiArrayAusgeben(erwrteterOutput, Chantall.GetOutput());
                        Console.WriteLine("Treffer Quote: {0}", 100 - (Fehler * 10));
                        Console.WriteLine(Chantall.ToString());
                        Chantall.Etha = Fehler;

                        Fehler = 0;
                    }

                }
            }

            private static void ZweiArrayAusgeben(double[] arr0, double[] arr1)
            {
                int sollIndex = 0;
                for (int i = 0; i < arr0.Length; ++i)
                {
                    if (arr0[i] == 1)
                        sollIndex = i;
                }

                for (int i = 0; i < arr0.Length; ++i)
                {
                    if (arr0[i] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (arr1[i] > arr1[sollIndex])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.WriteLine("{0}\t{1}\t{2:F4}", i, arr0[i], arr1[i]);

                    Console.ResetColor();
                }


                Console.WriteLine("\n");
            }

            private static double[] ConvertD(byte[][] arr0)
            {
                double[] arr1 = new double[28 * 28];

                for (int i0 = 0, arr1pos = 0; i0 < 28; ++i0)
                {
                    for (int i1 = 0; i1 < 28; ++i1, ++arr1pos)
                    {
                        arr1[arr1pos] = Convert.ToDouble(arr0[i0][i1]);
                        arr1[arr1pos] /= 255;
                    }
                }
                return arr1;
            }


            private class DigitImage
            {
                public byte[][] pixels;
                public byte label;

                public DigitImage(byte[][] pixels, byte label)
                {
                    this.pixels = new byte[28][];
                    for (int i = 0; i < this.pixels.Length; ++i)
                        this.pixels[i] = new byte[28];

                    for (int i = 0; i < 28; ++i)
                        for (int j = 0; j < 28; ++j)
                            this.pixels[i][j] = pixels[i][j];

                    this.label = label;
                }

                public override string ToString()
                {
                    string s = "";
                    for (int i = 0; i < 28; ++i)
                    {
                        for (int j = 0; j < 28; ++j)
                        {
                            if (this.pixels[i][j] == 0)
                                s += " "; // white
                            else if (this.pixels[i][j] > 240)
                                s += "█"; // black
                            else if (this.pixels[i][j] > 170)
                                s += "0";
                            else if (this.pixels[i][j] > 155)
                                s += "O";
                            else
                                s += "."; // gray
                        }
                        s += "\n";
                    }
                    s += this.label.ToString();
                    return s;
                } // ToString

            }
        }

        public static void SimplesSpielStart()
        {
            Q_Learning Tim3 = new Q_Learning(0.96, 0.1, 4, 0, "r", "l", "o", "u");
            SimplesSpiel.Setup(5, 5, 0,1);
            int action;
            SimpleNet Prediction = new SimpleNet(4, 29,30 ,25);
            Prediction.SetAfNames(AF.Linear, AF.Sigmoid,AF.Sigmoid);
            Prediction.SetAfT(1, 1);
            Prediction.SetAfT(2, 1);

            for (int i = 0; i < 100000; ++i)
            {
                double[] arrAckt = new double[4];

                if (SimplesSpiel.punkte == 0 && i > 0)
                {
                    Prediction.DeltawertBerechen((double[])SimplesSpiel.GetMap().Clone());
                    Prediction.Backpropagation();
                }


                action = Tim3.Get_SetAction(SimplesSpiel.GetPlayerPos(), SimplesSpiel.punkte);

                arrAckt[action] = 1;
                if(SimplesSpiel.punkte==0)
                Prediction.SetInput(arrAckt.Concat(SimplesSpiel.GetMap()).ToArray());
                Prediction.OutputBerechnen();

                SimplesSpiel.punkte = 0;
                SimplesSpiel.PlayerBewegen(action);
                SimplesSpiel.ExtraMapAusgeben(Prediction.GetOutput());


                //System.Threading.Thread.Sleep(10);
            }

            for (int i = 0; i < 5000; ++i)
            {
                action = Tim3.Get_SetAction(SimplesSpiel.GetPlayerPos(), SimplesSpiel.punkte);
                SimplesSpiel.punkte = 0;
                SimplesSpiel.PlayerBewegen(action);
                System.Threading.Thread.Sleep(100);
            }

            Console.ResetColor();
            while (true)
            {
                Console.SetCursorPosition(14, 0);
                Console.Write("  ");
                Console.SetCursorPosition(14, 1);
                Console.Write("  ");
                Console.SetCursorPosition(0, 0);
                Console.Write("X-Koordinate: ");
                sbyte x = Convert.ToSByte(Console.ReadLine());
                Console.Write("Y-Koordinate: ");
                sbyte y = Convert.ToSByte(Console.ReadLine());

                Q_State state = Tim3.Q_Table[new SimplesSpiel.SimplPos(x, y)];

                Console.SetCursorPosition(40, 4);
                Console.Write(Math.Round(state.actionReward[1]) + "    ");

                Console.SetCursorPosition(50, 4);
                Console.Write(Math.Round(state.actionReward[0]) + "    ");

                Console.SetCursorPosition(45, 1);
                Console.Write(Math.Round(state.actionReward[2]) + "    ");

                Console.SetCursorPosition(45, 7);
                Console.Write(Math.Round(state.actionReward[3]) + "    ");

            }
        }

        public static void SimplesSpielKI()
        {
            NeuronalNet Queckeline = new NeuronalNet( 2, 100, 4);
            Queckeline.StandartInitialisierung();
        }


        public static void SimpleTetris()
        {
            Random rnd = new Random();
            SimpleNet Kevin = new SimpleNet(0.05, 281, 500, 5);
            Kevin.SetAfT(1, 2);
            Kevin.SetAfNames(AF.Tanh, AF.Sigmoid);
            Kevin.SetAfT(2, 1);


            while (true)
            {
                Console.Clear();
                Tetris.MapAktualisieren(true);


                while (Tetris.leben  && Tetris.sinnloseBewegung < 350)
                {
                    Kevin.SetInput(Tetris.GetType2V());
                    Kevin.NeuronValue[0][280] = rnd.NextDouble() * 20 - 10;
                    Kevin.OutputBerechnen();


                    Tetris.BewegungShow(Kevin.GetOutput(),0.7);

                    //if ((Tetris.sinnloseBewegung + 1) % 70 == 0)
                    //{
                    //    Kevin.ReinforcementLearning(-0.01);
                    //}


                    if (Tetris.punkte != 0)
                    {
                        Kevin.ReinforcementLearning(Tetris.punkte,false);
                    }

                    

                    Console.SetCursorPosition(0, 0);
                    ArrayAusgeben(Kevin.GetOutput());
                    Console.Write(Tetris.punkte);

                    

                    //Console.SetCursorPosition(0, 35);
                    //Console.Write(TetrisKI[iKi].ToString());

                    
                    Tetris.punkte = 0;
                }

                System.Threading.Thread.Sleep(100);
                Kevin.ReinforcementLearning(-1,false);
                

                Tetris.AllesReset();

            }
        }

    }
}
