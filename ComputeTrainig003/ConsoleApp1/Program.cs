using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace MultithreadSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var queue = PrepareQueue();

            var t1 = new Thread(ThreadProc);
            var t2 = new Thread(ThreadProc);
            var t3 = new Thread(ThreadProc);
            var t4 = new Thread(ThreadProc);

            t1.Start(queue);
            t2.Start(queue);
            t3.Start(queue);
            t4.Start(queue);

            while (queue.Count > 0)
            {
                Thread.Sleep(1000);
            }

            Console.WriteLine("-------------------------------");
            Console.WriteLine("Finished!");
            Console.ReadKey();
        }

        static Queue<int> PrepareQueue()
        {
            var queue = new Queue<int>();
            var random = new System.Random();

            for (int i = 0; i < 100; i++)
            {
                queue.Enqueue(random.Next(100));
            }

            return queue;
        }

        static void ThreadProc(object o)
        {
            var queue = (Queue<int>)o;
            var random = new System.Random();

            while (true)
            {
                int val = 0;
                int d1 = 0;
                int d2 = 0;

                String Name = System.Threading.Thread.CurrentThread.Name;
                int id = System.Threading.Thread.CurrentThread.ManagedThreadId;

                // Dequeu a value and split them to each digit.
                try
                {
                    if (!queue.TryDequeue(out val))
                    {
                        // failing to dequeue means empty
                        break;
                    }
                    val++;
                    d1 = val % 10;
                    d2 = (val / 10) % 10;

                    if (d1 == 0 && d2 == 0)
                    {
                       // val should be over 0. If val = 0, return Exception.
                       Console.WriteLine("{0},{1},{2}",d1,d2,val);
                       throw new Exception("Input number has to be over 0");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("-------------------------------");
                    Console.Error.WriteLine("Error!! - {0}", ex.Message);

                    Trace.WriteLine(String.Format("d1={0},d2={1},val={2}", d1, d2, val));
                    Trace.WriteLine("ThreadID : " + id);
                    Trace.WriteLine("stack trace: ");
                    Trace.WriteLine(ex.StackTrace);
                    continue;
                }

                // show digit from here
                Console.WriteLine("Digit1 = {0}, Digit2 = {1}, Number = {1}{0}", d1, d2);
                string[] digit1_lines = MakeLines(d1);
                string[] digit2_lines = MakeLines(d2);

                for (int i = 0; i < digit1_lines.Length; i++)
                {
                    Console.WriteLine("{0} {1}", digit2_lines[i], digit1_lines[i]);
                }
                Console.WriteLine();

                // wait random msec
                int wait_msec = (random.Next(5) + 1) * 100;
                Thread.Sleep(wait_msec);
            }
        }

        static string[] MakeLines(int d)
        {
            var bit_array = GetBitArrayForDisplay(d);
            string[] digit_lines = new string[9];

            for (int i = 0; i < 7; i++)
            {
                switch (i)
                {
                    case 0:
                        if ((bit_array & 0b0100_0000) > 0)
                        {
                            digit_lines[0] = " *** ";
                        }
                        else
                        {
                            digit_lines[0] = "     ";
                        }
                        break;

                    case 1:
                        if ((bit_array & 0b0010_0000) > 0)
                        {
                            digit_lines[1] = digit_lines[2] = digit_lines[3] = "*  ";
                        }
                        else
                        {
                            digit_lines[1] = digit_lines[2] = digit_lines[3] = "   ";
                        }
                        break;

                    case 2:
                        if ((bit_array & 0b0001_0000) > 0)
                        {
                            digit_lines[1] += " *";
                            digit_lines[2] += " *";
                            digit_lines[3] += " *";
                        }
                        else
                        {
                            digit_lines[1] += "  ";
                            digit_lines[2] += "  ";
                            digit_lines[3] += "  ";
                        }
                        break;

                    case 3:
                        if ((bit_array & 0b0000_1000) > 0)
                        {
                            digit_lines[4] = " *** ";
                        }
                        else
                        {
                            digit_lines[4] = "     ";
                        }
                        break;

                    case 4:
                        if ((bit_array & 0b0000_0100) > 0)
                        {
                            digit_lines[5] = digit_lines[6] = digit_lines[7] = "*  ";
                        }
                        else
                        {
                            digit_lines[5] = digit_lines[6] = digit_lines[7] = "   ";
                        }
                        break;

                    case 5:
                        if ((bit_array & 0b0000_0010) > 0)
                        {
                            digit_lines[5] += " *";
                            digit_lines[6] += " *";
                            digit_lines[7] += " *";
                        }
                        else
                        {
                            digit_lines[5] += "  ";
                            digit_lines[6] += "  ";
                            digit_lines[7] += "  ";
                        }
                        break;

                    case 6:
                        if ((bit_array & 0b0000_0001) > 0)
                        {
                            digit_lines[8] = " *** ";
                        }
                        else
                        {
                            digit_lines[8] = "     ";
                        }
                        break;
                }
            }
            return digit_lines;
        }

        static int GetBitArrayForDisplay(int d)
        {
            int bit_array = 0b0111_1111;
            switch (d)
            {
                case 0:
                    bit_array = 0b0111_0111;
                    break;

                case 1:
                    bit_array = 0b0001_0010;
                    break;

                case 2:
                    bit_array = 0b0101_1101;
                    break;

                case 3:
                    bit_array = 0b0101_1011;
                    break;

                case 4:
                    bit_array = 0b0011_1010;
                    break;

                case 5:
                    bit_array = 0b0110_1011;
                    break;

                case 6:
                    bit_array = 0b0110_1111;
                    break;

                case 7:
                    bit_array = 0b0101_0010;
                    break;

                case 8:
                    bit_array = 0b0111_1111;
                    break;

                case 9:
                    bit_array = 0b0111_1011;
                    break;

            }

            return bit_array;
        }
    }
}
