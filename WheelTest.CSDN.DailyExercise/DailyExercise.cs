using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using WheelTest.Style;

namespace WheelTest.CSDN.DailyExercise
{
    public class DailyExercise
    {
        #region 2023-2-18
        /// <summary>
        /// 1
        /// </summary>
        public void DailyExercise2023020181()
        {
            Console.WriteLine(Resource._2023_2_18_1);
        rety:
            Console.WriteLine("请输入n");
            string str_0 = Console.ReadLine();
            int n = 0;
            if (int.TryParse(str_0, out n))
            {
            rety1:
                Console.WriteLine("请输入 " + n * n + " 个数");
                string str_1 = Console.ReadLine();
                var ns = str_1.Split(' ');
                if (ns.ToArray().IsEmptyAndGreaterThanZero()&& ns.ToArray().Count()== n * n)
                {
                    List<List<int>> stringss = new List<List<int>>();
                    Console.WriteLine("...............................");
                    for (int i = 0; i < n; i++)
                    {
                        List<int> strings = new List<int>();
                        for (int b = 0; b < n; b++)
                        {
                            strings.Add(int.Parse(ns[i*n+b]));
                        }
                        Console.WriteLine(string.Join(" ", strings));
                        stringss.Add(strings);
                    }
                    List<DictionarySize> ints =new List<DictionarySize>();
                    {
                        ints.Add(new DictionarySize { X = 0, Y = 0, ultimately = stringss[0][0] });
                        for (int i = 0; i < (2 * (n - 1)); i++)
                        {
                            var length = ints.Count;
                            for (int v = 0; v < length;v++)
                            {
                                var item = ints[v];
                                var x = item.X;
                                var y = item.Y;
                                var ultimately = item.ultimately;
                                try
                                {
                                    if (x == n - 1)
                                    {
                                        //y++
                                        {
                                            item.Y = y + 1;
                                            item.ultimately = item.ultimately + stringss[item.X][item.Y];
                                        }
                                    }
                                    else if (y == n - 1)
                                    {
                                        //X++
                                        {
                                            item.X = x + 1;
                                            item.ultimately = item.ultimately + stringss[item.X][item.Y];
                                        }
                                    }
                                    else
                                    {
                                        //X++
                                        {
                                            item.X = x + 1;
                                            item.ultimately = item.ultimately + stringss[item.X][item.Y];
                                        }
                                        //y++
                                        {
                                            ints.Add(new DictionarySize { X = x, Y = y + 1, ultimately = ultimately + stringss[x][y + 1] });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    Console.WriteLine(ints.Min(o=>o.ultimately));
                }
                else 
                {
                    goto rety1;
                }
            }
            else 
            { 
                Console.WriteLine("请输入数字");
                goto rety; 
            }
        }
        class DictionarySize
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int ultimately { get; set; } 
        }
        #endregion
    }
}
