using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojBroj
{
    public static class ExtensionMethods
    {
        public static List<List<T>> GetAllPermutations<T>(this List<T> list)
        {
            List<List<T>> outList = new List<List<T>>();
            int x = list.Count - 1;
            GetPermutation(list, 0, x, ref outList);

            return outList;
        }

        private static void GetPermutation<T>(List<T> list, int k, int m, ref List<List<T>> outList)
        {
            if (k == m)
            {
                List<T> newList = new List<T>();

                foreach (T item in list)
                {
                    newList.Add(item);
                }

                outList.Add(newList);
            }
            else
            {
                for (int i = k; i <= m; i++)
                {
                    SwapListItem(ref list, k, i);
                    GetPermutation(list, k + 1, m, ref outList);
                    SwapListItem(ref list, k, i);
                }
            }
        }

        private static void SwapListItem<T>(ref List<T> list, int index, int withIndex)
        {
            T tmp = list[index];
            list[index] = list[withIndex];
            list[withIndex] = tmp;
        }
    }
}
