using System;
using System.Linq;

namespace IZhy.Common.SimpleTools
{
    /// <summary>
    /// 一个简单的数组工具类
    /// </summary>
    public static class ArrayTool
    {
        #region 数值型 数组 冒泡法排序

        /// <summary>
        /// 冒泡法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static byte[] BubbleSort(byte[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            byte temp;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            return arr;
        }

        /// <summary>
        /// 冒泡法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static sbyte[] BubbleSort(sbyte[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            sbyte temp;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            return arr;
        }


        /// <summary>
        /// 冒泡法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static short[] BubbleSort(short[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            short temp;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            return arr;
        }

        /// <summary>
        /// 冒泡法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static ushort[] BubbleSort(ushort[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            ushort temp;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            return arr;
        }


        /// <summary>
        /// 冒泡法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static int[] BubbleSort(int[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            int temp;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            return arr;
        }

        /// <summary>
        /// 冒泡法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static uint[] BubbleSort(uint[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            uint temp;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            return arr;
        }


        /// <summary>
        /// 冒泡法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static long[] BubbleSort(long[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            long temp;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            return arr;
        }

        /// <summary>
        /// 冒泡法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static ulong[] BubbleSort(ulong[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            ulong temp;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            return arr;
        }


        /// <summary>
        /// 冒泡法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static float[] BubbleSort(float[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            float temp;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            return arr;
        }

        /// <summary>
        /// 冒泡法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static double[] BubbleSort(double[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            double temp;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            return arr;
        }

        /// <summary>
        /// 冒泡法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static decimal[] BubbleSort(decimal[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            decimal temp;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            return arr;
        }


        /// <summary>
        /// 冒泡法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static DateTime[] BubbleSort(DateTime[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            DateTime temp;
            for (int i = 1; i < arr.Length; i++)
            {
                for (int j = arr.Length - 1; j >= i; j--)
                {
                    if (arr[j] < arr[j - 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j - 1];
                        arr[j - 1] = temp;
                    }
                }
            }
            return arr;
        }

        #endregion

        #region 数值型 数组 插入法排序

        /// <summary>
        /// 插入法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static byte[] InsertionSort(byte[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            for (int i = 1; i < arr.Length; i++)
            {
                byte temp = arr[i];
                int index = i - 1;
                while (index >= 0 && temp < arr[index])
                {
                    arr[index + 1] = arr[index];
                    index--;
                }
                arr[index + 1] = temp;
            }
            return arr;
        }

        /// <summary>
        /// 插入法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static sbyte[] InsertionSort(sbyte[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            for (int i = 1; i < arr.Length; i++)
            {
                sbyte temp = arr[i];
                int index = i - 1;
                while (index >= 0 && temp < arr[index])
                {
                    arr[index + 1] = arr[index];
                    index--;
                }
                arr[index + 1] = temp;
            }
            return arr;
        }


        /// <summary>
        /// 插入法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static short[] InsertionSort(short[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            for (int i = 1; i < arr.Length; i++)
            {
                short temp = arr[i];
                int index = i - 1;
                while (index >= 0 && temp < arr[index])
                {
                    arr[index + 1] = arr[index];
                    index--;
                }
                arr[index + 1] = temp;
            }
            return arr;
        }

        /// <summary>
        /// 插入法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static ushort[] InsertionSort(ushort[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            for (int i = 1; i < arr.Length; i++)
            {
                ushort temp = arr[i];
                int index = i - 1;
                while (index >= 0 && temp < arr[index])
                {
                    arr[index + 1] = arr[index];
                    index--;
                }
                arr[index + 1] = temp;
            }
            return arr;
        }


        /// <summary>
        /// 插入法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static int[] InsertionSort(int[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            for (int i = 1; i < arr.Length; i++)
            {
                int temp = arr[i];
                int index = i - 1;
                while (index >= 0 && temp < arr[index])
                {
                    arr[index + 1] = arr[index];
                    index--;
                }
                arr[index + 1] = temp;
            }
            return arr;
        }

        /// <summary>
        /// 插入法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static uint[] InsertionSort(uint[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            for (int i = 1; i < arr.Length; i++)
            {
                uint temp = arr[i];
                int index = i - 1;
                while (index >= 0 && temp < arr[index])
                {
                    arr[index + 1] = arr[index];
                    index--;
                }
                arr[index + 1] = temp;
            }
            return arr;
        }


        /// <summary>
        /// 插入法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static long[] InsertionSort(long[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            for (int i = 1; i < arr.Length; i++)
            {
                long temp = arr[i];
                int index = i - 1;
                while (index >= 0 && temp < arr[index])
                {
                    arr[index + 1] = arr[index];
                    index--;
                }
                arr[index + 1] = temp;
            }
            return arr;
        }

        /// <summary>
        /// 插入法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static ulong[] InsertionSort(ulong[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            for (int i = 1; i < arr.Length; i++)
            {
                ulong temp = arr[i];
                int index = i - 1;
                while (index >= 0 && temp < arr[index])
                {
                    arr[index + 1] = arr[index];
                    index--;
                }
                arr[index + 1] = temp;
            }
            return arr;
        }


        /// <summary>
        /// 插入法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static float[] InsertionSort(float[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            for (int i = 1; i < arr.Length; i++)
            {
                float temp = arr[i];
                int index = i - 1;
                while (index >= 0 && temp < arr[index])
                {
                    arr[index + 1] = arr[index];
                    index--;
                }
                arr[index + 1] = temp;
            }
            return arr;
        }

        /// <summary>
        /// 插入法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static double[] InsertionSort(double[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            for (int i = 1; i < arr.Length; i++)
            {
                double temp = arr[i];
                int index = i - 1;
                while (index >= 0 && temp < arr[index])
                {
                    arr[index + 1] = arr[index];
                    index--;
                }
                arr[index + 1] = temp;
            }
            return arr;
        }

        /// <summary>
        /// 插入法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static decimal[] InsertionSort(decimal[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            for (int i = 1; i < arr.Length; i++)
            {
                decimal temp = arr[i];
                int index = i - 1;
                while (index >= 0 && temp < arr[index])
                {
                    arr[index + 1] = arr[index];
                    index--;
                }
                arr[index + 1] = temp;
            }
            return arr;
        }


        /// <summary>
        /// 插入法数组排序
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static DateTime[] InsertionSort(DateTime[] arr)
        {
            if (arr == null || arr.Length < 1)
            {
                return null;
            }
            for (int i = 1; i < arr.Length; i++)
            {
                DateTime temp = arr[i];
                int index = i - 1;
                while (index >= 0 && temp < arr[index])
                {
                    arr[index + 1] = arr[index];
                    index--;
                }
                arr[index + 1] = temp;
            }
            return arr;
        }

        #endregion


        /// <summary>
        /// 产生一个[minimum ~ maximum]的数组（可随机排序）
        /// <para>数组中包含 minimum 和 maximum</para>
        /// </summary>
        /// <param name="minimum">最小值</param>
        /// <param name="maximum">最大值（必须大于 minimum）</param>
        /// <param name="isRandom">返回的数组是否随机排序，默认：false 不随机排序；true 随机排序</param>
        /// <returns></returns>
        public static int[] RandomPermutationNum(int minimum, int maximum, bool isRandom = false)
        {
            if (maximum <= minimum)
            {
                return null;
            }

            int[] originalArr = new int[maximum - minimum + 1]; //声明原始数组

            for (int i = 0; i < originalArr.Length; i++)
            {
                originalArr[i] = minimum;
                minimum++;
            }

            if (isRandom)
            {
                //随机排序后的新数组
                int[] randomArr = originalArr.OrderBy(x => Guid.NewGuid()).ToArray();
                return randomArr;
            }
            else
            {
                return originalArr;
            }
        }
    }
}
